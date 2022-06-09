import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingHomeController } from "./home.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingHomeState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingHome';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingHomeController,
        template: require('./home.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'home',
        url: '/home',
        template: require('./home.html'),
        controller: OnboardingHomeController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Onboarding Home'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingHomeState.STATE_CONFIG, EssOnboardingHomeState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
