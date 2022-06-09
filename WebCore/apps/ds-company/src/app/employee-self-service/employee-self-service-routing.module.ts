import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserTypeGuard } from '../guards/user-type.guard';
import { UserType } from '@ds/core/shared';
import { DefaultComponent } from './landing-page/default.component';
import { LandingPageGuard } from '../guards/landing-page.guard';
import { DirtyCheckGuard } from '../guards/dirty-check.guard';
import { EventsPageComponent } from '@ds/employees/events/events-page.component';
import { UserProfileModule } from './user-profile/user-profile.module';


export const employeeSelfServiceRoutes: Routes = [
  {
    path: '',
    redirectTo: 'service/home',
    pathMatch: 'full'
  },
  {
    path: 'service',
    canActivate: [UserTypeGuard],
    data: {
      userTypes: [
        UserType.systemAdmin,
        UserType.companyAdmin,
        UserType.supervisor,
        UserType.employee,
      ],
    },
    children: [
      {
        path: 'user-profile',
        loadChildren: () => UserProfileModule,
      },
      {
        path: '',
        redirectTo: 'service/home',
        pathMatch: 'full',
      },
      { path: 'events', component: EventsPageComponent, canDeactivate: [DirtyCheckGuard]},
    ],
  },
  // this route is not a child because it is the landing page and needs a special guard
  // All children that don't need a special guard need to be included in service/children
  {
    path: 'service/home',
    canActivate: [LandingPageGuard],
    data: {
      userTypes: [
        UserType.systemAdmin,
        UserType.companyAdmin,
        UserType.supervisor,
        UserType.employee,
        UserType.applicant,
      ],
    },
    component: DefaultComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(employeeSelfServiceRoutes)],
  exports: [RouterModule],
})
export class EmployeeSelfServiceRoutingModule {}
