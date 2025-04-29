# Best practices for unit testing

- Use Vitest as the unit testing framework.
- Import testing functions (e.g., `test`, `describe`) explicitly instead of relying on the global Vitest API.
- Name unit test files with the `.spec.ts` suffix (or `.spec.js` for JavaScript).
- Keep unit test files in the same directory as their corresponding source files, rather than in a separate `__tests__` directory.
- Isolate dependencies by using mocking and stubbing where necessary. Prefer `vi.spyOn` over `vi.mock` for more targeted mocking.
- DON'T run unit test with `pnpm test` or `npm run test`, it will required user input to continue. Use `pnpm run test:ci` instead for a single run. 
  
  