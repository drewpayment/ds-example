import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingPaymentPreferenceController } from "./payment-preference.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingPaymentPreferenceState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingPaymentPreference';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingPaymentPreferenceController,
        template: require('./payment-preference.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'payment-preference',
        url: '/paymentpreference',
        template: require('./payment-preference.html'),
        controller: OnboardingPaymentPreferenceController
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Payment Preference'
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingPaymentPreferenceState.STATE_CONFIG, EssOnboardingPaymentPreferenceState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
