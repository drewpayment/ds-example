

export function hideConsole(production: boolean): void {
  if (window && production) {
    window.console.log = function() {}
    window.console.dir = function() {}
    window.console.error = function() {}
  }
}
