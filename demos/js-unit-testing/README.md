# Unit Test First

## A simple unit test approach

- Open Cursor AI Agent Mode (ctrl-i)
- Add src/calc as context 
- Make sure the 'agent' is selected
- Make sure the model is 'auto' or claude-3.7-sonnet
  
```
Write unit tests for the calc
```

## Unit test first

Add some rules to improve unit testing

```
.cursor/rules/testing.mdc
```

Here is a example

```md
---
description: Best practices for unit testing
globs: 
alwaysApply: false
---

- Use Vitest as the unit testing framework.
- Import testing functions (e.g., `test`, `describe`) explicitly instead of relying on the global Vitest API.
- Name unit test files with the `.spec.ts` suffix (or `.spec.js` for JavaScript).
- Keep unit test files in the same directory as their corresponding source files, rather than in a separate `__tests__` directory.
- Isolate dependencies by using mocking and stubbing where necessary. Prefer `vi.spyOn` over `vi.mock` for more targeted mocking.
- DON'T run unit test with `pnpm test` or `npm run test`, it will required user input to continue. Use `pnpm run test:ci` instead for a single run. 
```

Alternative you modify/add the rules for for you IDE.

```
.windsurfrules
.github/copilot-instructions.md
.clinerules
.cursorrules
```

A naive (too simple) way of prompting what you want.

```prompt
Create a function that converts a markdown string to a html string
```

Simple way (Level 1):
- Let the AI generate the function. 
- Ask the AI to generate a unit test or test it yourself.
- see its not working and try to fix it yourself.

Lets improve this!

First give the AI the right to auto-run tools (use at your own risk)

```bash
# Cursor
options - features - autorun

# Github CoPilot
chat.tools.autoApprove
```

And now, the better prompt that improves your results.

```prompt
Create a function that converts a markdown string to an html string. Write the tests first, then the code, then run the tests and update the code until the tests pass.
```

### Fix all ppr (pre PR) errors

```
I've got some lint, build test errors, run `node run ppr` to see the errors, then fix them, and run the ppr again util ppr passes.
```