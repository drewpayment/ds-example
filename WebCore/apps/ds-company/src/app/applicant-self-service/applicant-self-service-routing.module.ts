import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserTypeGuard } from '../guards/user-type.guard';
import { UserType } from '@ds/core/shared';


export const applicantSelfServiceRoutes: Routes = [
  { 
    path: 'applicant',
    canActivate: [UserTypeGuard],
    data: {
      userTypes: [
        UserType.systemAdmin, 
        UserType.companyAdmin, 
        UserType.supervisor, 
        UserType.employee,
        UserType.applicant
      ]
    }, 
    children: [
      { path: '', redirectTo: '', pathMatch: 'full' },
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(applicantSelfServiceRoutes)],
  exports: [RouterModule]
})
export class ApplicantSelfServiceRoutingModule { }
