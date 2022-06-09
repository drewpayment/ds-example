import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { BenefitHomeController } from "./home.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssBenefitsHomeState {
    static readonly COMPONENT_NAME = 'ajsEssBenefitsHome';
    static COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: BenefitHomeController,
        template: require('./home.html'),
    };
    static readonly STATE_CONFIG: IUiState = {
        parent: 'ess.benefits',
        name: 'home',
        url: '',
        template: require('./home.html'),
        controller: BenefitHomeController
    };
    static readonly STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Benefit Home'
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssBenefitsHomeState.STATE_CONFIG, EssBenefitsHomeState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
