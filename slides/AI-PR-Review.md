# PR Review by AI

These are the major AI tools for PR review:

- GitHub Copilot PR Review
- CodeRabbit

## Copilot PR Review

GitHub Copilot PR Review offers automated, concise, and easily digestible summaries of pull requests. It highlights key changes, improvements, refactoring efforts, and potential issues, ensuring developers can quickly grasp the essence of the PR. Seamlessly integrated into GitHub, Copilot can be added as a reviewer directly from the platform, streamlining the review process.

![](./images/copilot_pr_overview.png)
![](./images/copilot_issue1.png)

See more: https://github.com/Euricom/euri-demo-pr-review/pull/3

## CodeRabbit

Cut Code Review Time & Bugs in Half. Instantly.

### Clear Summaries and Visual Diagrams

![](./images/codeRabbit-summary.png)
![](./images/codeRabbit-walkThrough.png)
![](./images/codeRabbit-walkThrough2.png)

See more: https://github.com/Euricom/euri-demo-pr-review/pull/3


### Context-Aware, Instant Feedback, Context Of The Whole Project

![](./images/codeRabbit_issue1.png)
![](./images/codeRabbit_issue2.png)
![](./images/codeRabbit_issue3.png)
![](./images/codeRabbit_issue4.png)
![](./images/codeRabbit_issue5.png)

See more: https://github.com/Euricom/euri-demo-pr-review/pull/3

### Interactive AI Bot

![](./images/codeRabbit-interaction.png)

### Learning from Feedback (memory)

![](./images/codeRabbit-learning.png)

### Security and Compliance

<img src="./images/codeRabbot_compliances.png" width="300"/>

![](./images/codeRabbot-subprocessors.png)
![](./images/codeRabbot-privacy.png)


## Comparison

| Feature/Aspect        | CodeRabbit                                                                                          | GitHub Copilot PR Review                                                            |
|----------------------|------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------|
| **Review Style**     | Detailed, line-by-line feedback and actionable suggestions. 😀                                         | High-level summaries, highlights only the most relevant issues.                     |
| **Depth of Review**  | Comprehensive, context-aware analysis; catches nuanced bugs and code quality issues. 😀                | Minimalist; flags only obvious problems, avoids nitpicking.                         |
| **Comments Volume**  | Generates more comments for thorough critique. 😀                                                     | Fewer comments, reducing noise but may miss details.                                |
| **Summaries**        | Summarizes changes clearly; can generate visual diagrams. 😀                                           | Excels at succinct, high-quality PR summaries.                                      |
| **Integration**      | Works with GitHub, GitLab, Azure DevOps, issue trackers; customizable via UI or YAML. 😀              | Native to GitHub PRs; invoked from Reviewers menu.                                  |
| **Data Residency**     | GDPR compliant, but all subsystems are in the US.                                                      | Compliance with EU data protection regulations. 😀                                           |
| **Ideal Use Case**   | Teams needing comprehensive, automated reviews with actionable feedback and workflow integration.    | Developers wanting quick, high-level PR summaries and light review assistance.      |

### Peter's Verdict

Use **GitHub Copilot PR Review** if you don't have access to **CodeRabbit**. The PR review is quit basic and doesn't provide much feedback. It is a good start, but it is not as good as **CodeRabbit**. The PR review is not as detailed and doesn't provide as many actionable suggestions. It is a good tool for quick reviews, but it is not a replacement for a human reviewer.

**CodeRabbit** is a better tool for PR reviews. It provides more detailed feedback and actionable suggestions. CodeRabbit is a better choice for teams that need comprehensive, automated reviews with actionable feedback and workflow integration. CodeRabbit is fully GDPR complient but all sub systems are in the US, whats makes it less friendly for EU customers.




