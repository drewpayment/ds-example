import { Moment } from 'moment';
// import * as moment from 'moment';
import { ClockEmployeePunchDto, ClockEmployeePunchDtoImpl } from '@ds/core/employee-services/models/clock-employee-punch-dto';
// import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';

export interface ScheduleDetailsDay {
  date: Moment|Date|string;
  punches: ClockEmployeePunchDto[];
  hoursWorked?: number;
  hasNoPunches(): boolean;
  // getHoursWorked(): number;
  getInOutPunchPairs(): Array<ClockEmployeePunchDto[]>;
}

export class ScheduleDetailsDayImpl {
  private inOutPunchPairs?: Array<ClockEmployeePunchDto[]>;
  date: Moment|Date|string;
  punches: ClockEmployeePunchDto[] = [];
  hoursWorked?: number;

  constructor(date: Moment|Date|string) {
    this.date = date;
  }

  hasNoPunches(): boolean {
    return this.punches.length > 0;
  }

  // This isn't implented to allow updating once this.inOutPunchPairs is initialized.
  getInOutPunchPairs(): Array<ClockEmployeePunchDto[]> {
    if (!this.inOutPunchPairs) {
      this.inOutPunchPairs = this._getInOutPunchPairs(this.punches);
    }
    return this.inOutPunchPairs;
  }

  private _getInOutPunchPairs(punches: ClockEmployeePunchDto[]): Array<ClockEmployeePunchDto[]> {
    // Order the punches from ealiest to latest punch.
    punches.sort((a, b) => ClockEmployeePunchDtoImpl.comparePunches(a.rawPunch, b.rawPunch));

    const result: Array<ClockEmployeePunchDto[]> = [];
    let pairIndex = 0;

    punches.forEach((p, i) => {
      if (typeof(result[pairIndex]) === 'undefined' || result[pairIndex] === null) {
        result[pairIndex] = [];
      }
      result[pairIndex].push(p);
      pairIndex += i % 2;
    });

    return result;
  }

  // // This isn't implented to allow updating once this.hoursWorked is initialized.
  // getHoursWorked(): number {
  //   if (!this.hoursWorked) {
  //     this.hoursWorked = this._getHoursWorked(this.getInOutPunchPairs());
  //   }
  //   return this.hoursWorked;
  // }

  // private _getHoursWorked(punchPairs: Array<ClockEmployeePunchDto[]>): number {
  //   let milliseconds = 0;

  //   punchPairs.forEach(pp => {
  //     const inPunch  = pp[0];
  //     const outPunch = pp[1];

  //     if (ClockEmployeePunchDtoImpl.punchExists(outPunch)) {
  //       const millisecondsTemp = convertToMoment(outPunch.rawPunch).diff(inPunch.rawPunch);
  //       milliseconds += millisecondsTemp;
  //     }
  //   });

  //   const hours = moment.duration(milliseconds).asHours();

  //   return hours;
  // }
}
