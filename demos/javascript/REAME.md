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
- When creating unit test, try to run then with `npm run test:ci`
```

## Create a markdown to html convertor

```
create a function that converts a markdown string to an html string
```

Normally:
- generate by AI
- test yourself
- see its not working 

### Improve

- Enable Yolo mode (auto run)

```
You are allowed to run any type of tests including vitest, npm run test, and npm run test:ci. You can also execute build commands like tsc, create new files, and make directories (using touch, mkdir, etc). Never delete existing files.
```

New and improved prompt:

```
create a function that converts a markdown string to an html string

write the tests first, then the code, then run the tests and update the code until the tests pass.
```