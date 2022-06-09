import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingOtherInfoController } from "./other-info.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingOtherInfoState{
    static readonly COMPONENT_NAME = 'ajsEssOnboardingOtherInfo';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingOtherInfoController,
        template: require('./other-info.html'),
    };

    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'other-info',
        url: '/otherinfo',
        template: require('./other-info.html'),
        controller: OnboardingOtherInfoController
    }
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Other Info'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingOtherInfoState.STATE_CONFIG, EssOnboardingOtherInfoState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}