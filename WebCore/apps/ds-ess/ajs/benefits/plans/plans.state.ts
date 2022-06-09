import { BenefitPlansController } from "./plans.controller";
import { DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssBenefitsPlansState {
    static COMPONENT_NAME = 'ajsEssBenefitsPlans';
    static COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: BenefitPlansController,
        template: require('./plans.html'),
    };
    static STATE_CONFIG = {
        parent: 'ess.benefits',
        name: 'plans',
        url: '/plans',
        template: require('./plans.html'),
        controller: BenefitPlansController
    };
    static STATE_OPTIONS = {
        pageTitle: 'Benefit Plans'
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssBenefitsPlansState.STATE_CONFIG, EssBenefitsPlansState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
