import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingEmergencyContactController } from "./emergency-contact.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingEmergencyContactState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingEmergencyContact';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingEmergencyContactController,
        template: require('./emergency-contact.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'emergency-contact',
        url: '/emergencycontact',
        template: require('./emergency-contact.html'),
        controller: OnboardingEmergencyContactController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Emergency Contact'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingEmergencyContactState.STATE_CONFIG, EssOnboardingEmergencyContactState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
