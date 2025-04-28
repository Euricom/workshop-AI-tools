# Architecture Rules (Layered Dependency Enforcement)

Enforce the dependency direction and allowed interactions between layers, adhering to Onion Architecture principles. It ensures that inner layers are not corrupted by references to outer layers and that all coupling points follow the defined direction (toward the core). Sqlite in-memory database is used, but the actual database can be swapped out easily.

## Dependency Direction

All code dependencies must point inward toward the Core/domain. **No inner layer should ever depend on an outer layer.** In practice, this means:

- **MyApp.Core** must **not** reference Application, Infrastructure, or API projects.
- **MyApp.Application** may reference **Core** (to use domain models or interfaces) but must **not** reference Infrastructure or API.
- **MyApp.Infrastructure** may reference **Application** and **Core** (e.g., to implement interfaces or use domain types), but must **not** reference **API** (the infrastructure should not try to call controllers or UI logic).
- **MyApp.API (Presentation)** may reference **Application** (to call business logic) and can reference **Core** if necessary (often the Application layer will make Core types available, so direct Core usage from API is minimal). The API can reference Infrastructure **only** in the startup/composition phase to register services — but **avoid direct use of infrastructure services** in controller code.

Any other dependency combinations are disallowed.  
For example, if code in Core tries to use a class from Infrastructure (outer to inner) or if Application tries to directly instantiate a `DbContext` from Infrastructure, **it violates the architecture**. These rules uphold the fundamental Onion Architecture law that code can depend on more central layers, but never on layers further out.  
A violation should raise an error or warning indicating an **“Onion architecture dependency rule violation”**.


## Allowable Layer Interactions

- Outer layers can communicate with inner layers **only through well-defined interfaces or contracts**.
- Example: If the Application layer needs to retrieve data, it should call a repository interface defined in Core/Application, **implemented by Infrastructure**.
- The API layer should call an application service or MediatR request, **not directly query the database or manipulate domain entities** without going through the Application layer.
- All interactions should respect the hierarchy:

  - UI → Application → Domain (for commands)  
  - UI → Application → Domain (or Infrastructure) (for queries)

This enforces that "all of the layers interact strictly through the interfaces defined in the layers below, and the flow of dependencies is toward the core".  
If a piece of code in an outer layer tries to bypass the layer directly beneath and reaches into a deeper layer (skipping layers), that is considered an architecture violation.

---

## No Skipping Layers

- An outer layer should **not “reach around”** the layer just inside it.
- The typical call flow in onion architecture for a web request is:  
  `Controller (Presentation) → Application service/handler → Domain (Core), possibly then down into Infrastructure for persistence.`

- You should not:
  - Have a Controller directly access the database
  - Call domain entity methods bypassing the application service (that would skip the Application layer)
  - Have the UI talk directly to Infrastructure without going through the Application

- **Rule**:  
  Do not skip over an intermediate layer when invoking functionality in a deeper layer.  
  Always go through the proper abstraction in the next inner layer. (For example, controllers call application commands/queries, which in turn coordinate domain and infrastructure work.)  
  If layer-skipping is detected (e.g., UI code directly creating a repository class from Infrastructure), a warning should prompt:  
  **"Do not bypass the Application layer – calls should flow through the proper layer sequence."**

---

## Dependency Inversion & Interfaces

- To allow inner layers to remain independent of outer implementations, rely on the **Dependency Inversion Principle (DIP)**.
- Inner layers (Core/Application) should define abstractions (interfaces or base classes) for any functionality that will be implemented in outer layers.
- Outer layers then provide concrete implementations and inject them.
- For example:
  - Core might define an `IRepository` or an `INotificationService` interface.
  - Infrastructure will implement these (e.g., `EfRepository`, `SmtpNotificationService`).
  - The API (startup) will inject the concrete into the Application/Core via an IoC container.

- This way, the Core/Application can call interface methods without depending on the concrete classes.

  - For any external interaction (database, file, network), the interface/contract **is owned by an inner layer and the implementation resides in an outer layer**.

- **Violations**:
  - An inner layer class directly instantiating an `HttpClient` or `DbContext` – instead, those should be abstracted and injected.

- **Guidance**:  
  "Depend on abstractions, not concretions; define interfaces in Core/Application and implement them in Infrastructure."

---

## Module Boundaries

- Each layer’s code should **stay within its boundary**.
- Cross-layer concerns should be minimized or handled via well-defined patterns (e.g., use middleware or filters in API for cross-cutting concerns like logging or auth, rather than putting those in multiple layers).
- If something doesn’t clearly belong to a single layer (e.g., validation), decide on a consistent approach (maybe domain-level validation in entities for invariants vs. DTO validation in API/Application) and stick to it.

- This rule file should encourage keeping the architecture cleanly separated.
  - If code is found in the wrong place (like an EF `DbContext` appearing in Application layer code, or business rules in a controller), it should flag it as a concern.
  - The message could be:
    - **"Separation of concerns violated: this code belongs in {CorrectLayer} layer, not in {CurrentLayer}."**