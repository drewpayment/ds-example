import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingW4FederalController } from "./federal.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingW4FederalState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingW4Federal';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingW4FederalController,
        template: require('./federal.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'w4Federal',
        url: '/w4/federal',
        template: require('./federal.html'),
        controller: OnboardingW4FederalController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'W4 Federal'
    };

    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingW4FederalState.STATE_CONFIG, EssOnboardingW4FederalState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
