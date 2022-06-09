import * as angular from 'angular';
import { MESSAGE_FAILURE_FORM_SUBMISSION_ESS } from '@util/ds-common';
import { CountryStateService } from '@ajs/location/country-state/country-state.svc';
import { DsNavigationService } from '../../ui/nav/ds-navigation.service';
export class OnboardingCompanyInfoAddController {
    static readonly $inject = [
        '$scope',
        'DsMsg',
        'EmployeeContactInformationService',
        'DsInlineValidatedInputService',
        '$controller',
        CountryStateService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
    ];
    constructor ($scope, MessageService, EmployeeContactInformationService,
        DsInlineValidatedInputService, $controller, locations: CountryStateService, nav: DsNavigationService) {

        function init(employeeContactInformationData, countriesData, countryStatesData) {
            // ------------------------------------------------------------------------
            // initialize variables
            // ------------------------------------------------------------------------
            $controller('CountryAndStateController', { $scope: $scope });
            let originalData = {};
            originalData = angular.copy(employeeContactInformationData);
            $scope.employeeContactInformationData = angular.copy(employeeContactInformationData);

            function currentUserLoaded(data) {

                $scope.userData = data;

            }

            function errorCallback(data) {
            }

            // set country and state controller data, and initialize it
            $scope.countryAndState.initialize({
                countryId: $scope.employeeContactInformationData.countryId,
                stateId: $scope.employeeContactInformationData.stateId,
                countries: countriesData,
                states: countryStatesData
            });


            // set country and state controller data, and initialize it
            $scope.countryAndState.initialize({
                countryId: $scope.employeeContactInformationData.countryId,
                stateId: $scope.employeeContactInformationData.stateId,
                countries: countriesData,
                states: countryStatesData
            });

            $scope.countryAndState.setModifiedSnapshot();

            // --------------------------------------------------------
            // todo: refactor: (jay): create a submit button that can take this codes place and call the controllers submit if everything is good
            // this will validate all dsInlineValidtedInput controls before it goes to the server
            // --------------------------------------------------------
            $scope.validateThenSubmit = function () {
                DsInlineValidatedInputService.validateAll()
                .then(
                    function () {
                        $scope.updateContactInformation();
                    },
                    function () {
                        MessageService.setMessage(
                            MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                            MessageService.messageTypes.error);
                    }
                );
            };

            // --------------------------------------------------------
            // This function is called when the 'Save' (submit) button is clicked.
            // This will push the data to the server via the
            // EmployeeContactInformation Service
            // --------------------------------------------------------
            $scope.updateContactInformation = function () {
                // console.log('GOING TO THE SERVER');
                MessageService.sending(true);

                // the contry and state controller stores all the country data, so we need to transfer that to the entity before saving.
                $scope.employeeContactInformationData.countryId = $scope.countryAndState.countryId;
                $scope.employeeContactInformationData.stateId = $scope.countryAndState.stateId;

                // push the data to the server
                EmployeeContactInformationService
                    .putEmployeeContactInfo($scope.employeeContactInformationData)
                    .then(successfulSave)
                    .catch(MessageService.showWebApiException);
            };

            // --------------------------------------------------------
            // this is called if save (or request change) is successful
            // --------------------------------------------------------
            function successfulSave() {
                nav.gotoProfileThenShowSuccessMessage();
            }

            // --------------------------------------------------------
            // On cancel redirect to the contact info view page
            // todo: refactor: (jay) this function is duplicated in several controllers
            // --------------------------------------------------------
            $scope.cancelEdit = function () {
                nav.gotoProfile();
            };

            // --------------------------------------------------------
            // Indicates if the scope's contact info has changed from
            // the original data.
            // --------------------------------------------------------
            $scope.isDataModified = function () {
                // return !angular.equals($scope.employeeContactInformationData, originalData);
                let result =
                    !angular.equals($scope.employeeContactInformationData, originalData) ||
                        $scope.countryAndState.isModified();

                return result;
            };
        }

        nav.getCacheForCurrentRoute().then(employeeData => {
            locations.getCountryList().then(countries => {
                locations.getStatesByCountry(employeeData.countryId).then(states => {
                    init(employeeData, countries, states);
                });
            });
        });
    }
}
