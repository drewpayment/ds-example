import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingFinalizeController } from "./finalize.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingFinalizeState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingFinalize';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingFinalizeController,
        template: require('./finalize.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'finalize',
        url: '/finalize',
        template: require('./finalize.html'),
        controller: OnboardingFinalizeController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Finalize'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingFinalizeState.STATE_CONFIG, EssOnboardingFinalizeState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
