import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnboardingCardComponent } from './onboarding-card/onboarding-card.component';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { AvatarTableComponent } from './avatar-table/avatar-table.component';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';
import { AvatarWidgetComponent } from './avatar-widget/avatar-widget.component';
import { AvatarCalloutComponent } from './avatar-callout/avatar-callout.component';



@NgModule({
  declarations: [
    AvatarTableComponent,
    OnboardingCardComponent,
    AvatarWidgetComponent,
    AvatarCalloutComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    DsCardModule,
    AvatarModule,
    
  ],
  exports: [
    AvatarTableComponent,
    OnboardingCardComponent,
    AvatarWidgetComponent,
    AvatarCalloutComponent
  ]
})
export class AvatarSharedModule { }
