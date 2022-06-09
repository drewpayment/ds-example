import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompanyGoalsHeaderComponent } from './company-goals-header/company-goals-header.component';

const companyPerformanceRoutes: Routes = [
  {
    path: 'performance/goals',
    component: CompanyGoalsHeaderComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(companyPerformanceRoutes)],
  exports: [RouterModule]
})
export class CompanyRoutingModule { }
