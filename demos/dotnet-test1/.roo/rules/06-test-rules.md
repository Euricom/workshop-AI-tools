# Test Rules (Unit Testing Best Practices)

*(If using a dedicated test rules file, these rules focus on the structure and conventions for test code, especially with xUnit. This ensures tests are organized and written in a way that is consistent, reliable, and easy to understand.)*

- **Test Project Structure:**
    - Place tests in a separate project (or projects) named with a `.Tests` suffix corresponding to the target project/module, typically under a `tests/` directory in the solution. For example, to test `MyApp.Core`, use a project `MyApp.Core.Tests`; for API, `MyApp.API.Tests`, etc.
    - This naming makes it clear what is being tested. By keeping tests separate from production code (often in a parallel folder structure), we ensure no accidental dependencies from production code on test libraries and keep shipping binaries lean.
    - The `.Tests` projects can have access to internals of the main project via `[InternalsVisibleTo]` if needed.
    - Enforce the rule that test projects should reference the code projects, not the other way around, and no production code lives in the test assemblies.
    - If a test class or file is found in a non-test project (e.g., someone wrote a quick console app to test something in the main code), suggest moving it to the proper test project.

- **Test Class & File Naming:**
    - Name test classes after the unit under test (usually the class or feature being tested) with `Tests` as a suffix. For example, `OrderServiceTests` contains tests for `OrderService` functionality.
    - Alternatively, some prefer using a mirror namespace structure under the `.Tests` project. Either way, be consistent.
    - Each test file typically contains one test class (or a couple related classes’ tests). The test class can be in the same namespace as the class under test (except with `.Tests` appended in the project name).
    - If tests are grouped by behavior/scenario rather than by class, make that clear in naming (e.g., `OrderCancellationTests` focusing on a use-case).
    - The rules should encourage one concept per test class and one behavior per test method for clarity.

- **Test Method Naming:**
    - Use descriptive names for test methods that clearly state what is being tested and what the expected outcome is.
    - A common convention is `MethodName_Scenario_ExpectedResult`. For instance, `CalculatePrice_DiscountApplied_ReturnsReducedTotal()` is much clearer than `TestDiscount()` or `CalculatePriceTest1`.
    - This three-part naming (method under test, conditions, result) serves as documentation for behavior.
    - Enforce that test method names do not have generic names like “Test1” or duplicate the production method name without context.
    - When using xUnit, tests are methods decorated with `[Fact]` or `[Theory]`; ensure these attributes are present.

- **Use of xUnit Attributes:**
    - Use `[Fact]` for a test with no parameters (a single scenario).
    - Use `[Theory]` for parameterized tests, combined with `[InlineData(...)]` or other data sources to supply multiple cases.
    - If using `[Theory]`, ensure the method has parameters matching the data provided.
    - Use a parameterless constructor or `IClassFixture` for setup that is common for many tests (xUnit doesn’t have `[SetUp]`, but the concept is similar).

- **AAA Pattern (Arrange-Act-Assert):**
    1. **Arrange**: set up the objects and state needed (initialize the system under test, stub out dependencies, set inputs).
    2. **Act**: execute the behavior being tested (call the method or function under test).
    3. **Assert**: verify the outcome (check that results match expectations).
    - Structure each test method to have these sections clearly separated (you can use blank lines or comments like `// Arrange`, `// Act`, `// Assert`).
    - If a test method has multiple asserts and complex setup interwoven, suggest refactoring. One test should test one thing; if multiple asserts test different aspects, they should all relate to the single scenario.

- **One Assert Per Test (Principle):**
    - Encourage that each test focuses on one logical assertion.
    - It is fine to have multiple `Assert` calls if they together verify one outcome, but avoid testing unrelated things in one test.
    - Avoid asserting in loops or complex logic inside tests – that can hide which case failed. Use `[Theory]` for multiple cases.

- **No Logic in Tests:**
    - Tests should be straightforward—generally no significant logic themselves (no big if/else, no loops constructing complex scenarios dynamically).
    - If you find yourself writing a loop or conditional in a test, consider if there’s a simpler way or if that indicates multiple test cases.
    - Tests should not call each other—each test should be independent, and each test should set up its own data (or use shared fixtures).

- **Use Test Fixtures/Helpers Appropriately:**
    - Factor repeated setup into xUnit fixtures or builder patterns (e.g., `OrderBuilder` for creating default objects).
    - Use xUnit’s `IClassFixture<T>` to share expensive context (like database setup) across tests in a class.
    - Use `[CollectionDefinition]` in xUnit for integration tests that share context, to avoid parallel conflicts.

- **Test Data and Constants:**
    - Avoid using “magic values” in tests that are not obvious. Assign them to well-named constants or variables in the arrange section.
    - Where possible, use realistic data (e.g., `firstName = "John"`) to make tests more explanatory.

- **Testing Practices:**
    - Ensure tests run independently and reliably.
    - No test should depend on another test’s outcome or data.
    - Tests should be idempotent – running them multiple times yields the same result.
    - For external dependencies (database, file system, network), use test doubles or in-memory alternatives.
    - For integration tests that touch external systems, separate them or mark clearly. Unit tests should not rely on external I/O.
    - Use Moq or similar frameworks to mock dependencies rather than using actual implementations.

- **Test Coverage:**
    - Critical paths should have tests. For any bug fixes, add a regression test.
    - Ensure edge cases are tested (null inputs, zero/negative values, upper bounds, etc.).
    - Coverage should focus on meaningful logic, not just line coverage.

- **xUnit Specifics:**
    - xUnit doesn’t use `[TestInitialize]` or `[TestCleanup]` (use constructors for setup and `Dispose` for cleanup).
    - xUnit runs tests in parallel by default. If tests are not written with isolation, use `[Collection]` to control parallelism.
    - Mark test collections `[CollectionDefinition]` appropriately if using shared state.

> By following these testing rules, the project’s test suite will be well-structured, readable, and maintainable. The Roo assistant, when in test mode, will pay special attention to these guidelines—ensuring that any generated test code or test suggestions adhere to xUnit best practices and the patterns outlined above.