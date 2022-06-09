import { EmergencyContactsComponent } from './emergency-contacts/emergency-contacts.component';
import { BrowserModule } from '@angular/platform-browser';
import {
  NgModule,
  ComponentFactoryResolver,
  Inject,
  ApplicationRef,
} from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { ScheduleDetailsComponent } from './schedule-details/schedule-details.component';
import { UpgradeModule } from '@angular/upgrade/static';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { UpdateEmergencyContactComponent } from './update-emergency-contact/update-emergency-contact.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TimeOffComponent } from './time-off/time-off.component';
import { PunchCardComponent } from './punch-card/punch-card.component';
import { PaychecksSummaryComponent } from './paychecks/paychecks-summary/paychecks-summary.component';
import { PaychecksDetailsComponent } from './paychecks/paychecks-details/paychecks-details.component';
import { GetCellTitlePipe } from './paychecks/paychecks-details/get-cell-title.pipe';
import { UnderConstructionComponent } from './under-construction/under-construction.component';
import { DependentComponent } from './dependents/dependent/dependent.component';
import { DependentDetailComponent } from './dependents/dependent-detail/dependent-detail.component';
import { ScheduleDetailContentComponent } from './schedule-details/schedule-detail-content/schedule-detail-content.component';
import { ScheduleDetailCardComponent } from './schedule-details/schedule-detail-card/schedule-detail-card.component';
import { TaxesComponent } from './taxes/taxes.component';
import { TaxEditComponent } from './tax-edit/tax-edit.component';
import { ApiInterceptor } from './api-interceptor';
import { ContactInformationComponent } from './contact-information/contact-information.component';
import { PhonePipe } from '@ds/core/ui/pipes/phone.pipe';
import { PunchService } from '@ds/core/timeclock/punch.service';
import { UrlHandlingStrategy } from '@angular/router';
import { Ng2UrlHandlingStrategy } from './Ng2UrlHandlingStrategy';
import { MobileBreadcrumbComponent } from './mobile-breadcrumb/mobile-breadcrumb.component';
import { DsDialogModule } from '@ds/core/ui/ds-dialog';
import { NpsModule } from '@ds/admin/nps/nps.module';

import { NgxMaskModule, IConfig } from 'ngx-mask';
import { FederalW4FormComponent } from './federal-w4-form/federal-w4-form.component';
import { NgxTimeoutService } from '@ds/core/directives/ngx-timeout.service';
import { BetaFeaturesModule } from '@ds/core/users/beta-features/beta-features.module';
import { AppConfig, APP_CONFIG } from '@ds/core/app-config/app-config';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';
import { environment } from '../environments/environment';
export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
  declarations: [
    // If a pipe is used in a component.ts, due to a bug in ng8 with --aot flag,
    // it must be declared first in this file
    PhonePipe,
    // end of ng 8 bug workaround

    AppComponent,
    HomeComponent,
    ScheduleComponent,
    ScheduleDetailsComponent,
    UserSettingsComponent,
    ProfileComponent,
    EmergencyContactsComponent,
    TimeOffComponent,
    UpdateEmergencyContactComponent,
    PaychecksSummaryComponent,
    PaychecksDetailsComponent,
    GetCellTitlePipe,
    PunchCardComponent,
    UnderConstructionComponent,
    DependentComponent,
    DependentDetailComponent,
    ScheduleDetailContentComponent,
    ScheduleDetailCardComponent,
    TaxesComponent,
    TaxEditComponent,
    ContactInformationComponent,
    MobileBreadcrumbComponent,
    FederalW4FormComponent,
  ],
  imports: [
    BrowserModule,
    UpgradeModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    ReactiveFormsModule,
    // AjsUpgradesModule.forRoot(),
    FormsModule,
    DsDialogModule,
    NpsModule,
    StoreModule.forRoot(
      {},
      {
        runtimeChecks: {
          strictActionImmutability: false,
          strictStateImmutability: false,
        },
      }
    ),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: environment.production,
    }),
    EffectsModule.forRoot([]),

    // APPLICATION ROUTING
    AppRoutingModule,
    NgxMaskModule.forRoot(options),
  ],
  providers: [
    { provide: UrlHandlingStrategy, useClass: Ng2UrlHandlingStrategy },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ApiInterceptor,
      multi: true,
    },
    {
      provide: APP_CONFIG,
      useFactory: (config: AppConfig) => config,
      deps: [AppConfig],
    },
    PhonePipe,
    PunchService,
    { provide: 'window', useValue: window },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(private timeoutService: NgxTimeoutService) {
    this.timeoutService.listen();
  }
}
