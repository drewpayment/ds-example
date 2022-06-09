import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompanyRoutingModule } from './company-routing.module';
import { MaterialModule } from 'lib/ds/core/src/public_api';
import { CompanyGoalsHeaderComponent } from './company-goals-header/company-goals-header.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { GoalsModule } from '@ds/performance/goals/goals.module';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    CompanyRoutingModule,
    GoalsModule,
    DsCardModule
  ],
  declarations: [
    CompanyGoalsHeaderComponent
  ],
  entryComponents: []
})
export class CompanyAppModule { }
