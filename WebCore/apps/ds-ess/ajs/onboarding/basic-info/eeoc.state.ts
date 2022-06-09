import { IUiState, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingEEOCAddController } from "./eeoc.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingEeocState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingEeoc';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingEEOCAddController,
        template: require('./EEOC.html'),
    };
    static STATE_CONFIG: IUiState ={
        parent: 'ess.onboarding',
        name: 'EEOC',
        url: '/eeoc',
        template: require('./EEOC.html'),
        controller: OnboardingEEOCAddController
    };
    static STATE_OPTIONS = {
        pageTitle: 'EEOC'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingEeocState.STATE_CONFIG, EssOnboardingEeocState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
