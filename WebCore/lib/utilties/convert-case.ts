// ========================
// Case Conversion Utilities
// Note: All case conversion functions require `toKebab` to work.
// ------------------------
declare global {
  interface String {
    toKebabCase(): string;
    toCamelCase(): string;
    toTitleCase(): string;
    toSentenceCase(): string;
    toBoolean(): boolean;
  }
}

String.prototype.toKebabCase = function(): string {
  return String(this)
    .split("")
    .map((letter, index) => {
      if (/[A-Z]/.test(letter)) {
        return ` ${letter.toLowerCase()}`;
      }
      return letter;
    })
    .join("")
    .trim()
    .replace(/[_\s]+/g, "-");
}

String.prototype.toCamelCase = function(): string {
  return String(this).toKebabCase()
    .split("-")
    .map((word, index) => {
      if (index === 0) return word;
      return word.slice(0, 1).toUpperCase() + word.slice(1).toLowerCase();
    })
    .join("");
}

String.prototype.toTitleCase = function(): string {
  return String(this).toKebabCase()
    .split("-")
    .map((word) => {
      return word.slice(0, 1).toUpperCase() + word.slice(1);
    })
    .join(" ");
}

String.prototype.toSentenceCase = function(): string {
  const interim = String(this).toKebabCase().replace(/-/g, " ");
  return interim.slice(0, 1).toUpperCase() + interim.slice(1);
}

String.prototype.toBoolean = function(): boolean {
  const value = (this || '').trim().toLowerCase();
  if (value === 'false' || value === '0') return false;
  return !!value;
}

export {}
