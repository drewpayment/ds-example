import { BrowserModule } from "@angular/platform-browser";
import {
  NgModule,
  APP_INITIALIZER,
  ApplicationRef,
  ComponentFactoryResolver,
  Inject,
  Injector,
  InjectionToken,
} from "@angular/core";
import {
  UpgradeModule,
  downgradeComponent,
  downgradeInjectable,
} from "@angular/upgrade/static";
import { HttpClientModule } from "@angular/common/http";
import { AppRoutingModule } from "./app-routing.module";
import { PerformanceAppModule } from "./performance/performance.module";
import { UrlHandlingStrategy, UrlSerializer } from "@angular/router";
import { SidebarComponent } from "./sidebar/sidebar.component";
import { AjsUpgradesModule } from "@ds/core/ajs-upgrades/ajs-upgrades.module";
import { EssCommonModule } from "./common/common.module";
import {
  NgxDowngradesModule,
  NgxLinkComponent,
} from "@ds/core/ngx-downgrades/ngx-downgrades.module";
import { CoreModule } from "@ds/core/core.module";
import { AccountService } from "@ds/core/account.service";
import * as angular from "angular";
import { DsEssAppAjsModule } from "apps/ds-ess/ajs/ds-ess-app.module";
import { PaySettingsAppModule } from "./pay-settings/pay-settings-app.module";
import { AccountSettingsOutletComponent } from "./account-settings/account-settings-outlet/account-settings-outlet.component";
import { AccountSettingsAppModule } from "./account-settings/account-settings-app.module";
import { ResourcesAppModule } from "./resources/resources-app.module";
import { ConsentsAppModule } from './consents/consents-app.module';
import { NotesAppModule } from "./notes/notes-app.module";
import { ResourcesListOutletComponent } from "./resources/resources-list-outlet/resources-list-outlet.component";
import { ProfileAppModule } from "./profile/profile-app.module";
import { ProfileOutletComponent } from "./profile/profile-outlet/profile-outlet.component";
import { LOCAL_STORAGE } from "ngx-webstorage-service";
import { DS_STORAGE_SERVICE } from "@ds/core/storage/storage.service";
import { DsCustomOutletModule } from "@ds/core/ui/ds-custom-outlet/ds-custom-outlet.module";
import { BootstrapService } from "./bootstrap.service";
import { DOCUMENT, APP_BASE_HREF } from "@angular/common";
import { LoadingMessageModule } from "@ds/core/ui/loading-message/loading-message.module";
import { EssAppComponent } from "./ess-app/ess-app.component";
import { BetaFeaturesModule } from "@ds/core/users/beta-features/beta-features.module";
import { BenefitsEssModule } from "./benefits/benefits-ess.module";
import { MaterialModule } from "@ds/core/ui/material/material.module";
import { OutletComponent } from "./outlet/outlet.component";
import { TimeOffModule } from "./time-off/time-off.module";
import { DsUiNavModule } from "@ds/core/ui/nav/nav.module";
import { MenuWrapperHeaderModule } from "@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module";
import { AjsEssUpgradesModule } from "./ajs-upgrades.module";
import { StoreModule } from "@ngrx/store";
import { StoreDevtoolsModule } from "@ngrx/store-devtools";
import { environment } from "../environments/environment";
import { DsAppConfigModule } from "@ds/core/app-config";
import { LowerCaseUrlSerializer } from "@ds/core/utilities";
import { AppConfig, APP_CONFIG } from "@ds/core/app-config/app-config";
import { OnboardingEssModule } from "./onboarding/onboarding-ess.module";
import { EffectsModule } from "@ngrx/effects";
import { NgxTimeoutService } from '@ds/core/directives/ngx-timeout.service';

angular
  .module(DsEssAppAjsModule.AjsModule.name)
  .directive(
    "profileOutlet",
    downgradeComponent({
      component: ProfileOutletComponent,
    }) as ng.IDirectiveFactory
  )
  .factory("ngxAccountService", downgradeInjectable(AccountService) as any);

export function getBaseHref(config: AppConfig) {
  return config.baseSite.url.replace(location.origin, "");
}

/**
 * DS ESS site is bootstrapped differently from DS Source.
 * In order to bootstrap and handle a mixture of AngularJs (AJS & ui-router) and Angular 6 (NGX & @angular/router) routing, we
 * bootstrap a single NGX component called "OutletComponent". When OutletComponent
 * initializes, it checks to see if AJS is bootstrapped. At this point, we bootstrap our
 * AJS application to the entire document like normal.
 *
 * By doing this, @angular/router is controlling the routing process inside of the header of the site
 * instead of relying on ui-router to navigate. However, ui-router still has event listeners and watches
 * the state of the URL in the address bar and will render content in it's respective ui-view directives
 * that are injected into the dom by the OutletComponent template. In all, if ui-router sees a route that
 * has an AJS state file defined, it will render that AJS component. Otherwise, @angular/router
 * will handle the routing and show the appropriate NGX components.
 *
 * The header is AJS but has NGX router link components (NgxLinkComponent) downgraded to AJS and used in it, so that it can
 * properly communicate with @angular/router enabling links in our AJS defined header to navigate the user via @angular/router.
 */
@NgModule({
  imports: [
    BrowserModule,
    UpgradeModule,
    CoreModule,
    HttpClientModule,
    MaterialModule,
    AjsUpgradesModule.forRoot(),
    NgxDowngradesModule,
    DsCustomOutletModule,
    LoadingMessageModule,
    BetaFeaturesModule.forRoot(environment),
    BenefitsEssModule,
    OnboardingEssModule,
    TimeOffModule,
    MenuWrapperHeaderModule,
    AjsEssUpgradesModule,

    // NGX MIGRATED MODULES
    EssCommonModule,
    PerformanceAppModule,
    PaySettingsAppModule,
    AccountSettingsAppModule,
    ResourcesAppModule,
        ConsentsAppModule,
        NotesAppModule,
    ProfileAppModule,
    DsUiNavModule,

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
    DsAppConfigModule,

    // routing
    AppRoutingModule,
  ],
  providers: [
    {
      provide: "$scope",
      useFactory: (i: any) => i.get("$rootScope"),
      deps: ["$injector"],
    },
    {
      provide: DS_STORAGE_SERVICE,
      useExisting: LOCAL_STORAGE,
    },
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
      provide: "window",
      useValue: window,
    },
    {
      provide: APP_BASE_HREF,
      useFactory: getBaseHref,
      deps: [AppConfig],
    },
  ],
  declarations: [SidebarComponent, EssAppComponent, OutletComponent],
  bootstrap: [EssAppComponent],
})
export class DsEssAppModule {
  // bootstrapComponents: any[] = ESS_ENTRY_COMPONENTS;
  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private upgrade: UpgradeModule,
    private boot: BootstrapService,
    @Inject(DOCUMENT) public document: Document,
    private timeoutService: NgxTimeoutService,
  ) {
    if (!this.boot.isBootstrapped) {
      const ajs = document.documentElement;
      this.upgrade.bootstrap(ajs, [DsEssAppAjsModule.AjsModule.name], {
        strictDi: true,
      });
      this.boot.isBootstrapped = true;
    }
    this.timeoutService.listen();
  }

}
