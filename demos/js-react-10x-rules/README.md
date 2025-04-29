# JS React 10X Rules

This is a sample React project showcasing the application of advanced AI rules. 
It demonstrates how custom rules can enhance daily workflows and boost productivity.

## Rules files (per IDE)

**Github Copilot**

- AI's context window: 2k-8k

```bash
# location of Copilot rule files
.github/copilot-instructions.md
.vscode/settings.json    # config to customize custom rules
.github/prompts/         # preview feature
```

[Copilot Custom Instructions](https://docs.github.com/en/copilot/customizing-copilot/adding-repository-custom-instructions-for-github-copilot)

**RooCode**

```bash
# location of RooCode rules
.roo/rules/
```

[RooCode Custom Instructions](https://docs.roocode.com/features/custom-instructions)

**Cursor**

Cursor rules, identified by the `.mdc` extension, utilize frontmatter to define the scope and applicability of each rule.
There's no enforced file size restriction for .cursorrules or .mdc files, the AI's effective context window (reportedly ~10,000 tokens) means excessive rules risk being partially ignored.

```bash
# location of Cursor rules
.cursor/rules/
```

[Cursor Rule Docs](https://docs.cursor.com/context/rules)

**Cline**

```bash
# name of Cline rules folder
.clinerules/
```

[Cline Rules Folder](https://docs.cline.bot/improving-your-prompting-skills/prompting#clinerules-folder-system)

**Windsurf**

- Windsurf uses a single rule file and currently does not support multiple rule files.
- The `.windsurfrules` is limited to 6000 chars! 
- There are Global and Workspace AI Rules

```bash
# name of Windsurf rule file
.windsurfrules
```

[Cursor Rule Docs](https://docs.cursor.com/context/rules)

**JetBrains AI Assistant**

Custom prompts for JetBrains AI Assistant can only be defined within the UI. File-based rule definitions are not currently supported.

**Support for multiple IDE**

See [Github - Bhartendu-Kumar/rules_template](https://github.com/Bhartendu-Kumar/rules_template)

**Obsolete/Legacy Rule Files**

Following are outdated files and should no longer be used. 

```bash
# Obsolete/legacy Cursor rule file
.cursorrules

# Obsolete/legacy Cline and RooCode rule file
.clinerules
```