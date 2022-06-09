export enum DateUnit {
    Day = 1,
    Week,
    Month
  }

export function ConvertToDays(unit: DateUnit, value: number) {
  switch (unit) {
    case DateUnit.Day:
      return value;
    case DateUnit.Week:
      return 7 * value;
    case DateUnit.Month:
      return 7 * 4 * value;
  }
}