import { Component, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { IEmployeeBirthdate } from '@ds/analytics/shared/models/employee-birthdates.model';
import { EventsComponentService } from '@ds/analytics/shared/services/events-component.service';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import * as moment from 'moment';
import { IEmployeeAnniversary } from '@ds/analytics/shared/models/employee-anniversary.model';
import { MatDialog } from '@angular/material/dialog';
import { BirthdayEventDialogComponent } from './events-dialog-components/birthday-event-dialog/birthday-event-dialog.component';
import { AnniversaryEventDialogComponent } from './events-dialog-components/anniversary-event-dialog/anniversary-event-dialog.component';
import { NinetyDayEventDialogComponent } from './events-dialog-components/ninety-day-event-dialog/ninety-day-event-dialog.component';
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';

@Component({
  selector: 'ds-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})

export class EventsComponent implements OnInit {
  @Input() employeeIds: Number[];
  @Input() dateRange: DateRange;

  cardType: string = 'graph'; //"graph" or "info"
  title: string = 'Events'; //Title of the card
  settingsItems: string[] = ['Print All'];

  employeeBirthdates: IEmployeeBirthdate[] = [];
  birthdayCount: number;

  employeeAnniversaries: IEmployeeAnniversary[] = [];
  anniversaryCount: number;

  ninetyDayEnd: Date = new Date();
  employeeNinetyDayAnniversaries: IEmployeeAnniversary[] = [];
  ninetyDayAnniversaryCount: number;

  loadedBirthday: boolean;
  loadedAnniversary: boolean;
  loaded90Day: boolean;
  loaded: boolean

  constructor(private cdr: ChangeDetectorRef, private eventservice: EventsComponentService, private accountService: AccountService, private dialog: MatDialog) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((user) => {
      this.eventservice.getBirthdays(user.clientId, moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE), moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((data: IEmployeeBirthdate[]) => {
        this.employeeBirthdates = data;
        this.employeeBirthdates = this.employeeBirthdates.filter(x => x.dateOfBirth != null);
        let bday = this.employeeBirthdates.map((x) => x.dateOfBirth.toString().substring(5,10));
        this.checkBirthdayDate(bday)
        this.setAge();
        this.birthdayCount = this.employeeBirthdates.length;
        this.loadedBirthday = true;
        if (this.loadedBirthday && this.loadedAnniversary && this.loaded90Day) this.loaded = true;
      });

      this.eventservice.getAnniversaries(user.clientId, moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE), moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((data: IEmployeeAnniversary[]) => {
        this.employeeAnniversaries = data;
        this.employeeAnniversaries = this.employeeAnniversaries.filter(x => x.hireDate != null);
        let anniversary = this.employeeAnniversaries.map(x => x.hireDate.toString().substring(5,10));
        this.checkAnniversaryDate(anniversary)
        this.setYearsOfService();
        this.anniversaryCount = this.employeeAnniversaries.length;
        this.loadedAnniversary = true;
        if (this.loadedBirthday && this.loadedAnniversary && this.loaded90Day) this.loaded = true;
      });


      this.eventservice.getAnniversaries(user.clientId, moment(this.ninetyDayEnd.setDate(this.ninetyDayEnd.getDate() - 90)).format(MOMENT_FORMATS.DATE), moment(this.ninetyDayEnd).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((data: IEmployeeAnniversary[]) => {
        data.forEach(element => {
          if (element.hireDate != null) {
            element.hireDate = new Date(element.hireDate);
            var anniversaryDate = moment(element.hireDate).add(90, 'days');
            element.ninetyDayAnniversaryDate = new Date(anniversaryDate.toDate());

            if (element.ninetyDayAnniversaryDate >= new Date(this.dateRange.StartDate) && element.ninetyDayAnniversaryDate <= new Date(this.dateRange.EndDate))
              this.employeeNinetyDayAnniversaries.push(element);
          }
        });
      this.ninetyDayAnniversaryCount = this.employeeNinetyDayAnniversaries.length;
      this.loaded90Day = true;
      if (this.loadedBirthday && this.loadedAnniversary && this.loaded90Day) this.loaded = true;
      });
    })
  }

  checkBirthdayDate(data){
    var obj = [];
    for (var i = 0; i < this.employeeBirthdates.length;i++){
      if (data[i] >= moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE).substring(5,10) && data[i] <= moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE).substring(5,10)){
        obj.push(this.employeeBirthdates[i])
      }
      if (moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE).substring(5,10) >= moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE).substring(5,10)){
          obj.push(this.employeeBirthdates[i])
      }
    }
    this.employeeBirthdates = obj;
  }

  setAge(){
    this.employeeBirthdates.forEach(element => {
      let current = new Date();
      let temp = new Date(element.dateOfBirth);
      temp.setFullYear(current.getFullYear());
      if (current >= temp)
        element.age = current.getFullYear() - new Date(element.dateOfBirth).getFullYear();
      else
        element.age = current.getFullYear() - new Date(element.dateOfBirth).getFullYear()-1;
    })
  }

  checkAnniversaryDate(data){
    var obj = [];
    for (var i = 0; i < this.employeeAnniversaries.length;i++){
      if (data[i] >= moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE).substring(5,10) && data[i] <= moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE).substring(5,10)){
        obj.push(this.employeeAnniversaries[i])
      }
      if (moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE).substring(5,10) >= moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE).substring(5,10)){
          obj.push(this.employeeAnniversaries[i])
      }
    }
    this.employeeAnniversaries = obj;
  }

  setYearsOfService(){
    this.employeeAnniversaries.forEach(element => {
      let current = new Date();
      let temp = new Date(element.hireDate);
      temp.setFullYear(current.getFullYear());
      if (current >= temp)
        element.yearsOfService = current.getFullYear() - new Date(element.hireDate).getFullYear();
      else
        element.yearsOfService = current.getFullYear() - new Date(element.hireDate).getFullYear()-1;
    })
  }

  openBirthdateDialog(){
    var config = {
      width: '1000px',
      data: {
        employeeBirthdates: this.employeeBirthdates
      }
    };

    const dialogRef = this.dialog.open(BirthdayEventDialogComponent, config);
  }

  openAnniversaryDialog(){
    var config = {
      width: '1000px',
      data: {
        employeeAnniversaries: this.employeeAnniversaries
      }
    };

    const dialogRef = this.dialog.open(AnniversaryEventDialogComponent, config);
  }

  openNinetyDayDialog(){
    var config = {
      width: '1000px',
      data: {
        employeeNinetyDayAnniversaries: this.employeeNinetyDayAnniversaries
      }
    };

    const dialogRef = this.dialog.open(NinetyDayEventDialogComponent, config);
  }

  printAll = () => {
    //Anniversaries
    var employees = this.employeeAnniversaries;

    employees.sort((a, b) => { return a.firstName > b.firstName ? -1 : 1; })

    var printWindow = window.open("EMPLOYEE ANNIVERSARIES", 'Print' + (new Date()).getTime(), 'width=1200,height=700');
    printWindow.document.write('<!DOCTYPE html><html><head>');

    for (var i = 0; i < document.styleSheets.length; i++) {
        if (document.styleSheets[i].href) {
            printWindow.document.write('<link href= "' + document.styleSheets[i].href + '" rel="stylesheet" type="text/css" >');
        }
    }

    printWindow.document.write('</head><body style="background-color:#fff;">');
    printWindow.document.write('<style> html{font-size: 12px} @page{size:landscape;}</style>');
    printWindow.document.write('<div class="print-page">');
    printWindow.document.write('<div class="print-page-header">');
    printWindow.document.write('<h1 class="print-header">'+ "EMPLOYEE ANNIVERSARIES" + '</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    if (employees.length == 1)
      printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employee</div>');
    else{
      printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employees</div>');
    }
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Name</th>');
    printWindow.document.write('<th>Department</th>');
    printWindow.document.write('<th>Cost Center</th>');
    printWindow.document.write('<th>Hire Date</th>');
    printWindow.document.write('<th>   </th>');
    printWindow.document.write('<th>Years of Service</th>');
    printWindow.document.write('</tr>');

    employees.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write('<td>' + employee.lastName + ', ' + employee.firstName + '</td>');
      if (employee.clientDepartmentName == null) employee.clientDepartmentName = "";
      printWindow.document.write('<td>' + employee.clientDepartmentName + '</td>');
      if (employee.clientCostCenterName == null) employee.clientCostCenterName = "";
      printWindow.document.write('<td>' + employee.clientCostCenterName + '</td>');
      printWindow.document.write('<td>' + this.convertDate(new Date(employee.hireDate)) + '</td>');
      printWindow.document.write('<td>   </td>');
      printWindow.document.write('<td>' + employee.yearsOfService  + '</td>');
      printWindow.document.write('<tr>');
    })
    printWindow.document.write('<tr>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td>___</td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<tr>');

    printWindow.document.write('</table>');
    printWindow.document.write('</div>');

    printWindow.document.write('<p style="page-break-before: always">');

    //Birthdays
    var employeeBirthdays = this.employeeBirthdates;

    employeeBirthdays.sort((a, b) => { return a.firstName > b.firstName ? -1 : 1; })

    for (var i = 0; i < document.styleSheets.length; i++) {
        if (document.styleSheets[i].href) {
            printWindow.document.write('<link href= "' + document.styleSheets[i].href + '" rel="stylesheet" type="text/css" >');
        }
    }

    printWindow.document.write('<div class="print-page">');
    printWindow.document.write('<div class="print-page-header">');
    printWindow.document.write('<h1 class="print-header">'+ "EMPLOYEE BIRTHDAYS" + '</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    if (employeeBirthdays.length == 1)
      printWindow.document.write('<div class="instruction-text">'+ employeeBirthdays.length + ' Employee</div>');
    else{
      printWindow.document.write('<div class="instruction-text">'+ employeeBirthdays.length + ' Employees</div>');
    }
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Name</th>');
    printWindow.document.write('<th>Department</th>');
    printWindow.document.write('<th>Cost Center</th>');
    printWindow.document.write('<th>Birthdate</th>');
    printWindow.document.write('<th>   </th>');
    printWindow.document.write('<th>Age</th>');
    printWindow.document.write('</tr>');

    employeeBirthdays.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write('<td>' + employee.lastName + ', ' + employee.firstName + '</td>');
      if (employee.clientDepartmentName == null) employee.clientDepartmentName = "";
      printWindow.document.write('<td>' + employee.clientDepartmentName + '</td>');
      if (employee.clientCostCenterName == null) employee.clientCostCenterName = "";
      printWindow.document.write('<td>' + employee.clientCostCenterName + '</td>');
      printWindow.document.write('<td>' + this.convertDate(new Date(employee.dateOfBirth)) + '</td>');
      printWindow.document.write('<td>   </td>');
      printWindow.document.write('<td>' + employee.age  + '</td>');
      printWindow.document.write('<tr>');
    })
    printWindow.document.write('<tr>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td>___</td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<tr>');

    printWindow.document.write('</table>');
    printWindow.document.write('</div>');

    printWindow.document.write('<p style="page-break-before: always">');

    //90 Days
    var newHires = this.employeeNinetyDayAnniversaries;

    newHires.sort((a, b) => { return a.firstName > b.firstName ? -1 : 1; })

    for (var i = 0; i < document.styleSheets.length; i++) {
        if (document.styleSheets[i].href) {
            printWindow.document.write('<link href= "' + document.styleSheets[i].href + '" rel="stylesheet" type="text/css" >');
        }
    }

    printWindow.document.write('</head><body style="background-color:#fff;">');
    printWindow.document.write('<style> html{font-size: 12px} @page{size:landscape;}</style>');
    printWindow.document.write('<div class="print-page">');
    printWindow.document.write('<div class="print-page-header">');
    printWindow.document.write('<h1 class="print-header">'+ "NEW HIRE MILESTONES" + '</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    if (newHires.length == 1)
      printWindow.document.write('<div class="instruction-text">'+ newHires.length + ' Employee</div>');
    else{
      printWindow.document.write('<div class="instruction-text">'+ newHires.length + ' Employees</div>');
    }
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Name</th>');
    printWindow.document.write('<th>Department</th>');
    printWindow.document.write('<th>Cost Center</th>');
    printWindow.document.write('<th>Hire Date</th>');
    printWindow.document.write('<th>   </th>');
    printWindow.document.write('<th>90 Days of Service Date</th>');
    printWindow.document.write('</tr>');

    newHires.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write('<td>' + employee.lastName + ', ' + employee.firstName + '</td>');
      if (employee.clientDepartmentName == null) employee.clientDepartmentName = "";
      printWindow.document.write('<td>' + employee.clientDepartmentName + '</td>');
      if (employee.clientCostCenterName == null) employee.clientCostCenterName = "";
      printWindow.document.write('<td>' + employee.clientCostCenterName + '</td>');
      printWindow.document.write('<td>' + this.convertDate(new Date(employee.hireDate)) + '</td>');
      printWindow.document.write('<td>   </td>');
      printWindow.document.write('<td>' + this.convertDate(new Date(employee.ninetyDayAnniversaryDate))  + '</td>');
      printWindow.document.write('<tr>');
    })
    printWindow.document.write('<tr>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<td>_____</td>');
    printWindow.document.write('<td></td>');
    printWindow.document.write('<tr>');

    printWindow.document.write('</table>');
    printWindow.document.write('</div>')
    printWindow.document.write('<script>window.print();</script>');
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.focus();

    printWindow.addEventListener('afterprint', (event) => {
        printWindow.close();
    });
  }

  convertDate(date: Date){
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    return `${months[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}`;
  }

}
