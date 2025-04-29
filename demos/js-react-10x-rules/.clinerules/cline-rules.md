# Guidelines for creating and maintaining Cline rules to ensure consistency and effectiveness.

- **Required Rule Structure:**
  ```markdown
  # Clear, one-line description of what the rule enforces

  - **Main Points in Bold**
    - Sub-points with details
    - Examples and explanations
  ```

- **File Name/Location**

Always place rule files in PROJECT_ROOT/.clinerules/:

```
.clinerules/
├── 01_fundamentals.md
├── 02_another.md
└── ...
```

- **Code Examples:**
  - Use language-specific code blocks
  ```typescript
  // ✅ DO: Show good examples
  const goodExample = true;
  
  // ❌ DON'T: Show anti-patterns
  const badExample = false;
  ```

- **Rule Content Guidelines:**
  - Start with high-level overview
  - Include specific, actionable requirements
  - Show examples of correct implementation
  - Reference existing code when possible
  - Keep rules DRY by referencing other rules

- **Rule Maintenance:**
  - Update rules when new patterns emerge
  - Add examples from actual codebase
  - Remove outdated patterns
  - Cross-reference related rules

- **Best Practices:**
  - Use bullet points for clarity
  - Keep descriptions concise
  - Include both DO and DON'T examples
  - Reference actual code over theoretical examples
  - Use consistent formatting across rules 
