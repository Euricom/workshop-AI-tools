# General Rules (Onion Architecture Overview)

This rules file establishes the high-level Onion Architecture structure for the solution. It describes the project layout under `src/`, naming conventions, and the separation of concerns for each layer in the onion.

## Project Structure

Organize the solution into clear layers under a `src/` directory, following the Onion Architecture style. Each layer is a separate project:

- **MyApp.Core – Core Domain**
    - Contains domain models (entities, value objects), domain services, and core business logic.
    - This is the innermost layer (the “Domain”); it has **no dependencies on other projects**.

- **MyApp.Application – Application Services**
    - Contains application logic (use case coordinators, commands/queries, DTOs, service interfaces).
    - This layer implements business use-cases and orchestrates domain operations.
    - Depends on **Core** (to use domain entities/interfaces) but remains **independent of Infrastructure/API**.

- **MyApp.Infrastructure – Infrastructure**
    - Contains technical implementations for interfaces (e.g., repository classes, EF Core DbContext, file or network services).
    - This outer layer depends on **Application** (and transitively on **Core**) to implement required interfaces.
    - **Nothing in Core/Application should depend on Infrastructure** (enforced by architecture rules).

- **MyApp.API – Presentation (Web API)**
    - The ASP.NET Core Web API project with controllers, endpoints, and possibly view models.
    - Depends on **Application** (to invoke business logic via Mediator or services).
    - It may reference **Infrastructure only for composition root purposes** (e.g., registering implementations), but **API code should not directly use Infrastructure classes**.

The **Domain (Core)** is at the center, surrounded by **Application**, with **Infrastructure** and **API** on the outermost edge (along with **Tests**). All coupling is toward the center—**inner layers know nothing about outer layers**.

- **Project Naming:**  
  Use a consistent naming convention for projects and namespaces reflecting their layer. For example:
  - `<SolutionName>.Core`
  - `<SolutionName>.Application`
  - `<SolutionName>.Infrastructure`
  - `<SolutionName>.API`

  This makes it immediately clear which layer a given project belongs to. Folders within each project should mirror the feature or responsibility:
  - Under `Application`: subfolders like `Services/`, `Dtos/`, `Commands/Queries/`, etc.
  - Under `Infrastructure`: subfolders like `Persistence/`, `Repositories/`, `Services/` (for external integrations), etc.

- **Separation of Concerns:**  
  Each layer has a distinct set of responsibilities and must not mix concerns with other layers:
  - **Core** defines the **what** (business concepts and rules), independent of any particular technology.
  - **Application** defines **how** those concepts are orchestrated to fulfill use cases.
  - **Infrastructure** handles details of external interactions (data persistence, integrations).
  - **API** handles delivery concerns (HTTP, routing, serialization).

  For example:
  - Validation or business rules belong in **Application/Core**, *not* in controllers or EF code.
  - Database access belongs in **Infrastructure**, *not* in Application.

  This separation yields a **maintainable, testable system** where changes in one layer (e.g., swapping a database) do not ripple into others.

- **Independence of Core:**  
  The **Core (Domain) project should be completely independent and self-contained**. It should not depend on ASP.NET Core, EF Core, or other external frameworks. This means:
  - Domain entities are plain C# objects without framework attributes or behaviors (**persistence ignorance**).
  - Domain services and logic are written using domain models and interfaces only.

  Keeping the core pure ensures:
  - The domain can evolve or be used in different contexts (e.g., a console app or another UI) without requiring the Web API or database.
  - Outer layers can use the Core, but the Core should remain unaware of any outer layer implementations.

  *In practice, this means avoiding EF Core data annotations in Core entities; if needed for EF, use the fluent API in Infrastructure or mapping profiles.*