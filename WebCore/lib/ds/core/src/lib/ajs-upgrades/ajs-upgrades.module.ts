import { NgModule, ModuleWithProviders, Directive, ElementRef, Injector, Inject } from '@angular/core';
import { CommonModule, DOCUMENT } from '@angular/common';
import { EmployeeListNavBreadcrumbDirective } from '@ds/core/ajs-upgrades/directives/employee-list-nav.directive';
import {
  DsMsgServiceProvider,
  EmployeeSearchServiceProvider,
  JobProfilesApiServiceProvider,
  DsConfirmServiceProvider,
  DsStyleLoaderServiceProvider,
  ScriptLoaderServiceProvider,
  DsPopupServiceProvider,
  DsOnboardingAdminApiServiceProvider,
  ExternalApiServiceProvider,
  DsEmployeeAttachmentModalProvider,
  DsEmployeeAttachmentApiProvider,
  DsResourceApiProvider,
  DailyRulesModalServiceProvider,
  DsConfigurationServiceUpgradedProvider,
  CountryStateServiceProvider,
  DsUtilityServiceUpgradedProvider,
} from '@ds/core/ajs-upgrades/ajs-providers';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        EmployeeListNavBreadcrumbDirective
    ],
    exports: [
        EmployeeListNavBreadcrumbDirective
    ]
})
export class AjsUpgradesModule {
  static forRoot(): ModuleWithProviders<any> {
    return {
      ngModule: AjsUpgradesModule,
      providers: [
        DsMsgServiceProvider,
        EmployeeSearchServiceProvider,
        JobProfilesApiServiceProvider,
        DsConfirmServiceProvider,
        DsStyleLoaderServiceProvider,
        ScriptLoaderServiceProvider,
        DsPopupServiceProvider,
        DsOnboardingAdminApiServiceProvider,
        ExternalApiServiceProvider,
        DsEmployeeAttachmentModalProvider,
        DsEmployeeAttachmentApiProvider,
        DsResourceApiProvider,
        DailyRulesModalServiceProvider,
        CountryStateServiceProvider,
        DailyRulesModalServiceProvider,
        DsConfigurationServiceUpgradedProvider,
        DsUtilityServiceUpgradedProvider
      ]
    };
  }
}
