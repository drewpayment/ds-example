import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingI9Controller } from "./i9.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingI9State {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingI9';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingI9Controller,
        template: require('./i9.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'i9',
        url: '/i9',
        template: require('./i9.html'),
        controller: OnboardingI9Controller
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'I-9'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingI9State.STATE_CONFIG, EssOnboardingI9State.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
