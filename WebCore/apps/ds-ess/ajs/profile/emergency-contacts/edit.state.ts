import { IDsUiState, IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { EditEmployeeEmergencyContactController } from "./edit.controller";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";

//--------------------------------------------------------------------------------
// STATE: ess.profile.emergencyContacts.edit
//--------------------------------------------------------------------------------
export class EssProfileEmergencyContactsEditState {
    static STATE_CONFIG: IUiState = {
        parent: 'ess.profile.emergencyContacts',
        name: 'edit',
        url: '/Edit',
        template: require('./edit.html'),
        controller: EditEmployeeEmergencyContactController,
        resolve: {
            pageData: [
                DsNavigationService.SERVICE_NAME, 
                CountryStateService.SERVICE_NAME, 
                '$q', 
                function(nav:DsNavigationService, locations:CountryStateService, $q: ng.IQService) {
                    // create the promise to be resolved once all data has been loaded successfully
                    var deferred = $q.defer();
                    var data = {
                        employeeEmergencyContact: <any>{},
                        countries: {},
                        countryStates: {}
                    };

                    //------------------------------------------------------------------------
                    // load up the emergency contact data from the navigation cache
                    //------------------------------------------------------------------------
                    data.employeeEmergencyContact = nav.getCacheForCurrentRoute();

                    if (!data.employeeEmergencyContact) {
                        // reject if not found in cache
                        deferred.reject('No emergency contact data was specified to edit.');
                    } else {
                        // this call will begin the chain of async calls to the server
                        var countryPromise = locations
                            .getCountryList()
                            .then(countriesLoaded, error);

                        var statePromise = locations
                            .getStatesByCountry(data.employeeEmergencyContact.countryId)
                            .then(statesLoaded, error);

                        $q.all([countryPromise, statePromise])
                            .then(resolve, error);
                    }

                    //------------------------------------------------------------------------
                    // HELPERS
                    //------------------------------------------------------------------------
                    function countriesLoaded(countries) {
                        data.countries = countries;
                    }

                    function statesLoaded(states) {
                        data.countryStates = states;
                    }

                    function resolve() {
                        deferred.resolve(data);
                    }

                    function error(reason) {
                        deferred.reject(reason);
                    }

                    //------------------------------------------------------------------------
                    // PROMISE : getPageData()
                    //------------------------------------------------------------------------
                    return deferred.promise;
                }
            ]
        }
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Edit Emergency Contact',
        permissions: ['Employee.EmployeeEmergencyContactUpdate']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssProfileEmergencyContactsEditState.STATE_CONFIG, EssProfileEmergencyContactsEditState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}