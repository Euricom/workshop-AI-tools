import { describe, expect, it } from "vitest";
import { formatCurrency, formatDate } from "./format";

describe("format utilities", () => {
  describe("formatCurrency", () => {
    it("formats numbers with € symbol by default", () => {
      expect(formatCurrency(1000)).toBe("€1,000.00");
      expect(formatCurrency(1234.56)).toBe("€1,234.56");
      expect(formatCurrency(0.99)).toBe("€0.99");
    });

    it("formats numbers with the provided currency symbol", () => {
      expect(formatCurrency(1000, "$")).toBe("$1,000.00");
      expect(formatCurrency(1234.56, "£")).toBe("£1,234.56");
    });
  });

  describe("formatDate", () => {
    it("formats dates in short format by default", () => {
      const date = new Date("2023-05-15");
      expect(formatDate(date)).toBe("5/15/2023");
    });

    it("formats dates in long format when specified", () => {
      const date = new Date("2023-05-15");
      expect(formatDate(date, "long")).toBe("May 15, 2023");
    });
  });
});
