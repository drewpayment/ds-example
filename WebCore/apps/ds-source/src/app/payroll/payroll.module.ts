import { NgModule }                  from '@angular/core';
import { CommonModule }              from '@angular/common';
import { PaycheckOutletComponent }   from './paycheck-outlet/paycheck-outlet.component';
import { Routes, RouterModule }      from '@angular/router';
import { PaycheckListComponent }     from '@ds/payroll/paycheck-list/paycheck-list.component';
import { CheckStockDialogComponent } from '@ds/payroll/check-stock/check-stock-dialog/check-stock-dialog.component';
import { CheckStockTriggerComponent } from '@ds/payroll/check-stock/check-stock-trigger/check-stock-trigger.component';
import { TimeAndAttendanceComponent } from '@ds/payroll/time-and-attendance/time-and-attendance.component';
import { TimeClockHardwareComponent } from '@ds/payroll/time-and-attendance/time-clock-hardware/time-clock-hardware.component';
import { PayrollModule } from '@ds/payroll/payroll.module';
import { VoidChecksComponent } from './void-checks/void-checks.component';
import { VoidChecksGuard } from './void-checks/void-checks.guard';
import { DsCardModule } from '@ds/core/ui/ds-card/ds-card.module';
import { EmployeeTimecardComponent } from '@ds/core/employees/employee-timecard/employee-timecard.component';

export const payrollRoutes: Routes = [
    // { path:'payroll/paycheck/list/:payrollId', component: PaycheckOutletComponent}
    // { path:'payroll/paycheck/list/:id', component: PaycheckOutletComponent}
    { path: 'payroll', children: [
        { path: '', component: PaycheckOutletComponent, children:
            [
                { path: '', redirectTo: ':payrollId/paycheck/list', pathMatch: 'full' },
                { path: ':payrollId/paycheck/list', component: PaycheckListComponent }
            ]
        },
        {
            path: 'void-checks', component: VoidChecksComponent, canActivate: [VoidChecksGuard],
        }
    ]},
    // {
    //     path: 'time-and-attendance', children: [
    //         {path: '', component: TimeAndAttendanceComponent },
    //         {path: 'supervisor/:supervisorId/is-approved/:isApproved', component: TimeAndAttendanceComponent}
    //     ]
    // },
    {
        path: 'time-clock-hardware', children: [
            {path: '', component: TimeClockHardwareComponent },
            {path: 'supervisor/:supervisorId/is-approved/:isApproved', component: TimeClockHardwareComponent}
        ]
    } ,
    {
        path: 'clock-punch-history', component: EmployeeTimecardComponent
    }
];

@NgModule({
    imports: [
        CommonModule,
        PayrollModule,
        DsCardModule,

        RouterModule.forChild(payrollRoutes),
    ],
    exports: [
        RouterModule,
    ],
    providers: [
        VoidChecksGuard,
    ],
    declarations: [PaycheckOutletComponent, VoidChecksComponent],
    entryComponents: [CheckStockDialogComponent, CheckStockTriggerComponent],
})
export class PayrollAppModule { }
