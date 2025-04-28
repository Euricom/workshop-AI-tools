# Controller Rules (API Layer Guidelines)

Defines standards for the API layer (controllers in the ASP.NET Core Web API). Ensure controllers remain thin and focused on HTTP concerns, delegating business logic properly, and following RESTful conventions.

---

## Controllers in API Project Only

- Controllers (and any classes inheriting from `ControllerBase` or related to HTTP endpoints) **should only exist in the MyApp.API project** (the presentation layer).
- No controllers or API endpoints should be defined in Core, Application, or Infrastructure.
- Enforce that files ending with `Controller.cs` or classes decorated with `[ApiController]` are **only in the Web API project**.
- If a controller-like class is found elsewhere, that's a violation.
- **Keep UI logic in the UI layer exclusively.**

---

## Thin Controllers (No Business Logic)

- Controllers should act as a **facade/orchestrator for HTTP request/response and nothing more**.
- No business or domain logic should be in controllers.
- Controllers **validate and parse incoming data**, then hand off to the Application layer (e.g., by calling an application service or using MediatR).
- After getting a result, the controller formats the HTTP response.
- **Rule of thumb:** If you find complex conditional logic or computations in controller actions, move it to a service or handler.
- Controllers can:
  - Authorize (via attributes)
  - Model validation (automatic via `[ApiController]`)
  - Choose which service/handler to call
- Controllers **should NOT**:
  - Access the database directly
  - Directly instantiate or operate on domain entities
  - Initiate complex domain transactions or apply business policies
- **Guidance:**  
  *“Controllers should delegate business work to the Application layer.”*

---

## Use MediatR (CQRS Pattern)

- Controllers should use MediatR (or similar mediator pattern) to handle requests/responses.
    - For commands: Controller creates Command object (e.g., `CreateOrderCommand`) and sends it to Mediator.
    - The Application layer handles the command via a handler (e.g., `CreateOrderHandler`).
    - For queries: Controller sends a Query object to the Mediator to retrieve data.
- This decouples the Web API from the implementation and promotes CQRS.
- Controllers **should not** directly call application services or repository code; go through the mediator or a public interface.
- **If not using MediatR,** call an application service, but keep it one call per action—no multi-step processes in the controller.
- **Compliance message:**  
  *“Use MediatR (or application service) to handle this operation instead of inline logic in controller.”*

---

## RESTful Routing & Conventions

- **Attribute routing:** Use `[Route("api/[controller]")]` at class level and `[HttpGet]`, `[HttpPost]`, etc. at method level.
- **Routes use nouns for resources, not verbs.**  
  E.g., `ProductsController` with GET /api/products, GET /api/products/{id}, POST /api/products, etc.
- **Appropriate HTTP methods:**
    - GET: Retrieve data (no side effects)
    - POST: Create
    - PUT/PATCH: Update
    - DELETE: Remove
    - **Avoid GET for mutations or POST for reads**
- **Status Codes:**  
  Use proper HTTP status codes:
    - 200 OK - for successful GET
    - 201 Created - for successful creation (add Location header if appropriate)
    - 204 No Content - for successful deletion
    - 400 Bad Request - for validation errors
    - 404 Not Found - if an entity is not found
    - 500 - for unhandled server errors
  The framework and `[ApiController]` help with common model binding errors.
- **Naming:**  
  Controllers' class names correspond to the resource name (e.g., `CustomerController` or `CustomersController`).  
  Methods typically: `GetAll`, `GetById`, `Create`, `Update`, `Delete` (method names are secondary to HTTP verb decoration).  
  **Stick to one naming convention for classes/routes.**
- **DTOs:**  
  Use Data Transfer Objects for requests and responses.
    - **Never expose domain entities directly in controller actions or payloads.**
    - E.g., if you have an `Order` domain entity, use an `OrderDto`.
    - Mapping between DTOs and domain models occurs in the Application layer or via mapping profiles, **not in the controller**.
    - **Rule:**  
      *“Never use domain entities to receive or return data via an API.”*
- **Consistent response shapes:**  
  All controllers should use consistent response conventions (e.g., all responses wrapped in an envelope type OR all return raw data with status codes—choose one and apply uniformly).

---

## Action Implementation

- Each controller action should be concise—typically a few lines:
    - Validate input (often automatic)
    - Call mediator or service
    - Return result
For example:
```cs
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateOrderCommand cmd) {
    var result = await _mediator.Send(cmd);
    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
}
```

In this example, the controller did minimal work. This is the pattern to encourage. If an action grows too large or starts containing try/catch for business exceptions, consider moving that logic to middleware or to the application layer. Use filters or middleware for cross-cutting concerns (logging, exception handling, authorization) rather than repeating in each action. For instance, instead of try/catch in every action to return 500 on exception, use an ASP.NET Core global exception handler or filter.

---

## Location of Logic

- Ensure things like **authorization checks** (aside from declarative `[Authorize]` attributes) or **complex validation** are in the proper place.
    - Simple validation (required fields, ranges): use model validation attributes or FluentValidation in the Application layer.
    - Authorization logic (e.g., “can the current user access this resource?”): implement as a policy or within the handler that knows about the business rules.
    - **Controllers should not contain significant if/else logic for validation/authorization** beyond maybe checking `ModelState`.
    - If you see a lot of logic in the controller, consider refactoring into handlers or middleware.

---

## Compliance Warnings

- If a controller tries to do something outside its scope, the rules should flag it.
    - **Directly accessing the database or EF Core DbContext** in a controller is wrong—should go through repository via Application.
        - *Warning:* “Controllers should not directly access the database; delegate to Application layer.”
    - If a controller instantiates domain objects and does domain processing:
        - *Prompt:* “Move business logic out of controller into Application layer.”
    - If a controller doesn’t use MediatR or any service and contains heavy logic:
        - *Suggestion:* Use the mediator pattern to separate concerns.
    - **Routing consistency:** If a controller action lacks an `[Http...]` attribute, flag as a possible mistake.
