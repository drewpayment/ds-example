import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeePerformanceNavComponent } from './employee-performance-nav/employee-performance-nav.component';
import { EmployeeReviewsOutletComponent } from './employee-reviews-outlet/employee-reviews-outlet.component';
import { EmployeeGoalsListComponent } from '@ds/performance/goals/employee-goals-list/employee-goals-list.component';
import { CompetencyModelAssignEmployeeComponent } from '@ds/performance/competencies/competency-model-assign-employee/competency-model-assign-employee.component';
import { EmployeeAttachmentViewerComponent } from '@ds/performance/attachments/employee-attachment-viewer/employee-attachment-viewer.component';
import { EmployeesPerformanceGuard } from './employees-performance.guard';

export const EmployeePerformanceRoutes: Routes = [
    {
        path: 'performance/employees',
        component: EmployeePerformanceNavComponent,
        canActivate: [EmployeesPerformanceGuard],
        children: [
            {
                path: "reviews",
                component: EmployeeReviewsOutletComponent
            },
            {
                path: "goals",
                component: EmployeeGoalsListComponent,
                data: { isEmployeeGoals: true, isAssignToRestricted: true }
            },
            {
                path: "competencies",
                component: CompetencyModelAssignEmployeeComponent
            },
            {
                path: "attachments",
                component: EmployeeAttachmentViewerComponent
            }
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(EmployeePerformanceRoutes)
    ],
    exports: [
        RouterModule
    ],
    providers: [
        EmployeesPerformanceGuard,
    ],
})
export class EmployeePerformanceRoutingModule { }
