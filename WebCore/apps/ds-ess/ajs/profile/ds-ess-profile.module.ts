import * as angular from "angular";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsEmployeeModule } from "@ajs/employee/ds-employee.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";
import { DsTaxesModule } from "@ajs/taxes/ds-taxes.module";
import { DsEssCommonModule } from "../common/ds-ess-common.module";
import { DsEssUiModule } from "../ui/ds-ess-ui.module";
import { EssProfileContactInfoEditState } from "./contact-info/edit.state";
import { DependentViewModalService } from "./dependents/view-modal.service";
import { EmergencyContactViewModalService } from "./emergency-contacts/view-modal.service";
import { EssProfileDependentsAddState } from "./dependents/add.state";
import { EssProfileDependentsEditState } from "./dependents/edit.state";
import { EssProfileEmergencyContactsAddState } from "./emergency-contacts/add.state";
import { EssProfileEmergencyContactsEditState } from "./emergency-contacts/edit.state";
import { DsCompanyResourceModule } from "@ajs/company-resource/ds-company-resource.module";
import { EssPayViewState, EssPayViewAllState } from "./paycheck/taxes/view.state";
import { EssProfilePersonalInfoEditState } from "./personal-info/edit.state";

/**
 * @ngdoc module
 * @module ds.ess.profile
 */
export module DsEssProfileModule {
    export const AjsModule = angular.module('ds.ess.profile',
    [
        DsCoreModule.AjsModule.name,
        DsEmployeeModule.AjsModule.name,
        DsUiModule.AjsModule.name,
        DsTaxesModule.AjsModule.name,
        DsEssCommonModule.AjsModule.name,
        DsEssUiModule.AjsModule.name,
        DsCompanyResourceModule.AjsModule.name,
    ]);

    //services
    AjsModule
        .service(DependentViewModalService.SERVICE_NAME, DependentViewModalService)
        .service(EmergencyContactViewModalService.SERVICE_NAME, EmergencyContactViewModalService);

    //ui-router states
    AjsModule
        .config(EssProfileContactInfoEditState.$config())
        .config(EssProfileDependentsAddState.$config())
        .config(EssProfileDependentsEditState.$config())
        .config(EssProfileEmergencyContactsAddState.$config())
        .config(EssProfileEmergencyContactsEditState.$config())
        .config(EssPayViewState.$config())
        .config(EssPayViewAllState.$config())
        .config(EssProfilePersonalInfoEditState.$config());
}
