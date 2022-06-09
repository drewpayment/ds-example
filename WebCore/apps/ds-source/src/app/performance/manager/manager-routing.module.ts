import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ManagerHeaderComponent } from './manager-header/manager-header.component';
import { EmployeeSchedulerOutletComponent } from './employee-scheduler-outlet/employee-scheduler-outlet.component';
import { ReviewStatusOutletComponent } from './review-status-outlet/review-status-outlet.component';
import { ArchiveOutletComponent } from './archive-outlet/archive-outlet.component';
import { ReviewAnalyticsOutletComponent } from './review-analytics-outlet/review-analytics-outlet.component';
import { PayrollRequestGuard } from './guard/payroll-request.guard';
import { PayrollRequestsComponent } from '@ds/performance/performance-manager/payroll-requests/payroll-requests.component';

const performanceManagerRoutes: Routes = [
  {
    path: 'performance/manage',
    component: ManagerHeaderComponent,
    children: [
        {
            path: '',
            redirectTo: 'schedule',
            pathMatch: 'full'
        },
        { 
            path: 'schedule', 
            component: EmployeeSchedulerOutletComponent, 
        },
        {                
            path: 'status',
            component: ReviewStatusOutletComponent            
        },
        {                
            path: 'analytics',
            component: ReviewAnalyticsOutletComponent            
        },
        {
            path: 'archive',
            component: ArchiveOutletComponent
        },
        {
          path: 'approval',
          component: PayrollRequestsComponent,
          canActivate: [PayrollRequestGuard]
        }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(performanceManagerRoutes)],
  exports: [RouterModule]
})
export class PerformanceManagerRoutingModule { }