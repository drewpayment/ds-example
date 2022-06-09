import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingDocumentController } from "./document.controller";

export class EssOnboardingDocumentState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingDocument';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingDocumentController,
        template: require('./document.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'document',
        url: '/document/:workFlowTaskId',
        template: require('./document.html'),
        controller: OnboardingDocumentController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Documents'
    };

    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingDocumentState.STATE_CONFIG, EssOnboardingDocumentState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
