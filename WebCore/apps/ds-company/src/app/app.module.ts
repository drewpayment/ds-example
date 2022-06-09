import { BrowserModule } from "@angular/platform-browser";
import {
  NgModule,
  ApplicationRef,
  Inject,
  ComponentFactoryResolver,
} from "@angular/core";
import { UpgradeModule } from "@angular/upgrade/static";
import { CompanyJobProfilesModule } from "./job-profiles/job-profiles.module";
import { HttpClientModule } from "@angular/common/http";
import { CoreModule } from "@ds/core/core.module";
import { AjsUpgradesModule } from "@ds/core/ajs-upgrades/ajs-upgrades.module";
import { LoadingMessageModule } from "@ds/core/ui/loading-message/loading-message.module";
import { BetaFeaturesModule } from "@ds/core/users/beta-features/beta-features.module";
import { CompanyAppComponent } from "./company-app/company-app.component";
import { DsUiNavModule } from "@ds/core/ui/nav/nav.module";
import { RouterModule, Route, UrlSerializer, UrlHandlingStrategy } from "@angular/router";
import { OutletComponent } from "./outlet/outlet.component";
import { LowerCaseUrlSerializer } from "@ds/core/utilities";
import { Ng1Ng2UrlHandlingStrategy, routes } from "./routes";
import { AjsCompanyComponent } from "./ajs.component";
import { MenuWrapperHeaderModule } from "@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module";
import { DOCUMENT, APP_BASE_HREF } from "@angular/common";
import { DsAppConfigModule } from "@ds/core/app-config";
import { StoreModule } from "@ngrx/store";
import { StoreDevtoolsModule } from "@ngrx/store-devtools";
import { environment } from "../environments/environment";
import { EffectsModule } from "@ngrx/effects";
import { AppConfig, APP_CONFIG } from "@ds/core/app-config/app-config";
import { ClientManagementModule } from "./client-management/client-management.module";
import { DsUsersModule } from "@ds/core/users";
import { MaterialModule } from "@ds/core/ui/material";
import { DS_STORAGE_SERVICE } from "@ds/core/storage/storage.service";
import { LOCAL_STORAGE } from "ngx-webstorage-service";
import { CompanyManagementModule } from "./company-management/company-management.module";
import { EmployeeSelfServiceModule } from "./employee-self-service/employee-self-service.module";
import { ApplicantSelfServiceModule } from "./applicant-self-service/applicant-self-service.module";
import { EmployeeManagementModule } from "./employee-management/employee-management.module";
import { AttachmentManagementModule } from "./employee-management/attachments/attachment-management.module";
import { SharedModule } from "./shared/shared.module";
import { NgxTimeoutService } from '@ds/core/directives/ngx-timeout.service';
import { JobProfileModule } from "./job-profile/job-profile.module";
import { SnapengageComponent } from "@ds/core/snapengage/snapengage.component";
import { PermissionErrorModule } from "@ds/core/ui/permission-error/permission-error.module";
import { FourOhFourModule } from "@ds/core/ui/four-oh-four/four-oh-four.module";
import { CompanyMessageComponent } from './services/company-message/company-message.component';


export function getBaseHref(config: AppConfig) {
  return config.baseSite.url.replace(location.origin, "");
}

@NgModule({
  imports: [
    BrowserModule,
    UpgradeModule,
    CompanyJobProfilesModule,
    JobProfileModule,
    CoreModule,
    HttpClientModule,
    LoadingMessageModule,
    BetaFeaturesModule.forRoot(environment),
    DsUiNavModule,
    MenuWrapperHeaderModule,
    DsAppConfigModule,
    MaterialModule,
    ClientManagementModule,
    CompanyManagementModule,
    EmployeeManagementModule,
    AttachmentManagementModule,
    EmployeeSelfServiceModule,
    ApplicantSelfServiceModule,
    SharedModule,
    AjsUpgradesModule.forRoot(),
    DsUsersModule,
    PermissionErrorModule,
    FourOhFourModule,
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

    // ROUTING MODULE
    AjsUpgradesModule.forRoot(),
    DsUsersModule,

    /**
     * ALL ROUTING IS HANDLED THROUGH THIS ROUTER MODULE AS AN ENTRY POINT
     * URL is checked for Angular JS routes and if none match then it checks
     * Angular routes.
     *
     * https://angular.io/guide/upgrade#create-a-service-to-lazy-load-angularjs
     */
    RouterModule.forRoot(routes),
  ],
  declarations: [
    CompanyAppComponent,
    OutletComponent,
    AjsCompanyComponent,
    SnapengageComponent,
    CompanyMessageComponent,
  ],
  // entryComponents: [CompanyAppComponent],
  bootstrap: [CompanyAppComponent],
  providers: [
    { provide: DS_STORAGE_SERVICE, useExisting: LOCAL_STORAGE },
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer,
    },
    {
      provide: APP_CONFIG,
      useFactory: (config: AppConfig) => config,
      deps: [AppConfig],
    },
    {
      provide: APP_BASE_HREF,
      useFactory: getBaseHref,
      deps: [AppConfig],
    },
    {
      provide: 'window',
      useValue: window,
    },
    {
      provide: UrlHandlingStrategy,
      useClass: Ng1Ng2UrlHandlingStrategy,
    },
  ],
  entryComponents: [],
})
export class DsCompanyAppNgxModule {
  constructor(
    private upgrade: UpgradeModule,
    private appRef: ApplicationRef,
    @Inject(DOCUMENT) private document: Document,
    private resolver: ComponentFactoryResolver,
    private timeoutService: NgxTimeoutService,
  ) {
    this.timeoutService.listen();
  }

  ngDoBootstrap() {
    // const element = angular.element(document.documentElement);
    // const isBootstrapped = element.injector();
    // if (!isBootstrapped) {
    //   this.upgrade.bootstrap(document.documentElement, [
    //     DsCompanyAppAjsModule.AjsModule.name,
    //   ]);
    // }
  }
}
