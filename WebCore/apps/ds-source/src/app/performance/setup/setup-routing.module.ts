import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ReviewProfilesOutletComponent } from './review-profiles-outlet/review-profiles-outlet.component';
import { ReviewProfileFormOutletComponent } from './review-profile-form-outlet/review-profile-form-outlet.component';
import { ReviewPolicySetupFormComponent } from '@ds/performance/review-policy/review-policy-setup-form/review-policy-setup-form.component';

const performanceSetupRoutes: Routes = [
  {
    path: 'performance/setup',
    children: [
        { 
            path: 'review-profiles', 
            component: ReviewProfilesOutletComponent, 
        },
        {                
            path: 'review-profiles/edit',
            component: ReviewProfileFormOutletComponent            
        },
        {
          path: 'review-policy',
          component: ReviewPolicySetupFormComponent
        },
        {
          path: 'review-policy/edit/:reviewPolicyId',
          component: ReviewPolicySetupFormComponent
        },
        // {
        //   path: 'review-policy',
        //   component: ReviewCyclesOutletComponent,
        //   children: [
        //     {
        //       path: '',
        //       redirectTo: 'list',
        //       pathMatch: 'full'
        //     },
        //     {
        //       path: 'list',
        //       component: ReviewCyclesListComponent
        //     },
        //     {
        //       path: 'edit/:id',
        //       component: ReviewCyclesSetupFormComponent
        //     }
        //   ]
        // }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(performanceSetupRoutes)],
  exports: [RouterModule]
})
export class PerformanceSetupRoutingModule { }
