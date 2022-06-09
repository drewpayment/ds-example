import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { BenefitInfoReviewController } from "./info-review.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.info
//--------------------------------------------------------------------------------
export class EssBenefitsInfoState {
    static readonly COMPONENT_NAME = 'ajsEssBenefitsInfoReview';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: BenefitInfoReviewController,
        template: require('./info-review.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.benefits',
        name: 'info',
        url: '/info',
        template: require('./info-review.html'),
        controller: BenefitInfoReviewController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Benefit Info Review'
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssBenefitsInfoState.STATE_CONFIG, EssBenefitsInfoState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
