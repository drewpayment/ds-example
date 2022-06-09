import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CoreModule } from '@ds/core/core.module';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsConfirmDialogModule } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { MaterialModule } from '@ds/core/ui/material';
import { DsEmployeeProfileModule } from '@ds/employees/profile/ds-employee-profile.module';
import { CompanyAccessDialogComponent } from './company-access-dialog/company-access-dialog.component';
import { UserProfileFormComponent, UserProfileLoginHelpDialog } from './user-profile-form/user-profile-form.component';
import { UserProfileComponent } from './user-profile.component';
import { UserProfileService } from './user-profile.service';
import { NotDirective } from './user-profile-form/directives/not.directive';

const routes: Routes = [
  {
    path: '',
    component: UserProfileComponent,
  },
];

@NgModule({
  declarations: [
    UserProfileComponent,
    UserProfileFormComponent,
    UserProfileLoginHelpDialog,
    CompanyAccessDialogComponent,
    NotDirective,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    DsCardModule,
    DsCoreFormsModule,
    DsAutocompleteModule,
    CoreModule,
    LoadingMessageModule,
    DsCoreResourcesModule,
    DsConfirmDialogModule,
    DsEmployeeProfileModule,

    RouterModule.forChild(routes),
  ],
  exports: [
    UserProfileComponent,
    UserProfileFormComponent,
  ],
  providers: [
    UserProfileService,
  ],
})
export class UserProfileModule {}
