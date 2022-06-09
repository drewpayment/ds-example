import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UserTypeGuard } from "../guards/user-type.guard";
import { UserType } from "@ds/core/shared";
//import { DirtyCheckGuard } from "../guards/dirty-check.guard";
import { JobProfileDetailsComponent } from "./job-profile-details/job-profile-details.component";
import { JobProfileListComponent } from "./job-profile-list/job-profile-list.component";

export const jobProfileRoutes: Routes = [
  {
    path: "admin",
    canActivate: [UserTypeGuard],
    data: {
      userTypes: [
        UserType.systemAdmin,
        UserType.companyAdmin,
        UserType.supervisor,
      ],
    },
    children: [
      { path: "job-profiles", component: JobProfileListComponent  },
      { path: "job-profile-details/:cId/:jpId/edit", component: JobProfileDetailsComponent  },

      { path: '', redirectTo: '', pathMatch: 'full', },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(jobProfileRoutes)],
  exports: [RouterModule],
})
export class JobProfileRoutingModule {} 