import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ApplicantSelfServiceRoutingModule } from './applicant-self-service-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { UpgradeModule } from '@angular/upgrade/static';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { MatDialogModule } from '@angular/material/dialog';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ApplicantSelfServiceRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatDialogModule,
    DsCardModule,
    DsCoreFormsModule,
    UpgradeModule,
    LoadingMessageModule,
    DsCoreResourcesModule
  ],

})
export class ApplicantSelfServiceModule {}
