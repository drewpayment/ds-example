import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { EmployeeSearchService } from "@ajs/employee/search/shared/employee-search.service";
import { JobProfilesApiService } from '@ajs/job-profiles/shared/job-profiles-api.service';
import { DsConfirmService } from "@ajs/ui/confirm/ds-confirm.service";
import { DsStyleLoaderService } from "@ajs/ui/ds-styles/ds-styles.service";
import { ScriptLoaderService } from '@ajs/google-charts/script-loader.service';
import { DsPopupService } from "@ajs/ui/popup/ds-popup.service";
import { DsOnboardingAdminApiService } from '@ajs/onboarding/shared/ds-admin-api.service';
import { ExternalApiService } from '@ajs/ds-external-api/ds-external-api.svc';
import { DsEmployeeAttachmentModalService } from '@ajs/employee/attachments/addattachment-modal.service';
import { DsEmployeeAttachmentApiService } from '@ajs/employee/attachments-api.service';
import { DsResourceApi } from '@ajs/core/ds-resource/ds-resource-api.service';
import { DailyRulesModalService } from '@ajs/labor/company/rules/modal/daily-rules-modal.service';
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { DsConfigurationService } from "@ajs/core/ds-configuration/ds-configuration.service";
import { AjsClientSelectorService } from '@ajs/selector/client-selector.svc';
import { DsUtilityServiceProvider } from "@ajs/core/ds-utility/ds-utility.service";

export function DsMsgServiceProviderFactory($injector:any) {
    return $injector.get(DsMsgService.SERVICE_NAME);
}
export const DsMsgServiceProvider = {
    provide: DsMsgService,
    useFactory: DsMsgServiceProviderFactory,
    deps: ['$injector']
};

export function EmployeeSearchServiceProviderFactory($injector:any) {
    try {
        return $injector.get(EmployeeSearchService.SERVICE_NAME);    
    } catch (e) {
        // ESS has no EmployeeSearchService added
        return <EmployeeSearchService>{};
    }
}
export const EmployeeSearchServiceProvider = {
    provide: EmployeeSearchService,
    useFactory: EmployeeSearchServiceProviderFactory,
    deps: ['$injector']
};

export function JobProfilesApiServiceProviderFactory($injector:any) {
    return $injector.get(JobProfilesApiService.SERVICE_NAME);
}
export const JobProfilesApiServiceProvider = {
    provide: JobProfilesApiService,
    useFactory: JobProfilesApiServiceProviderFactory,
    deps: ['$injector']
};

export function DsConfirmServiceProviderFactory($injector:any) {
    return $injector.get(DsConfirmService.SERVICE_NAME);
}
export const DsConfirmServiceProvider = {
    provide: DsConfirmService,
    useFactory: DsConfirmServiceProviderFactory,
    deps: ['$injector']
};

export function DsStyleLoaderServiceProviderFactory($injector:any) {
    return $injector.get(DsStyleLoaderService.SERVICE_NAME);
}
export const DsStyleLoaderServiceProvider = {
    provide: DsStyleLoaderService,
    useFactory: DsStyleLoaderServiceProviderFactory,
    deps: ['$injector']
};

export function ScriptLoaderServiceProviderFactory($injector:any){
    return $injector.get(ScriptLoaderService.SERVICE_NAME);
}
export const ScriptLoaderServiceProvider = {
    provide: ScriptLoaderService,
    useFactory: ScriptLoaderServiceProviderFactory,
    deps: ['$injector']
};

export function DsPopupServiceProviderFactory($injector:any) {
    return $injector.get(DsPopupService.SERVICE_NAME);
}
export const DsPopupServiceProvider = {
    provide: DsPopupService,
    useFactory: DsPopupServiceProviderFactory,
    deps: ['$injector']
};

export function DsOnboardingAdminApiServiceFactory($injector: any) {
    return $injector.get(DsOnboardingAdminApiService.SERVICE_NAME);
}

export const DsOnboardingAdminApiServiceProvider = {
    provide: DsOnboardingAdminApiService,
    useFactory: DsOnboardingAdminApiServiceFactory,
    deps: ['$injector']
}

export function ExternalApiServiceFactory($injector: any) {
    return $injector.get(ExternalApiService.SERVICE_NAME);
}

export const ExternalApiServiceProvider = {
    provide: ExternalApiService,
    useFactory: ExternalApiServiceFactory,
    deps: ['$injector']
}
export function DsEmployeeAttachmentModalFactory($injector:any) {
    return $injector.get(DsEmployeeAttachmentModalService.SERVICE_NAME);
}

export const DsEmployeeAttachmentModalProvider = {
    provide: DsEmployeeAttachmentModalService,
    useFactory: DsEmployeeAttachmentModalFactory,
    deps: ['$injector']
};

export function DsEmployeeAttachmentApiFactory($injector:any) {
    return $injector.get(DsEmployeeAttachmentApiService.SERVICE_NAME);
}

export const DsEmployeeAttachmentApiProvider = {
    provide: DsEmployeeAttachmentApiService,
    useFactory: DsEmployeeAttachmentApiFactory,
    deps: ['$injector']
};

export function DsResourceApiFactory($injector:any) {
    return $injector.get(DsResourceApi.SERVICE_NAME);
}

export const DsResourceApiProvider = {
    provide: DsResourceApi,
    useFactory: DsResourceApiFactory,
    deps: ['$injector']
};

export function DailyRulesModalServiceFactory($injector:any) {
    return $injector.get(DailyRulesModalService.SERVICE_NAME);
}

export const DailyRulesModalServiceProvider = {
    provide: DailyRulesModalService,
    useFactory: DailyRulesModalServiceFactory,
    deps: ['$injector']
};

export function CountryStateServiceFactory($injector:any) {
    return $injector.get(CountryStateService.SERVICE_NAME);
}

export const CountryStateServiceProvider = {
    provide: CountryStateService,
    useFactory: CountryStateServiceFactory,
    deps: ['$injector']
};

export function DsConfigurationServiceProviderFactory($injector:any) {
    return $injector.get(DsConfigurationService.SERVICE_NAME);
}

export const DsConfigurationServiceUpgradedProvider = {
    provide: DsConfigurationService,
    useFactory: DsConfigurationServiceProviderFactory,
    deps: ['$injector']
};

export function ClientSelectorServiceFactory($injector: any) {
    return $injector.get(AjsClientSelectorService.SERVICE_NAME);
}

export const ClientSelectorServiceProvider = {
    provide: AjsClientSelectorService,
    useFactory: ClientSelectorServiceFactory,
    deps: ['$injector']
};

export function DsUtilityServiceProviderFactory($injector:any) {
    return $injector.get(DsUtilityServiceProvider.SERVICE_NAME);
}

export const DsUtilityServiceUpgradedProvider = {
    provide: DsUtilityServiceProvider,
    useFactory: DsUtilityServiceProviderFactory,
    deps: ['$injector']
};
