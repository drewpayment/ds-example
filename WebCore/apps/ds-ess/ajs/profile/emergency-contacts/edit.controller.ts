import * as angular from "angular";
import { MESSAGE_FAILURE_FORM_SUBMISSION_ESS } from "@util/ds-common";
import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsEmployeeEmergencyContactService } from "@ajs/employee/emergency-contact.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { CountryAndStateControllerBase } from "../../common/base/country-state.controller";

export class EditEmployeeEmergencyContactController {
    static readonly $inject = [
        '$scope',
        '$location',
        DsMsgService.SERVICE_NAME,
        DsEmployeeEmergencyContactService.SERVICE_NAME,
        'pageData',
        DsInlineValidatedInputService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        '$controller'
    ];

    constructor($scope, $location, MessageService, EmployeeEmergencyContactService, pageData, DsInlineValidatedInputService, DsNavigationService, $controller) {

        //--------------------------------------------------------
        //initialize the variables
        //--------------------------------------------------------
        $controller(CountryAndStateControllerBase.CONTROLLER_NAME, { $scope: $scope });

        var originalData = {};
        originalData = angular.copy(pageData.employeeEmergencyContact);
        $scope.employeeEmergencyContact = angular.copy(pageData.employeeEmergencyContact);
        


        //set country and state controller data, and initialize it
        $scope.countryAndState.initialize({
            countryId: $scope.employeeEmergencyContact.countryId,
            stateId: $scope.employeeEmergencyContact.stateId,
            countries: pageData.countries,
            states: pageData.countryStates
        });

        $scope.countryAndState.setModifiedSnapshot();

        //--------------------------------------------------------
        //todo: refactor: (jay): create a submit button that can take this codes place and call the controllers submit if everything is good 
        //this will validate all dsInlineValidtedInput controls before it goes to the server
        //--------------------------------------------------------
        $scope.validateThenSubmit = function() {
            DsInlineValidatedInputService.validateAll()
                .then(
                    function() {
                        $scope.updateEmployeeEmergencyContact();
                    },
                    function() {
                        MessageService.setMessage(
                            MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                            MessageService.messageTypes.error);
                    }
                );
        };

        //--------------------------------------------------------
        //updated the model object after the directivs are validated
        //--------------------------------------------------------
        $scope.updateEmployeeEmergencyContact = function() {
            //todo: release: refactor: (jay): (THIS IS FOR ALL FORMS NOT JUST THIS ONE): the input directive will set a NULL value from the server as an empty string if there is interaction with the input. Either find out why this is or setup the change request code check to see if previous val was NULL and cur val is empty string.
            //console.log('EDIT EMERGENCY CONTACT - SEDING DATA TO SERVER');
            MessageService.sending(true);

            //the contry and state controller stores all the country data, so we need to transfer that to the entity before saving.
            $scope.employeeEmergencyContact.countryId = $scope.countryAndState.countryId;
            $scope.employeeEmergencyContact.stateId = $scope.countryAndState.stateId;

            EmployeeEmergencyContactService.saveEmergencyContact($scope.employeeEmergencyContact, { isChangeRequest: false }).then(
                successfulSave,
                MessageService.showWebApiException
            );
        };

        //--------------------------------------------------------
        // this is called if save (or request change) is successful
        //--------------------------------------------------------
        function successfulSave() {
            DsNavigationService.gotoRouteThenShowMessage(STATES.ds.ess.MENU_NAME, "Your emergency contact has been saved successfully.");
        }

        //--------------------------------------------------------
        // checks if the current dependent data on the scope has been modified from its original values.
        //--------------------------------------------------------
        $scope.isDataModified = function() {
            var result =
                !angular.equals($scope.employeeEmergencyContact, originalData) ||
                    $scope.countryAndState.isModified();

            return result;
        };

        //--------------------------------------------------------
        // On cancel redirect to the contact info view page
        // todo: refactor: (jay) this function is duplicated in several controllers
        //--------------------------------------------------------
        $scope.cancelEdit = function() {
            DsNavigationService.gotoProfile();
        };


        // ------------------------------------------------------------------------------------------
        // VIEW HELPERS
        // ------------------------------------------------------------------------------------------


        $scope.hasAddress = function (employeeEmergencyContact) {
            
            var blAddress = !(employeeEmergencyContact.addressLine1.trim() === ''
                && employeeEmergencyContact.city.trim() === ''
                && employeeEmergencyContact.postalCode.trim() === '');
            return blAddress;
        };

        $scope.showAddress = $scope.hasAddress($scope.employeeEmergencyContact);
    }
}