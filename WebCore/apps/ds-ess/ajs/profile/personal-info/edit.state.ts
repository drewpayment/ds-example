import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { EmployeePersonalInfoEditController } from "./edit.controller";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";

export class EssProfilePersonalInfoEditState {
    static STATE_CONFIG: IUiState = {
            parent: 'ess.profile.personalInfo',
            name: 'edit',
            url: '/Edit',
            template: require('./edit.html'),
            controller: EmployeePersonalInfoEditController,
            resolve: {
                pageData: [
                    '$q',
                    DsNavigationService.SERVICE_NAME, 
                    function($q, nav: DsNavigationService) {

                        // create the promise to be resolved once all data has been loaded successfully
                        var deferred = $q.defer();
                        var employeePersonalInformation = nav.getCacheForCurrentRoute();

                        // stop resolve if the employee to load cannot be determined
                        if (!employeePersonalInformation) {
                            deferred.reject('Employee personal info was not specified to edit.');
                        } else {
                            var pageData = {
                                employeePersonalInformationData: employeePersonalInformation
                            };

                            deferred.resolve(pageData);
                        }

                        //------------------------------------------------------------------------
                        //------------------------------------------------------------------------
                        function errorCallback(data) {
                            deferred.reject(data);
                        }

                        return deferred.promise;
                    }
                ]
            }
        };

    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Edit Personal Info',
        permissions: ['Employee.EmployeeUpdate']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssProfilePersonalInfoEditState.STATE_CONFIG, EssProfilePersonalInfoEditState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}