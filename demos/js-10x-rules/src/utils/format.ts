/**
 * Utility functions for formatting values
 */

/**
 * Formats a number as currency with the given currency symbol
 * @param value The number to format
 * @param currency The currency symbol to use (default: €)
 * @returns Formatted currency string
 */
export function formatCurrency(value: number, currency = "€"): string {
  return `${currency}${value.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",")}`;
}

/**
 * Formats a date in the specified format
 * @param date The date to format
 * @param format The format to use (default: 'short')
 * @returns Formatted date string
 */
export function formatDate(
  date: Date,
  format: "short" | "long" = "short",
): string {
  const options: Intl.DateTimeFormatOptions =
    format === "long"
      ? { year: "numeric", month: "long", day: "numeric" }
      : { year: "numeric", month: "numeric", day: "numeric" };

  return new Intl.DateTimeFormat("en-US", options).format(date);
}
