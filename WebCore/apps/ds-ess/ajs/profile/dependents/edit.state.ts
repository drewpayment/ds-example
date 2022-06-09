import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { EditDependentController } from "./edit.controller";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";

//--------------------------------------------------------------------------------
// STATE: ess.profile.dependents.edit
//--------------------------------------------------------------------------------
export class EssProfileDependentsEditState {
    static STATE_CONFIG: IUiState = {
        parent: 'ess.profile.dependents',
        name: 'edit',
        url: '/Edit',
        template: require('./edit.html'),
        controller: EditDependentController,
        resolve: {
            dependent: [
                DsNavigationService.SERVICE_NAME, 
                '$q', 
                function (nav: DsNavigationService, $q: ng.IQService) {
                    var deferred = $q.defer();

                    var dependent = nav.getCacheForCurrentRoute();

                    if (!dependent) {
                        deferred.reject('No dependent was specified to edit.');
                    }

                    deferred.resolve(dependent);

                    return deferred.promise;
                }
            ]
        }
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Edit Dependent',
        permissions: ['Employee.EmployeeDependentUpdate']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssProfileDependentsEditState.STATE_CONFIG, EssProfileDependentsEditState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}

