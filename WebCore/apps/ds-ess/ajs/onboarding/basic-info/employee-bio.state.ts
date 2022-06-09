import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { OnboardingEmployeeBioController } from "./employee-bio.controller";

export class EssOnboardingEmployeeBioState {
    static readonly COMPONENT_NAME = 'ajsEssOnboardingEmployeeBio';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingEmployeeBioController,
        template: require('./employee-bio.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'employee-bio',
        url: '/employeebio',
        template: require('./employee-bio.html'),
        controller: OnboardingEmployeeBioController,
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'EmployeeBio'
    };

    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingEmployeeBioState.STATE_CONFIG, EssOnboardingEmployeeBioState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
