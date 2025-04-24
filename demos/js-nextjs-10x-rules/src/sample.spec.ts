import { describe, expect, it } from "vitest";

/**
 * A simple sum function for demonstration
 */
function sum(a: number, b: number): number {
  return a + b;
}

describe("Basic test suite", () => {
  it("sums two numbers correctly", () => {
    expect(sum(2, 3)).toBe(5);
    expect(sum(-1, 1)).toBe(0);
    expect(sum(0, 0)).toBe(0);
  });

  it("demonstrates various assertions", () => {
    // Type checking
    expect(sum(1, 2)).toBeTypeOf("number");

    // Boolean assertions
    expect(true).toBeTruthy();
    expect(false).toBeFalsy();

    // Object comparison
    expect({ name: "test" }).toEqual({ name: "test" });

    // Array testing
    expect([1, 2, 3]).toContain(2);
  });
});
