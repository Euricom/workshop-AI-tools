export function add(a: number, b?: number): number {
  const c = a + (b ?? 0);
  return c;
}

export function subtract(a: number, b?: number): number {
  return a - (b ?? 0);
}

export function multiply(a: number, b: number): number {
  return a * b;
}

export function divide(a: number, b: number): number {
  if (b === 0) {
    throw new Error('Division by zero is not allowed');
  }
  return a / b;
}
