export function add(a: number, b?: number): number {
  var c = a + b;
  return c;
}

export function subtract(a: number, b?: number): number {
  return a - b;
}

export function multiply(a: number, b: number): number {
  return a * b;
}

export function divide(a: number, b: number): number {
  if (b === 0) {
    throw new Error("Division by zero is not allowed");
  }
  return a / b;
}
