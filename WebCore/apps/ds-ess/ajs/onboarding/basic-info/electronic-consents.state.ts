import { DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingElectronicConsentsController } from "./electronic-consents.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingElectronicConsentState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingElectronicConsent';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingElectronicConsentsController,
        template: require('./electronic-consents.html'),
    };
    static STATE_CONFIG = {
        parent: 'ess.onboarding',
        name: 'electronic-consents',
        url: '/electronicconsents',
        template: require('./electronic-consents.html'),
        controller: OnboardingElectronicConsentsController
    };
    static STATE_OPTIONS = {
        pageTitle: 'Electronic Consents'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingElectronicConsentState.STATE_CONFIG, EssOnboardingElectronicConsentState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
