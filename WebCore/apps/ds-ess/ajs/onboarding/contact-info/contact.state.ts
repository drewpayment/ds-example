import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingContactInfoAddController } from "./contact.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingContactInfoState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingContactInfo';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingContactInfoAddController,
        template: require('./contact.html'),
    };
    static STATE_CONFIG:IUiState = {
        parent: 'ess.onboarding',
        name: 'contact-info',
        url: '/contactinfo',
        template: require('./contact.html'),
        controller: OnboardingContactInfoAddController

    };

    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Contact Information'
    };

    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingContactInfoState.STATE_CONFIG, EssOnboardingContactInfoState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
