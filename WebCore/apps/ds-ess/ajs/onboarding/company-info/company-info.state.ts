import { WorkflowService } from "../../ui/workflow/workflow.service";
import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { OnboardingCompanyInfoAddController } from "./company-info.controller";

//--------------------------------------------------------------------------------
// STATE: ess.benefits.overview
//--------------------------------------------------------------------------------
export class EssOnboardingCompanyInfoState {
    static readonly COMPONENT_NAME = 'ajxEssOnboardingCompanyInfo';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: OnboardingCompanyInfoAddController,
        template: require('./company-info.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.onboarding',
        name: 'company-info',
        url: '/companyinfo',
        template: require('./company-info.html'),
        controller: OnboardingCompanyInfoAddController,
        resolve: {
            workflow: [
                WorkflowService.SERVICE_NAME, function(svc:WorkflowService) {
                    return svc.getUserWorkflowPromise();
                }
            ],
        }
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Company Info'
        //permissions: ['Employee.EmployeeUpdate']
    };
    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssOnboardingCompanyInfoState.STATE_CONFIG, EssOnboardingCompanyInfoState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
