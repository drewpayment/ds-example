import {
  NgModule,
  Type,
  ComponentFactoryResolver,
  Inject,
  ApplicationRef,
} from '@angular/core';
import { CommonModule, DOCUMENT } from '@angular/common';
import { CertifyI9Component } from '@ds/onboarding/certify-I9/certify-I9.component';
import {
  CertifyI9TriggerComponent,
  CertifyI9DialogComponent,
} from '@ds/onboarding/certify-I9/certify-I9-trigger/certify-I9-trigger.component';
import { I9DocumentTypePipe } from '@ds/onboarding/certify-I9/i9-document-type.pipe';
import { CertifyI9FormComponent } from '@ds/onboarding/certify-I9/certify-I9-form/certify-I9-form.component';
import { RouterModule, Routes } from '@angular/router';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { UserTypeGuard } from '../guards/user-type.guard';
import { UserType } from '@ds/core/shared/user-type';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';

export const onboardingRoutes: Routes = [
    {
        path: 'manage/onboarding', children: [
            { path: 'certify-i9', component: CertifyI9Component, canActivate: [UserTypeGuard], 
              data: {
                userTypes: [UserType.systemAdmin, UserType.companyAdmin, UserType.supervisor, ],
              } },
        ]
    }
];

@NgModule({
  declarations: [
    CertifyI9Component,
    I9DocumentTypePipe,
    CertifyI9FormComponent,
    CertifyI9TriggerComponent,
    CertifyI9DialogComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(onboardingRoutes),
    DsCardModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    DsCoreFormsModule,
    LoadingMessageModule,
    MatSortModule,
    AvatarModule,
  ],
  entryComponents: [
    CertifyI9Component,
    CertifyI9TriggerComponent,
    CertifyI9DialogComponent,
  ],
  exports: [RouterModule],
  providers: [],
})
export class OnboardingAppModule {}
