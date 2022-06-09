import { Injectable } from '@angular/core';
// import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
// import { ClockEmployeeSetup } from '@ajs/labor/clock-employee/models';
// import { ClockClientRulesSummary } from 'apps/ds-mobile/src/app/schedule/clock-client-rules-summary.model';
// import { Moment } from 'moment';
// import * as moment from 'moment';   // https://momentjs.com/docs/#/use-it/typescript/

@Injectable({
  providedIn: 'root'
})
export class EmployeeServicesService {

  constructor(
  ) {
    // Nothing.
  }

//   getServiceTester(): string {
//     return 'It worked!';
//   }


//   getEmployeeLaborSetup(employeeId: number) {
//     const apiUri = `${EmployeeServicesService.EMPLOYEE_LABOR_API_BASE}/employees/${employeeId}`;

//     let response = this.http.get<ClockEmployeeSetup>(apiUri);

//     // Misguided...
//     // response.subscribe(x => {
//     //   x.selectedSchedules.forEach(s => {
//     //       console.log(typeof(s.scheduleDetails.startTime));
//     //       s.scheduleDetails.startTime = moment(s.scheduleDetails.startTime);
//     //       console.log(typeof(s.scheduleDetails.startTime));
//     //       s.scheduleDetails.stopTime  = moment(s.scheduleDetails.stopTime);
//     //       s.scheduleDetails.startDate = moment(s.scheduleDetails.startDate);
//     //       s.scheduleDetails.endDate   = moment(s.scheduleDetails.endDate);
//     //       s.scheduleDetails.modified  = moment(s.scheduleDetails.modified);
//     //   });
//     // });

//     // Closer, but still misguided...
//     // response.pipe(map(x => {
//     //     return {
//     //         employeeId: x.employeeId,
//     //         clientId: x.clientId,
//     //         badgeNumber: x.badgeNumber,
//     //         pin: x.pin,
//     //         allowGroupScheduling: x.allowGroupScheduling,
//     //         allowEditEmployeeSetup: x.allowEditEmployeeSetup,
//     //         selectedTimePolicy: x.selectedTimePolicy,
//     //         // selectedSchedules: x.selectedSchedules,
//     //         selectedSchedules: {
//     //             clientId: x.selectedSchedules.clientId,
//     //             scheduleId: x.selectedSchedules.scheduleId,
//     //             name: x.selectedSchedules.name,
//     //             isActive: x.selectedSchedules.isActive,
//     //             $isNotAvailable: x.selectedSchedules.$isNotAvailable,
//     //             _lowerName: x.selectedSchedules._lowerName,
//     //             scheduleDetails: [
//     //               // Damn, this is an array.
//     //             ]
//     //         },
//     //         selectedCostCenters: x.selectedCostCenters
//     //     };
//     // }));

//     return response;
//   }


//   getWeeklyHoursWorked(clientId:   number,
//                        employeeId: number,
//                        startDate:  Moment|Date|string = moment().startOf('week').format('YYYY-MM-DD'),
//                        endDate:    Moment|Date|string = moment().endOf('week').format('YYYY-MM-DD')
//   ) {
//     // TODO: We should handle the different types for {start,end}Date.
//     // Currently only works as expected when passed in as string, or default value used.
//     const apiUri = `${EmployeeServicesService.CLOCK_API_BASE}/weekly-hours-worked/clients/` +
//                    `${clientId}/employees/${employeeId}?start=${startDate}&end=${endDate}`;

//     // TODO: add a return interface/type declaration
//     let response = this.http.get<any>(apiUri);

//     // Misguided...
//     // response.subscribe(x => {
//     //     x.days.forEach(d => {
//     //         d.date = moment(d.date);
//     //         d.startTime = moment(d.startTime);
//     //         d.endTime = moment(d.endTime);
//     //     });
//     // });

//     return response;
//   }


//   getNextPunchDetail(employeeId: number) {
//     const apiUri = `${EmployeeServicesService.CLOCK_API_BASE}/punchDetail/${employeeId}?config=true`;

//     // TODO: add a return interface/type declaration
//     let response = this.http.get<any>(apiUri);

//     return response;
//   }

  // getEmployeePunches(
  //   employeeId: number,
  //   startDate:  string = moment().startOf('week').format('YYYY-MM-DD'),
  //   endDate:    string = moment().endOf('week').format('YYYY-MM-DD')
  //   // startDate:  Moment|Date|string = moment().startOf('week').format('YYYY-MM-DD'),
  //   // endDate:    Moment|Date|string = moment().endOf('week').format('YYYY-MM-DD')
  // ) {
  //   const apiUri = `${EmployeeServicesService.CLOCK_API_BASE}`;

  //   // TODO: We should handle the different types for {start,end}Date.
  //   // Currently only works as expected when passed in as string, or default value used.

  //   const params = new HttpParams();
  //   params.append('id', employeeId.toString());
  //   params.append('startDate', startDate);
  //   params.append('endDate', endDate);

  //   // TODO: add a return interface/type declaration
  //   let response = this.http.get<any>(apiUri, {params});

  //   return response;
  // }


// //   // Probably don't need this anymore...
// //   getClockClientRulesSummary(employeeId: number) {
// //     const apiUri = `${EmployeeServicesService.MOBILE_API_BASE}/clock/clientRulesSummary/${employeeId}`;

// //     let response = this.http.get<ClockClientRulesSummary>(apiUri);

// //     return response;
// //   }

  // // FIXME: Find a way to differentiate Date from Moment.
  // coerceToMoment(date: Moment|Date|string): Moment {
  //   let result: Moment;
  //   console.log(date);
  //   console.log('typeof(date)=', typeof(date));
  //   // @ts-ignore
  //   console.log('typeof(date.now)=', typeof(date.now));
  //   // @ts-ignore
  //   console.log('typeof(date.now) === \'function\'=', typeof(date.now) === 'function');
  //   // console.log('date.hasOwnProperty(\'now\')=', date.hasOwnProperty('now'));
  //   if (typeof(date) === 'string') {
  //       // TODO: we should validate that string is of an acceptable/parseable format.
  //       result = moment(date);
  //   // FIXME: This is where it falls apart. Find a way to differentiate Date from Moment.
  //   // @ts-ignore
  //   } else if (typeof(date.now) === 'function') {
  //   // } else if (date.hasOwnProperty('now')) {
  //       result = moment(date);
  //   } else {
  //       // Must be a Moment after all.
  //       result = <Moment>date;
  //   }
  //   return result;
  // }
}
