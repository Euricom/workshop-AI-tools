/**
 * Converts a markdown string to HTML
 * @param markdown The markdown string to convert
 * @returns The HTML string
 */
export function markdownToHtml(markdown: string): string {
  if (!markdown) {
    return '';
  }

  // Process headings (# Heading) first
  let html = processHeadings(markdown);

  // Process code blocks (```)
  html = processCodeBlocks(html);

  // Process paragraphs and line breaks
  html = processParagraphs(html);

  // Process lists
  html = processLists(html);

  // Process blockquotes
  html = processBlockquotes(html);

  // Process inline formatting (bold, italic, code, links)
  html = processInlineFormatting(html);

  return html;
}

/**
 * Process code blocks (```)
 */
function processCodeBlocks(text: string): string {
  const codeBlockRegex = /```\n([\s\S]*?)\n```/g;
  return text.replace(codeBlockRegex, (match, code) => {
    // Preserve the trailing newline in the code block
    return `<pre><code>${code}\n</code></pre>`;
  });
}

/**
 * Process paragraphs and line breaks
 */
function processParagraphs(text: string): string {
  // Split by double newlines to get paragraphs
  const paragraphs = text.split(/\n\n+/);

  return paragraphs
    .map((p) => {
      // Skip wrapping if it's already a special block
      if (
        p.startsWith('<pre>') ||
        p.startsWith('<h') ||
        p.startsWith('<ul>') ||
        p.startsWith('<ol>') ||
        p.startsWith('<blockquote>')
      ) {
        return p;
      }

      // Wrap in paragraph tags if not empty
      return p.trim() ? `<p>${p}</p>` : '';
    })
    .join('\n');
}

/**
 * Process headings (# Heading)
 */
function processHeadings(text: string): string {
  // Process h1 to h6
  for (let i = 6; i >= 1; i--) {
    const regex = new RegExp(`^${'#'.repeat(i)}\\s+(.*?)$`, 'gm');
    text = text.replace(regex, `<h${i}>$1</h${i}>`);
  }

  return text;
}

/**
 * Process unordered and ordered lists
 */
function processLists(text: string): string {
  // Process unordered lists
  let result = text.replace(
    /<p>(\s*-.*(?:\n\s*-.*)*)<\/p>/g,
    (match: string, listContent: string) => {
      const items = listContent
        .split('\n')
        .map((item: string) => {
          const trimmedItem = item.replace(/^\s*-\s*/, '');
          return `<li>${trimmedItem}</li>`;
        })
        .join('\n');

      return `<ul>\n${items}\n</ul>`;
    }
  );

  // Process ordered lists
  result = result.replace(
    /<p>(\s*\d+\..*(?:\n\s*\d+\..*)*)<\/p>/g,
    (match: string, listContent: string) => {
      const items = listContent
        .split('\n')
        .map((item: string) => {
          const trimmedItem = item.replace(/^\s*\d+\.\s*/, '');
          return `<li>${trimmedItem}</li>`;
        })
        .join('\n');

      return `<ol>\n${items}\n</ol>`;
    }
  );

  return result;
}

/**
 * Process blockquotes
 */
function processBlockquotes(text: string): string {
  return text.replace(/<p>\s*>\s*(.*?)<\/p>/g, '<blockquote>$1</blockquote>');
}

/**
 * Process inline formatting (bold, italic, code, links)
 */
function processInlineFormatting(text: string): string {
  // Process bold text (**text**)
  let result = text.replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>');

  // Process italic text (*text*)
  result = result.replace(/\*(.*?)\*/g, '<em>$1</em>');

  // Process inline code (`code`)
  result = result.replace(/`(.*?)`/g, '<code>$1</code>');

  // Process links ([text](url))
  result = result.replace(/\[(.*?)\]\((.*?)\)/g, '<a href="$2">$1</a>');

  return result;
}
