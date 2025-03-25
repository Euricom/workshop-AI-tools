# Demo Scenario

## Create unit test for calc

- Open Cursor AI Agent Mode (ctrl-i)
- Add src/calc as context 
- Make sure the 'agent' is selected
- Make sure the model is 'auto' or claude-3.7-sonnet
  
```
Write unit tests for the calc
```

## Make better unit tests

```md
# Best practices for unit testing with Vitest

- Use `import` for test, describe, and other testing functions
- Name unit test files with the `spec` suffix
- Place test files alongside source files, not in a separate `__tests__` folder
- Use mocking and stubbing for isolating dependencies
- Implement proper test coverage analysis
- When creating unit test, try to run then with `npm run test`
```