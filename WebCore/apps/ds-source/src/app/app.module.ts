import { LaborAppModule } from "./labor/labor.module";
import { BrowserModule } from "@angular/platform-browser";
import {
  NgModule,
  ComponentFactoryResolver,
  Inject,
  ApplicationRef,
  Component,
} from "@angular/core";
import { UpgradeModule } from "@angular/upgrade/static";
import { UrlHandlingStrategy } from "@angular/router";
import { AppRoutingModule } from "./app-routing.module";
import { DsSourceAppAjsModule } from "../../ajs/ds-source-app.module";
import { Ng2UrlHandlingStrategy } from "./Ng2UrlHandlingStrategy";
import { OutletComponent } from "./outlet.component";
import { Type } from "@angular/core";
import { DOCUMENT } from "@angular/common";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { CoreModule } from "@ds/core/core.module";
import { AjsUpgradesModule } from "@ds/core/ajs-upgrades/ajs-upgrades.module";
import { PerformanceAppModule } from "./performance/performance.module";
import { PayrollAppModule } from "./payroll/payroll.module";
import { OnboardingAppModule } from "./onboarding/onboarding.module";
import { DsReportsAppModule } from "./reports/reports.module";
import { VendorMaintenanceComponent } from "@ds/payroll/vendor-maintenance/vendor-maintenance.component";
import { ClientModule } from "./client/client.module";
import {
  NotificationsModule,
  ClientNotificationPreferencesFormComponent,
  ContactNotificationPreferencesFormComponent,
} from "@ds/notifications";
import { EmployeesAppModule } from "./employees/employees.module";
import { ApplicantsAppModule } from "./applicants/applicants.module";
import { AccessRuleManagerComponent, DsUsersModule } from "@ds/core/users";
import {
  DsAppConfigModule,
  AppResourceManagerComponent,
  AppResourceDialogComponent,
  MenuBuilderComponent,
} from "@ds/core/app-config";
import { DsAutocompleteModule } from "@ds/core/ui/ds-autocomplete";
import { DsContactAutocompleteComponent } from "@ds/core/ui/ds-autocomplete/ds-contact-autocomplete/ds-contact-autocomplete.component";
import { BanksAppModule } from "./banks/banks.module";
import { EmployeeAppModule } from "./employee/employee.module";
import { DS_STORAGE_SERVICE } from "@ds/core/storage/storage.service";
import { LOCAL_STORAGE } from "ngx-webstorage-service";
import { NgxDowngradesModule } from "@ds/core/ngx-downgrades/ngx-downgrades.module";
import { TaxDeferralsComponent } from "./client/tax-deferrals/tax-deferrals.component";
import { ArOutletComponent } from "./admin/ar/ar-outlet/ar-outlet.component";
import { ArModule } from "./admin/ar/ar.module";
import { DsAppComponent } from "./ds-app/ds-app.component";
import { DsUiNavModule } from "@ds/core/ui/nav/nav.module";
import { DsNavMenuComponent } from "@ds/core/ui/nav/ds-nav-menu/ds-nav-menu.component";
import { DsNavMainContentComponent } from "@ds/core/ui/nav/ds-nav-main-content/ds-nav-main-content.component";
import { DsNavMainLegacyContentComponent } from "@ds/core/ui/nav/ds-nav-main-legacy-content/ds-nav-main-legacy-content.component";
import { DsNavToolbarContentComponent } from "@ds/core/ui/nav/ds-nav-toolbar-content/ds-nav-toolbar-content.component";
import { DsNavMenuContentComponent } from "@ds/core/ui/nav/ds-nav-menu-content/ds-nav-menu-content.component";
import { BetaFeaturesModule } from "@ds/core/users/beta-features/beta-features.module";
import { DsBenefitsAppModule } from "./benefits/benefits.module";
import { EmployeeEEOCExportComponent } from "./employee/employee-eeoc/employee-eeoc-export.component";
import { handleDsChangeTracking } from "@ds/core/ui/change-track/track-changes";
import { StoreModule } from "@ngrx/store";
import { StoreDevtoolsModule } from "@ngrx/store-devtools";
import { environment } from "../environments/environment";
import { EffectsModule } from "@ngrx/effects";
import { AppConfig, APP_CONFIG } from "@ds/core/app-config/app-config";
import { windowProvider } from "@ds/core/core.tokens";
import { SnapengageComponent } from "@ds/core/snapengage/snapengage.component";
import { EmployeePayComponent } from "./employee/employee-pay/employee-pay.component";
import { AnalyticsDashboardComponent } from "./reports/analytics-dashboard/analytics-dashboard.component";
import { NgxTimeoutService } from "@ds/core/directives/ngx-timeout.service";
import { AdminAppModule } from "./admin/admin.module";
import { AdminModule } from "@ds/admin";
import { AngularMaterialDateboxModule } from "@ds/core/ui/angularMaterialDatebox/angularMaterialDatebox.module";
import { InputPunchesWidgetComponent } from "./employee/input-punches-widget/input-punches-widget.component";
import { createCustomElement } from "@angular/elements";
import { Injector } from "@angular/core";
import { AccountService } from "@ds/core/account.service";
import { ClientService } from "@ds/core/clients/shared";
import { DogfoodInterceptor } from './dogfood.interceptor';
import { EmployeeExportModalComponent } from '../../../ds-company/src/app/company-management/employee-export/employee-export-modal/employee-export-modal.component';
import { MaterialModule } from "@ds/core/ui/material";

declare const angular: any;

// the components that will make available entry points for NGX modules in our hybrid applications
const NgxEntryComponents: any[] = [
  OutletComponent,
  InputPunchesWidgetComponent,
  TaxDeferralsComponent,
  DsAppComponent,
  SnapengageComponent,
  EmployeePayComponent,
  AnalyticsDashboardComponent,
  ArOutletComponent,
];

@NgModule({
  imports: [
    BrowserModule,
    UpgradeModule,
    CoreModule,
    HttpClientModule,
    DsReportsAppModule,
    DsAutocompleteModule,
    AjsUpgradesModule.forRoot(),
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

    // NGX MIGRATED MODULES
    MaterialModule,
    PerformanceAppModule,
    DsUsersModule,
    PayrollAppModule,
    ClientModule,
    NotificationsModule,
    EmployeesAppModule,
    BanksAppModule,
    EmployeesAppModule,
    AdminModule,
    NotificationsModule,
    EmployeesAppModule,
    EmployeeAppModule,
    NgxDowngradesModule,
    LaborAppModule,
    DsUiNavModule,
    BetaFeaturesModule.forRoot(environment),
    DsBenefitsAppModule,
    ApplicantsAppModule,
    AngularMaterialDateboxModule,
    ArModule,
    AdminAppModule,
    DsAppConfigModule,

    // DON'T PROVIDE ROUTES IN THIS MODULE.
    // THIS ROUTER WILL COLLECT ALL ROUTES FROM ALL REGISTERED MODULES.
    AppRoutingModule,
    DsBenefitsAppModule,
    OnboardingAppModule,
  ],

  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DogfoodInterceptor,
      multi: true,
    },
    { provide: UrlHandlingStrategy, useClass: Ng2UrlHandlingStrategy },
    {
      provide: "$scope",
      useFactory: (i) => i.get("$rootScope"),
      deps: ["$injector"],
    },
    { provide: "window", useValue: window },
    // <-- added to fix this SO https://stackoverflow.com/questions/45950137/use-angularjs-directive-in-angular-component
    // {
    //     provide: APP_BASE_HREF,
    //     useValue: window.location.href.substring(window.location.origin.length, window.location.href.length)
    // },
    { provide: DS_STORAGE_SERVICE, useExisting: LOCAL_STORAGE },
    {
      provide: APP_CONFIG,
      useFactory: (config: AppConfig) => config,
      deps: [AppConfig],
    },
    windowProvider,
  ],

  entryComponents: [
    ...NgxEntryComponents,
    DsNavMenuComponent,
    DsNavMainContentComponent,
    DsNavMainLegacyContentComponent,
    DsNavToolbarContentComponent,
    DsNavMenuContentComponent,
    AccessRuleManagerComponent,
    AppResourceManagerComponent,
    ClientNotificationPreferencesFormComponent,
    ContactNotificationPreferencesFormComponent,
    VendorMaintenanceComponent,
    AppResourceDialogComponent,
    DsContactAutocompleteComponent,
    MenuBuilderComponent,
    DsAppComponent,
    EmployeeEEOCExportComponent,
  ],
  declarations: [OutletComponent, DsAppComponent, SnapengageComponent, EmployeeExportModalComponent],
})
export class DsSourceAppModule {
  static bootstrapComponents: Type<any>[] = NgxEntryComponents;

  constructor(
    private upgrade: UpgradeModule,
    private _componentFactoryResolver: ComponentFactoryResolver,
    @Inject(DOCUMENT) private _document: any,
    private timeoutService: NgxTimeoutService,
    private injector: Injector,
    private account: AccountService,
    private clientService: ClientService
  ) {
    handleDsChangeTracking();
    this.timeoutService.listen();

    this.registerBootstrapJsValidator();
  }

  ngDoBootstrap(applicationRef: ApplicationRef) {
    // this won't work yet, has issues with ajs injector not being set before the web components are bootstrapped
    // for (const component of DsSourceAppModule.bootstrapComponents) {
    //   const el = createCustomElement(component, { injector: this.injector });
    //   const selector = (component as any).ɵcmp.selectors[0][0] + '-wc';
    //   console.log(selector);
    //   customElements.define(selector, el);
    // }
    const el = createCustomElement(DsNavMenuComponent, { injector: this.injector });
    customElements.define('ds-nav-menu-wc', el);

    const element = angular.element(this._document.documentElement);
    const ajsIsInitialized = element.injector();

    if (!ajsIsInitialized)
      this.upgrade.bootstrap(
        this._document.documentElement,
        [DsSourceAppAjsModule.AjsModule.name],
        { strictDi: true }
      );

    for (const component of DsSourceAppModule.bootstrapComponents) {
      const { selector } =
        this._componentFactoryResolver.resolveComponentFactory(component);

      if (this._document.querySelector(selector)) {
        applicationRef.bootstrap(component);
      }
    }
  }

  // Register BootstrapJS form validator. On Mainlayout.Master, there is a data-toggle="validator" attribute
  // that triggers validation via BootstrapJS. However, the introduction of Angular Material's material sidenav
  // the validation is not triggered. This hack will force the BootstrapJS validation to be triggered on page load
  // for legacy pages that need it.
  private registerBootstrapJsValidator() {
    if ((window as any).dominion && window.$ && window.$.fn.validator) {
      (window as any).dominion.registerAsyncPostbackEndEvent(function () {
        window.$('form[data-toggle="validator"]').validator("update");
      });
    }

    if (window.$ && window.$.fn.validator) {
      window.$('form[data-toggle="validator"]').validator("update");
    }
  }
}
