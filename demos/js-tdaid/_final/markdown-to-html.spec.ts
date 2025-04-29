import { test, expect, describe } from 'vitest';
import { markdownToHtml } from './markdown-to-html';

describe('markdownToHtml', () => {
  test('converts paragraphs', () => {
    const markdown = 'This is a paragraph.';
    const expected = '<p>This is a paragraph.</p>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts multiple paragraphs', () => {
    const markdown = 'First paragraph.\n\nSecond paragraph.';
    const expected = '<p>First paragraph.</p>\n<p>Second paragraph.</p>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts headings', () => {
    const markdown = '# Heading 1\n## Heading 2\n### Heading 3';
    const expected = '<h1>Heading 1</h1>\n<h2>Heading 2</h2>\n<h3>Heading 3</h3>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts bold text', () => {
    const markdown = 'This is **bold** text.';
    const expected = '<p>This is <strong>bold</strong> text.</p>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts italic text', () => {
    const markdown = 'This is *italic* text.';
    const expected = '<p>This is <em>italic</em> text.</p>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts unordered lists', () => {
    const markdown = '- Item 1\n- Item 2\n- Item 3';
    const expected = '<ul>\n<li>Item 1</li>\n<li>Item 2</li>\n<li>Item 3</li>\n</ul>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts ordered lists', () => {
    const markdown = '1. Item 1\n2. Item 2\n3. Item 3';
    const expected = '<ol>\n<li>Item 1</li>\n<li>Item 2</li>\n<li>Item 3</li>\n</ol>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts links', () => {
    const markdown = 'This is a [link](https://example.com).';
    const expected = '<p>This is a <a href="https://example.com">link</a>.</p>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts code blocks', () => {
    const markdown = '```\nconst x = 1;\n```';
    const expected = '<pre><code>const x = 1;\n</code></pre>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts inline code', () => {
    const markdown = 'This is `inline code`.';
    const expected = '<p>This is <code>inline code</code>.</p>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('converts blockquotes', () => {
    const markdown = '> This is a blockquote.';
    const expected = '<blockquote>This is a blockquote.</blockquote>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('handles empty input', () => {
    const markdown = '';
    const expected = '';
    expect(markdownToHtml(markdown)).toBe(expected);
  });

  test('handles complex markdown', () => {
    const markdown =
      '# Title\n\nParagraph with **bold** and *italic* text.\n\n- List item 1\n- List item 2\n\n> Blockquote\n\n```\ncode block\n```';

    const expected =
      '<h1>Title</h1>\n<p>Paragraph with <strong>bold</strong> and <em>italic</em> text.</p>\n<ul>\n<li>List item 1</li>\n<li>List item 2</li>\n</ul>\n<blockquote>Blockquote</blockquote>\n<pre><code>code block\n</code></pre>';
    expect(markdownToHtml(markdown)).toBe(expected);
  });
});
