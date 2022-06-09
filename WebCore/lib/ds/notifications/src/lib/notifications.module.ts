import { NgModule } from '@angular/core';
import { ClientNotificationPreferencesFormComponent } from '@ds/notifications/client-notification-preferences-form/client-notification-preferences-form.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ContactNotificationPreferencesFormComponent } from '@ds/notifications/contact-notification-preferences-form/contact-notification-preferences-form.component';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DsCoreFormsModule,
    DsCardModule
  ],
  entryComponents:[
    ClientNotificationPreferencesFormComponent,
    ContactNotificationPreferencesFormComponent
  ],
  declarations: [
    ClientNotificationPreferencesFormComponent,
    ContactNotificationPreferencesFormComponent
  ],
  exports: [
    ClientNotificationPreferencesFormComponent,
    ContactNotificationPreferencesFormComponent
  ]
})
export class NotificationsModule { }
