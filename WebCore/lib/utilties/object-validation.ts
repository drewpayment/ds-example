

export function IsObject(value: any): boolean {
  return typeof value === 'object'
    && !Array.isArray(value)
    && value !== null;
}
