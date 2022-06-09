import { IUiState, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingVideoController } from "./video.controller";

export class EssOnboardingVideoState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingVideo';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingVideoController,
        template: require('./video.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'video',
        url: '/video/:workFlowTaskId',
        template: require('./video.html'),
        controller: OnboardingVideoController
    };
    static STATE_OPTIONS = {
        pageTitle: 'Videos'
    };

    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingVideoState.STATE_CONFIG, EssOnboardingVideoState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
