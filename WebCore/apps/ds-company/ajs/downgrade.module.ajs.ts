import {
  downgradeComponent,
  downgradeInjectable,
  downgradeModule,
} from "@angular/upgrade/static";
import { MenuWrapperToggleComponent } from "@ds/core/users/beta-features/menu-wrapper-toggle/menu-wrapper-toggle.component";
import { Provider, StaticProvider } from "@angular/core";
import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
import { BetaFeaturesModule } from "@ds/core/users/beta-features/beta-features.module";
import { DsStorageService } from "@ds/core/storage/storage.service";
import { AvatarComponent } from "@ds/core/ui/avatar/avatar.component";
import { LoadingMessageComponent } from "@ds/core/ui/loading-message/loading-message.component";
import { CertifyI9TriggerComponent } from "@ds/onboarding/certify-I9/certify-I9-trigger/certify-I9-trigger.component";

declare const angular: ng.IAngularStatic;

export module DsCompanyDowngradeNgxModule {
  export const MODULE_NAME = "DsCompanyDowngradeNgxModule";
  export const MODULE_DEPS = [
    downgradeModule((extraProviders: StaticProvider[]) => {
      const pr = platformBrowserDynamic(extraProviders);
      return pr.bootstrapModule(BetaFeaturesModule);
    }),
  ];

  export const AjsModule = angular.module(MODULE_NAME, []);

  AjsModule.directive(
    "ajsLoadingMessage",
    downgradeComponent({
      component: LoadingMessageComponent,
    }) as ng.IDirectiveFactory
  )

    .directive(
      "dsMenuWrapperToggle",
      downgradeComponent({
        component: MenuWrapperToggleComponent,
      }) as ng.IDirectiveFactory
    )
    .directive(
      "dsAvatar",
      downgradeComponent({ component: AvatarComponent }) as ng.IDirectiveFactory
    )
    .factory("DsStorageService", downgradeInjectable(DsStorageService) as any)
    .directive(
            'dsCertifyI9Trigger',
            downgradeComponent({ component: CertifyI9TriggerComponent }) as ng.IDirectiveFactory
        );
}
