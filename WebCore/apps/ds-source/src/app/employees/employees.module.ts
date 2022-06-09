import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TerminateEmployeeModalComponent, TerminateEmployeeModule } from '@ajs/employee/terminate-employee';
import { EmployeeNotesModule } from '../../../../../lib/ds/core/src/lib/employees/employee-notes/employee-notes.module';
import { EmployeeNotesComponent } from '../../../../../lib/ds/core/src/lib/employees/employee-notes/employee-notes/employee-notes.component';
import { EmployeeTimecardModule } from '@ds/core/employees/employee-timecard/employee-timecard.module';
import { EmployeeTimecardComponent } from '@ds/core/employees/employee-timecard/employee-timecard.component';

@NgModule({
  imports: [
    CommonModule,
    EmployeeNotesModule,
    TerminateEmployeeModule,
    EmployeeTimecardModule,
  ],
  declarations: [],
  entryComponents:[
    EmployeeNotesComponent,
    TerminateEmployeeModalComponent,
    EmployeeTimecardComponent,
  ]
})
export class EmployeesAppModule { }
