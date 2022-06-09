import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { BenefitSummaryController } from "./summary.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefit.summary
//--------------------------------------------------------------------------------
export class EssBenefitsSummaryState {
    static COMPONENT_NAME = 'ajxEssBenefitsSummary';
    static COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: BenefitSummaryController,
        template: require('./summary.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.benefits',
        name: 'summary',
        url: '/summary',
        template: require('./summary.html'),
        controller: BenefitSummaryController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Benefit Summary',
        permissions: ['Benefit.GetPlans']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssBenefitsSummaryState.STATE_CONFIG, EssBenefitsSummaryState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}

