import * as angular from "angular";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsBenefitsModule } from "@ajs/benefits/ds-benefits.module";
import { DsEmployeeModule } from "@ajs/employee/ds-employee.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";
import { DsMaterial } from "@ajs/ui/material/ds-material.module";
import { EssBenefitsConfirmationState } from "./confirmation/confirmation.state";
import { DsEssUiModule } from "../ui/ds-ess-ui.module";
import { BenefitDependentEditModalService } from "./dependents/edit-modal.service";
import { EssBenefitsEnrollmentState } from "./enrollment/enrollment.state";
import { EssBenefitsHomeState } from "./home/home.state";
import { EssBenefitsInfoState } from "./info-review/info-review.state";
import { BenefitLifeEventRequestModalService } from "./life-event/request-modal.service";
import { BenefitPlanViewModalService } from "./plans/view-modal.service";
import { PlanSelectionModalService } from "./plans/plan-selection-modal.service";
import { EssBenefitsPlansState } from "./plans/plans.state";
import { EssBenefitsSummaryState } from "./summary/summary.state";
import { BenefitsWorkflowHeaderComponent } from "./header/header.component";
import { STATES } from '../shared/state-router-info';
import { AccountService } from '@ajs/core/account/account.service';
import { MenuService } from '@ds/core/app-config/shared/menu.service';
import { downgradeInjectable } from '@angular/upgrade/static';

/**
 * @ngdoc module
 * @module ds.ess.app
 *
 * @description
 * Primary ESS angular application module in which all required dependencies will be built.
 */
export module DsEssBenefitsModule {
    export const AjsModule = angular.module('ds.ess.benefits',
    [
        'ngSanitize',
        DsCoreModule.AjsModule.name,
        DsBenefitsModule.AjsModule.name,
        DsEmployeeModule.AjsModule.name,
        DsEssUiModule.AjsModule.name,
        DsUiModule.AjsModule.name,
        DsMaterial.AjsModule.name
    ]);

    AjsModule
        .service(BenefitDependentEditModalService.SERVICE_NAME, BenefitDependentEditModalService)
        .service(BenefitLifeEventRequestModalService.SERVICE_NAME, BenefitLifeEventRequestModalService)
        .service(BenefitPlanViewModalService.SERVICE_NAME, BenefitPlanViewModalService)
        .service(PlanSelectionModalService.SERVICE_NAME, PlanSelectionModalService)
        .factory(MenuService.AJS_SERVICE_NAME, downgradeInjectable(MenuService))
        .component(BenefitsWorkflowHeaderComponent.SELECTOR, BenefitsWorkflowHeaderComponent.CONFIG)
        .component(EssBenefitsHomeState.COMPONENT_NAME, EssBenefitsHomeState.COMPONENT_OPTIONS)
        .component(EssBenefitsInfoState.COMPONENT_NAME, EssBenefitsInfoState.COMPONENT_OPTIONS)
        .component(EssBenefitsEnrollmentState.COMPONENT_NAME, EssBenefitsEnrollmentState.COMPONENT_OPTIONS)
        .component(EssBenefitsPlansState.COMPONENT_NAME, EssBenefitsPlansState.COMPONENT_OPTIONS)
        .component(EssBenefitsSummaryState.COMPONENT_NAME, EssBenefitsSummaryState.COMPONENT_OPTIONS)
        .component(EssBenefitsConfirmationState.COMPONENT_NAME, EssBenefitsConfirmationState.COMPONENT_OPTIONS);
}
