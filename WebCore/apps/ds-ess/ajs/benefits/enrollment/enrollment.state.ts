import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { BenefitEnrollmentController } from "./enrollment.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.enrollment
//--------------------------------------------------------------------------------
export class EssBenefitsEnrollmentState {
    static COMPONENT_NAME = 'ajsEssBenefitsEnrollment';
    static COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: BenefitEnrollmentController,
        template: require('./enrollment.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.benefits',
        name: 'enrollment',
        url: '/enrollment',
        template: require('./enrollment.html'),
        controller: BenefitEnrollmentController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Benefits Enrollment'
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssBenefitsEnrollmentState.STATE_CONFIG, EssBenefitsEnrollmentState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
