import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingDependentsController } from "./dependents.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingDependentsState{
    static readonly COMPONENTS_NAME = 'ajsEssOnboardingDependents';
    static readonly COMPONENTS_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingDependentsController,
        template: require('./dependents.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'dependents',
        url: '/dependents',
        template: require('./dependents.html'),
        controller: OnboardingDependentsController
    }
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Dependents'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingDependentsState.STATE_CONFIG, EssOnboardingDependentsState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
