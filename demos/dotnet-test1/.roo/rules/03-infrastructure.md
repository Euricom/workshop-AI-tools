# Entity Framework & Data Access Rules (Infrastructure Layer)

Govern the use of Entity Framework Core and the repository/unit-of-work patterns, ensuring they are properly confined to the Infrastructure layer. Prevent business or presentation logic from leaking into data access, and vice versa, enforcing a clean separation for persistence concerns.

---

## EF Core in Infrastructure Only

- All usage of Entity Framework Core (or any OR/M or data access technology) **must be restricted to the Infrastructure project**.
    - Classes like `DbContext` and EF configuration (e.g., `OnModelCreating`, EF entity type configurations), migration files, and any `DbSet<TEntity>` definitions live in Infrastructure.
    - **No other layer should directly use EF Core types.**
    - Core and Application layers should be **agnostic of EF** – they operate in terms of repository interfaces or abstraction.
    - Referencing EF-specific attributes or methods indicates the code belongs in Infrastructure.
    - **Rule violation example:** a service in Application directly instantiates a `DbContext` or executes LINQ on a `DbSet` – that should trigger an error:  
    *“Entity Framework usage outside Infrastructure layer is not allowed.”*

---

## Repository Pattern Enforcement

- Data access should be done via the **Repository pattern**.
    - Define repository interfaces in an inner layer (commonly in Core or Application, e.g., `IRepository<T>` or more specific ones like `ICustomerRepository`).
    - Infrastructure implements them (e.g., `EfCustomerRepository : ICustomerRepository`).
    - Ensures Application logic speaks to an abstraction and doesn’t depend on EF.
- Any class in Application or Core that needs to query the database **uses an interface** (like `IRepository<T>.Find(...)` or sending a query via MediatR) rather than directly querying EF.
    - Forbid direct usage:  
      `using (var context = new DbContext())` outside Infrastructure.
    - By adhering to this pattern, the application core only holds the interface, and the implementation is at the edges.
    - **If a repository interface doesn’t exist yet for some data operation, create one rather than working around it.**
- **Compliance message:**  
  *“Use repository interfaces for data access. Direct data queries in this layer violate the Onion Architecture.”*

---

## Unit of Work

- If multiple repository operations need to be atomic (transactional), **use an `IUnitOfWork` abstraction**.
    - The Unit of Work interface might expose a method like `SaveChanges()` or `Commit`, and possibly access to various repositories.
    - Infrastructure ties this to the `DbContext` (e.g., `EfUnitOfWork` calls `DbContext.SaveChanges()`).
    - Application layer uses `IUnitOfWork` (injected) to commit transactions, with no knowledge of `DbContext`.
    - Rules should **encourage grouping operations via Unit of Work** rather than managing transactions in Application.
        - Example: if an Application service calls two repositories, both should commit under one UnitOfWork.
    - **Violation:** Application instantiating a transaction or calling EF save directly.
    - **Pattern:**  
      `using(var uow = _unitOfWork) { ... await uow.SaveChangesAsync(); }`
    - **Rule:**  
      *“Coordinate EF operations through a Unit of Work interface to maintain atomicity and keep transaction logic out of application code.”*

---

## No Business Logic in Repositories/EF

- Data access layer is limited to data mapping and persistence.
    - **Business logic (calculations, decisions, enforcing business rules) does NOT belong in repository methods or DbContext.**
        - Example: Calculating discounts or sending emails in a repository is not allowed.
    - **If complex logic exists in repository:** it likely belongs in Application or Domain.
    - Repository methods should focus on fetching or persisting data, translating queries to domain objects or DTOs.
    - Avoid calling business services inside repositories.
    - **Guideline message:**  
      *“Avoid business logic in data access layer – move this to Application/Core.”*
    - Business layer should not directly use EF features like tracking or lazy-loading; these are encapsulated in Infrastructure.

---

## No EF Leakage to Other Layers

- Prevent “leakage” of persistence concerns to Application or Presentation layers.
    - Application should not handle EF’s IQueryable or Include expressions, or any connection string/database transaction logic.
    - Infrastructure should shield the rest of the system from these details.
    - Repository interfaces may return domain entities or read models, but **never IQueryable<T>** that lets Application compose EF queries.
    - EF-specific exceptions or behaviors should be handled at the Infrastructure boundary (e.g., wrap `DbUpdateException` into a domain-specific exception).
    - Core/Application should be **unaware that EF is being used**.
    - **Rule violation:** Any class outside Infrastructure using `Microsoft.EntityFrameworkCore`, `System.Data.SqlClient`, etc.
    - **No data access in controllers:** Controllers should call Application, which calls repository. A controller should NEVER call `dbContext.Entities.ToList()` itself.
    - **Guidance:**  
      *“There are no database applications, just applications that use a database via external infrastructure code.”*

---

## Consistent Persistence Patterns

- Encourage repository-per-aggregate or per-entity patterns; **discourage anemic or inconsistent data access**.
    - Decide if using a generic repository for all entities or specific ones — and stick to it.
    - If using specific repositories, ensure clear responsibility (e.g., `OrderRepository` handles Order aggregate queries).
    - For more complex queries, consider Specification pattern (query object to repository) rather than leaking query logic out.
    - The rules can’t enforce a specific pattern beyond repository/Unit of Work, but should give guidance:
    - **Guidance:**  
      *“Prefer repository methods that return domain entities or DTOs, rather than exposing raw EF data structures.”*
- This abstraction keeps Infrastructure **interchangeable** (e.g., could swap EF for Dapper by only changing Infrastructure).
- Adding cache or additional data sources stays in Infrastructure, thanks to this abstraction.
