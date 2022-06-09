import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingW4StateController } from "./state.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingW4StateState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingW4State';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingW4StateController,
        template: require('./state.html'),
    };
    static STATE_CONFIG: IUiState =  {
        parent: 'ess.onboarding',
        name: 'w4State',
        url: '/w4/state',
        template: require('./state.html'),
        controller: OnboardingW4StateController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'W4 State'
    };

    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingW4StateState.STATE_CONFIG, EssOnboardingW4StateState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
