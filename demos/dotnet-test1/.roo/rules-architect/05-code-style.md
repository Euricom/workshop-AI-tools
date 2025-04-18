# Code Style & Convention Rules (Naming and Formatting)

Define coding standards for the project: naming conventions, file organization, and general C# style guidelines to ensure a consistent and readable codebase. Adhering to a common style makes the development team more efficient and the code easier to maintain.
 
- **Naming Conventions:** Follow standard .NET naming conventions for all identifiers:
    - **PascalCase** for type names, namespaces, properties, methods, and public fields. Each word starts with a capital letter. For example: `OrderService`, `CustomerId`, `CalculateTotal()`. This also applies to enum names and enum members, events, etc.
    - **camelCase** for local variables and method parameters (and generally private fields if not using an underscore). The first word is lowercased, e.g., `totalAmount`, `customerRepository`.
    - **Interfaces:** Prefix interface names with `I` followed by PascalCase (e.g. `IRepository`, `IUnitOfWork`).
    - **Classes:** Use nouns or noun phrases (e.g. `OrderProcessor`), avoid prefixes/suffixes like Cls or Hungarian notation. One class per file, and file name should match the class name.
    - **Methods:** Use verb or verb-phrase names (e.g. `GetCustomerById`, `CalculateInvoiceTotal`). PascalCase these as stated.
    - **Properties:** Name as the data they hold (usually nouns/adjectives, e.g. `FirstName`, `IsArchived`).
    - **Constants:** For const or static readonly fields, also use PascalCase (e.g., `MaxRetries`). Avoid ALL_CAPS with underscores for constants in C#; the .NET practice is PascalCase even for constants.
    - **Private Fields:** We prefer either `_camelCase` or just `camelCase` (consistent within the project). A common convention is to prefix private fields with an underscore (e.g. `_repository`) to distinguish them from locals and parameters – this is allowed as an exception to the “no underscores” rule for identifiers, since it improves clarity. Choose one style and stick to it project-wide. For example, if _ prefix is used, then all private fields should follow that.
    - **Async Methods:** Postfix async method names with `Async` (e.g., `GetAllAsync`) as per .NET convention.
    - **Namespaces:** Use PascalCase and a clear hierarchy matching the project structure. Typically start with the company/organization or product name, then the layer or feature (e.g., `MyApp.Core.Entities`, `MyApp.Application.Services.Orders`). Avoid excessively deep namespace hierarchies; align them with folder structure.
    - Ensure names are descriptive and unambiguous. Avoid abbreviations unless well-known (`Id` not `ID` per .NET style, acronyms of 2 letters are both capitalized in PascalCase except for very common ones). For example, use `HtmlParser` (HTML as acronym > 2 letters, each letter capitalized) but `Id` (not ID). The rules should flag obviously non-conforming names: e.g., method `calculate_total` (snake_case) or a class `customerManager` (not capitalized) – suggest renaming to proper casing.
 
- **Formatting & Braces:** Use a consistent formatting style:
    - **Braces:** Prefer Allman style (each brace on a new line) for readability, which is common in C# (the VS default). For example:
```cs
if (condition)
{
    DoSomething();
}
else
{
    DoSomethingElse();
}
```

- **Braces:** Always use braces for conditional or loop blocks, even if they are a single line. This avoids errors when adding new lines later and is a recommended practice for clarity (no single-line `if` without braces). The only exception might be simple properties or lambda expressions where an expression-bodied member is used.
 
- **Indentation:** 4 spaces per indentation level (no tabs, or if tabs are used, they represent 4 spaces). Ensure continuity in visual indent; the rules can flag mixed tabs/spaces or incorrect indent levels.
 
- **Line Length:** Aim to keep lines reasonably short (e.g., under 120 characters) for readability. This isn’t a hard rule but long lines may indicate complex expressions that could be broken down.
 
- **Spacing:** Put a single space after keywords (`if (` not `if()`), around operators (`a + b` not `a+b`), and after commas. No trailing whitespaces at end of lines. Use blank lines to separate logical sections of code, but avoid multiple consecutive blank lines. For example, use a blank line between method definitions, and between local variable declarations and the next statement if it improves readability.
 
- **Newlines at EOF:** Ensure each file ends with a newline (some linters enforce this).
 
- The rules might auto-format or warn if braces are missing or spacing is off. For instance, if someone writes `if(condition){Do();}` on one line, the style rule should flag it: “Use braces on new lines and separate lines for statements for clarity.”
 
- **Commenting & Documentation:**
    - Code should be self-documenting where possible.
    - Use XML documentation comments (`///`) for public classes and members, especially those part of the API (even if internal to company) – describing what a method does, its parameters, and return value. This is important for the Core and Application layer where future maintainers or API consumers might rely on IntelliSense.
    - For example, document a service method with `summary`, `param`, `returns` tags. The rules could enforce that public methods in Core/Application have an XML comment.
    - For non-public code, inline comments (`//`) are fine to explain complex logic or rationale, but should be used sparingly – if the code is clear, comments are often not needed.
    - Update comments if code changes; outdated comments are misleading (the rule can’t auto-check that content, but it’s a guideline).
    - Also avoid commented-out code in the repository – if code isn’t needed, remove it (source control keeps history). A style rule could warn if large blocks of code are commented out: “Remove dead code instead of commenting it out.”
 
- **File Organization:**
    - One top-level class per file (exception: small enum or interface can share a file with a closely related class, but generally avoid multiples).
    - Filename should match the class/enum/interface name exactly (including case). If a file doesn’t follow this (e.g., class name doesn’t match file name), fix it.
    - Inside a file, order members logically: e.g., start with public properties and constructors, then public methods, then private methods (some teams use other ordering like fields at top, or region blocks – pick one). Consistent ordering makes it easier to navigate.
    - Use regions sparingly; if a class is so large you consider regions, maybe it should be split. However, organizing by regions (e.g., “// Fields”, “// Constructors”, “// Methods”) is acceptable if it helps readability.
 
- **Best Practices & Additional Analyzers:**
    - Enable Nullable Reference Types in .NET 8 (this should be on by default in new projects – check your `.csproj` for `<Nullable>enable</Nullable>`). This enforces handling of nulls at compile time and helps catch null-reference bugs. Adhere to it by marking reference types as nullable (`string?`) when null is allowed and using `?` and `!` appropriately. The rules should warn if you disable nullable or if you suppress warnings inappropriately.
    - Embrace other analyzers and code quality rules:
        - Consider using analyzers like FXCop analyzers or Roslyn rules (many are built-in by default now) to catch things like unused variables, unused async (e.g., method marked async with no await), etc.
        - If using any coding style analyzers (like StyleCop or EditorConfig conventions), ensure the rules align (for example, StyleCop might demand documentation on all public members – which we already encourage).
        - Treat warnings as errors in the project build for cleanliness (optional, but a good discipline).
    - Use language features consistently: e.g., pattern matching, LINQ, expression-bodied members when it makes code more concise and clear (but not just to golf code). For example, use null-coalescing instead of verbose `if` when appropriate: `var name = inputName ?? "default";`.
    - Favor `var` for local variable declarations when the type is obvious from the right-hand side, to reduce noise (e.g., `var service = new OrderService()` is fine, the type is clearly OrderService). Conversely, avoid `var` when the type isn’t apparent (like calling a method that returns an interface, prefer to state the interface for clarity). This balanced approach improves readability and is recommended in Microsoft’s coding conventions.
 
The code style rules should be enforced to a reasonable degree. For instance, the CI could run `dotnet format` or analyzers to ensure these are followed. The Roo Code rules here should remind the AI assistant (and indirectly the developers) to stick to this style. If code is generated or suggested that doesn’t follow these conventions, the assistant should adjust it accordingly (e.g., fix naming or add braces). Any deviation should either be justified or corrected.