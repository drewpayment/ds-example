import { IUiState, DsStateServiceProvider, IDsUiStateOptions } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingLinkController } from "./link.controller";

export class EssOnboardingLinkState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingLink';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingLinkController,
        template: require('./link.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'link',
        url: '/link/:workFlowTaskId',
        template: require('./link.html'),
        controller: OnboardingLinkController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Links'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingLinkState.STATE_CONFIG, EssOnboardingLinkState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
