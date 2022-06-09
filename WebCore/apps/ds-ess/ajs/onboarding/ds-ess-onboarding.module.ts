import * as angular from "angular";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsEmployeeModule } from "@ajs/employee/ds-employee.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";
import { DsTaxesModule } from "@ajs/taxes/ds-taxes.module";
import { DsLocationModule } from "@ajs/location/ds-location.mod";
import { DsEssUiModule } from "../ui/ds-ess-ui.module";
import { DsEssCommonModule } from "../common/ds-ess-common.module";
import { EssOnboardingDependentsState } from "./basic-info/dependents.state";
import { EssOnboardingEeocState } from "./basic-info/eeoc.state";
import { EssOnboardingElectronicConsentState } from "./basic-info/electronic-consents.state";
import { EssOnboardingEmergencyContactState } from "./basic-info/emergency-contact.state";
import { EssOnboardingPaymentPreferenceState } from "./basic-info/payment-preference.state";
import { EssOnboardingCompanyInfoState } from "./company-info/company-info.state";
import { EssOnboardingContactInfoState } from "./contact-info/contact.state";
import { EssOnboardingDocumentState } from "./custom-pages/document.state";
import { EssOnboardingLinkState } from "./custom-pages/link.state";
import { EssOnboardingVideoState } from "./custom-pages/video.state";
import { EssOnboardingFinalizeState } from "./finalize/finalize.state";
import { EssOnboardingHomeState } from "./home/home.state";
import { EssOnboardingDefaultState } from "./home/home.state.default";
import { EssOnboardingI9State } from "./i9/i9.state";
import { EssOnboardingW4AssistState } from "./w4/assist.state";
import { EssOnboardingW4FederalState } from "./w4/federal.state";
import { CapitalizeFilter } from "./shared/capitalize.filter";
import { EssOnboardingW4StateState } from "./w4/state.state";
import { EssOnboardingEmployeeBioState } from "./basic-info/employee-bio.state";
import { EssOnboardingOtherInfoState } from "./basic-info/other-info.state";
import { EssOnboardingHeaderComponent } from '../common/header/headerOnboarding.controller';
import { WorkflowSidebarComponent } from '../common/main-sidebar/workflow-sidebar.controller';
import { OnboardingCompanyInfoAddController } from './company-info/company-info.controller';
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsOnboardingAdminApiService } from "@ajs/onboarding/shared/ds-admin-api.service";
import { DsOnboardingWorkflowApiService } from "@ajs/onboarding/shared/ds-admin-workflow.service";
import { DsOnboardingFormsApiService } from '@ajs/onboarding/forms/ds-onboarding-forms-api.service';

/**
 * @ngdoc module
 * @module ds.ess.app
 *
 * @description
 * Primary ESS angular application module in which all required dependencies will be built.
 */
export module DsEssOnboardingModule {
    export const AjsModule = angular.module('ds.ess.onboarding',
    [
        'ngSanitize',
        'ngFileSaver',
        DsCoreModule.AjsModule.name,
        DsEmployeeModule.AjsModule.name,
        DsUiModule.AjsModule.name,
        DsTaxesModule.AjsModule.name,
        DsEssCommonModule.AjsModule.name,
        DsEssUiModule.AjsModule.name,
        DsLocationModule.AjsModule.name,
    ]);

    //services, filters, etc
    AjsModule
        .filter(CapitalizeFilter.FILTER_NAME, CapitalizeFilter.$filter())
        .service(DsOnboardingAdminApiService.SERVICE_NAME, DsOnboardingAdminApiService)
        .service(DsOnboardingWorkflowApiService.SERVICE_NAME, DsOnboardingWorkflowApiService)
        .service(DsOnboardingEmployeeApiService.SERVICE_NAME, DsOnboardingEmployeeApiService)
        .service(DsOnboardingFormsApiService.SERVICE_NAME, DsOnboardingFormsApiService)
        .component(EssOnboardingHeaderComponent.COMPONENT_NAME, EssOnboardingHeaderComponent.COMPONENT_OPTIONS)
        .component(WorkflowSidebarComponent.COMPONENT_NAME, WorkflowSidebarComponent.COMPONENT_OPTIONS)
        .component(EssOnboardingHomeState.COMPONENT_NAME, EssOnboardingHomeState.COMPONENT_OPTIONS)
        .component(EssOnboardingContactInfoState.COMPONENT_NAME, EssOnboardingContactInfoState.COMPONENT_OPTIONS)
        .component(EssOnboardingDependentsState.COMPONENTS_NAME, EssOnboardingDependentsState.COMPONENTS_OPTIONS)
        .component(EssOnboardingEeocState.COMPONENT_NAME, EssOnboardingEeocState.COMPONENT_OPTIONS)
        .component(EssOnboardingElectronicConsentState.COMPONENT_NAME, EssOnboardingElectronicConsentState.COMPONENT_OPTIONS)
        .component(EssOnboardingEmergencyContactState.COMPONENT_NAME, EssOnboardingEmergencyContactState.COMPONENT_OPTIONS)
        .component(EssOnboardingPaymentPreferenceState.COMPONENT_NAME, EssOnboardingPaymentPreferenceState.COMPONENT_OPTIONS)
        .component(EssOnboardingCompanyInfoState.COMPONENT_NAME, EssOnboardingCompanyInfoState.COMPONENT_OPTIONS)
        .component(EssOnboardingDocumentState.COMPONENT_NAME, EssOnboardingDocumentState.COMPONENT_OPTIONS)
        .component(EssOnboardingLinkState.COMPONENT_NAME, EssOnboardingLinkState.COMPONENT_OPTIONS)
        .component(EssOnboardingVideoState.COMPONENT_NAME, EssOnboardingVideoState.COMPONENT_OPTIONS)
        .component(EssOnboardingFinalizeState.COMPONENT_NAME, EssOnboardingFinalizeState.COMPONENT_OPTIONS)
        .component(EssOnboardingDefaultState.COMPONENT_NAME, EssOnboardingDefaultState.COMPONENT_OPTIONS)
        .component(EssOnboardingI9State.COMPONENT_NAME, EssOnboardingI9State.COMPONENT_OPTIONS)
        .component(EssOnboardingW4AssistState.COMPONENT_NAME, EssOnboardingW4AssistState.COMPONENT_OPTIONS)
        .component(EssOnboardingW4FederalState.COMPONENT_NAME, EssOnboardingW4FederalState.COMPONENT_OPTIONS)
        .component(EssOnboardingW4StateState.COMPONENT_NAME, EssOnboardingW4StateState.COMPONENT_OPTIONS)
        .component(EssOnboardingEmployeeBioState.COMPONENT_NAME, EssOnboardingEmployeeBioState.COMPONENT_OPTIONS)
        .component(EssOnboardingOtherInfoState.COMPONENT_NAME, EssOnboardingOtherInfoState.COMPONENT_OPTIONS);

    //ui-router states
    AjsModule
        .config(EssOnboardingDependentsState.$config())
        .config(EssOnboardingEeocState.$config())
        .config(EssOnboardingElectronicConsentState.$config())
        .config(EssOnboardingEmergencyContactState.$config())
        .config(EssOnboardingPaymentPreferenceState.$config())
        .config(EssOnboardingCompanyInfoState.$config())
        .config(EssOnboardingContactInfoState.$config())
        .config(EssOnboardingDocumentState.$config())
        .config(EssOnboardingLinkState.$config())
        .config(EssOnboardingVideoState.$config())
        .config(EssOnboardingFinalizeState.$config())
        .config(EssOnboardingDefaultState.$config())
        .config(EssOnboardingHomeState.$config())
        .config(EssOnboardingI9State.$config())
        .config(EssOnboardingW4AssistState.$config())
        .config(EssOnboardingW4FederalState.$config())
        .config(EssOnboardingW4StateState.$config())
        .config(EssOnboardingEmployeeBioState.$config())
        .config(EssOnboardingOtherInfoState.$config());;
}
