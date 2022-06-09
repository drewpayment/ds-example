import { DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { AddDependentController } from "./add.controller";

//--------------------------------------------------------------------------------
// STATE: ess.profile.dependents.add
//--------------------------------------------------------------------------------
export class EssProfileDependentsAddState {
    static STATE_CONFIG = {
        parent: 'ess.profile.dependents',
        name: 'add',
        url: '/Add',
        template: require('./add.html'),
        controller: AddDependentController
    };

    static STATE_OPTIONS = {
        pageTitle: 'Add Dependent',
        permissions: ['Employee.EmployeeDependentUpdate']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssProfileDependentsAddState.STATE_CONFIG, EssProfileDependentsAddState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
    