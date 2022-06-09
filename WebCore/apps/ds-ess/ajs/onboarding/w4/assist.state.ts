import { IUiState, DsStateServiceProvider, IDsUiStateOptions } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingW4Controller } from "./assist.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingW4AssistState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingW4Assist';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingW4Controller,
        template: require('./assist.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'w4Assist',
        url: '/w4/assist',
        template: require('./assist.html'),
        controller: OnboardingW4Controller
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'W4'
    };

    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingW4AssistState.STATE_CONFIG, EssOnboardingW4AssistState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
