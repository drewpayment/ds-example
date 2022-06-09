import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { EssContactInfoEditController } from "./edit.controller";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";

//--------------------------------------------------------------------------------
// STATE: ess.profile.contactInfo.edit
//--------------------------------------------------------------------------------
export class EssProfileContactInfoEditState {
    static STATE_CONFIG: IUiState = {
        parent: 'ess.profile.contactInfo',
        name: 'edit',
        url: '/Edit',
        template: require('./edit.html'),
        controller: EssContactInfoEditController,
        resolve: {
            pageData: [
                CountryStateService.SERVICE_NAME,
                '$q',
                DsNavigationService.SERVICE_NAME,
                function(locations: CountryStateService, $q, nav: DsNavigationService) {
                    // create the promise to be resolved once all data has been loaded successfully
                    var deferred = $q.defer();
                    var countries = {};
                    var countryStates = {};
                    var counties = {};
                    var employeeContactInformation = nav.getCacheForCurrentRoute();

                    // stop resolve if the employee to load cannot be determined
                    if (!employeeContactInformation) {
                        deferred.reject('Employee contact info was not specified to edit.');
                    } else {
                        locations
                            .getCountryList()
                            .then(countriesSuccess)["catch"](errorCallback);
                    }

                    //------------------------------------------------------------------------
                    //------------------------------------------------------------------------
                    function countriesSuccess(data) {
                        countries = data;
                        locations
                            .getStatesByCountry(employeeContactInformation.countryId)
                            .then(countryStateSuccess)["catch"](errorCallback);
                    }

                    //------------------------------------------------------------------------
                    //------------------------------------------------------------------------
                    function countryStateSuccess(data) {
                        countryStates = data;
                        locations
                            .getCountiesByState(employeeContactInformation.stateId)
                            .then(countySuccess)["catch"](errorCallback);
                    }

                    function countySuccess(data){
                        counties = data;
                        var pageData = {
                            employeeContactInformationData: employeeContactInformation,
                            countriesData: countries,
                            countryStatesData: countryStates,
                            countyData : counties
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
        pageTitle: 'Edit Contact Info',
        permissions: ['Employee.EmployeeUpdate']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssProfileContactInfoEditState.STATE_CONFIG, EssProfileContactInfoEditState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}