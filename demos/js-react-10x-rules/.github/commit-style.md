# Commit Style Guide

We follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) style for our commit messages

Rules:  
- use lower case for the complete commit messages (summary line and description)
- use the imperative mood in the summary line
- keep the summary line concise

The first line of the commit message should be a short summary of the changes made. This should be followed by a blank line and then a more detailed description of the changes, if necessary.

The summary line starts with a type** (`feat`, `fix`, `docs`, etc.) based on the primary intent 
Use these types:
- `feat: add new user authentication module`
- `fix: resolve issue with data fetching.`
- `docs: update README with installation instructions`
- `style: format code with Prettier`
- `refactor: improve performance of data processing`
- `test: add unit tests for user service`
- `chore: update dependencies, scripts and build stuff`

Additionally, a scope is added to the commit message in parentheses to specify the area of the codebase that is affected. The scope should be a short, lowercase identifier that describes the part of the codebase being modified. For example, if you are making changes to the authentication module, you might use `auth` as the scope.
An example would be:
- `fix(auth): resolve issue with user login`
- `feat(auth): add new user authentication module`

The commit message should also include a detailed description of the changes made in the commit after the summary line if necessary.
Always use a bullet list to describe the changes made in the commit. This makes it easier to read and understand the changes at a glance.
