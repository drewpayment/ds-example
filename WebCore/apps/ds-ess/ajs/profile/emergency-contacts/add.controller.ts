import * as angular from "angular";
import { DsEmployeeEmergencyContactService } from "@ajs/employee/emergency-contact.service";
import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { CountryAndStateControllerBase } from "../../common/base/country-state.controller";
import { MESSAGE_FAILURE_FORM_SUBMISSION_ESS } from "@util/ds-common";

export class AddEmployeeEmergencyContactController {

    static readonly $inject = [
        '$scope',
        DsEmployeeEmergencyContactService.SERVICE_NAME,
        STATES.ds.ess.RESOLVE.userAccount,
        DsMsgService.SERVICE_NAME,
        DsInlineValidatedInputService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        '$controller'
    ];

    constructor($scope, EmployeeEmergencyContactService, userAccount, MessageService, DsInlineValidatedInputService, DsNavigationService, $controller) {

        $controller(CountryAndStateControllerBase.CONTROLLER_NAME, { $scope: $scope });
        $scope.countryAndState.loadCountries();
        var originalData = {};
        $scope.frmName = null;

        var initCode = function () {

            //--------------------------------------------------------
            //initialize variables
            //--------------------------------------------------------
            $scope.currentUser = {};
            $scope.employeeEmergencyContact = {};
            originalData = angular.copy($scope.employeeEmergencyContact);

        }();

        //--------------------------------------------------------
        //validate all the directives using the directive service
        //--------------------------------------------------------
        $scope.validateThenSubmit = function (formName) {

            DsInlineValidatedInputService.validateAll(formName)
                .then(
                    function () {
                        $scope.addEmployeeEmergencyContact();
                    },
                    function () {
                        MessageService.setMessage(
                            MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                            MessageService.messageTypes.error);
                    }
                );
        };

        //--------------------------------------------------------
        //This function is called when the 'Save' (submit) button is clicked.
        //--------------------------------------------------------
        $scope.addEmployeeEmergencyContact = function () {
            MessageService.sending(true);

            //These are the current logged in users values (clientId and employeeId).
            $scope.employeeEmergencyContact.clientId = userAccount.clientId;
            $scope.employeeEmergencyContact.employeeId = userAccount.employeeId;

            //the contry and state controller stores all the country data, so we need to transfer that to the entity before saving.
            $scope.employeeEmergencyContact.countryId = $scope.countryAndState.countryId;
            $scope.employeeEmergencyContact.stateId = $scope.countryAndState.stateId;

            //Now send the new emergency contact data to be added
            EmployeeEmergencyContactService.saveEmergencyContact($scope.employeeEmergencyContact, { isChangeRequest: false }).then(
                //----------
                // success
                //----------
                function (data) {
                    DsNavigationService.gotoRouteThenShowMessage(STATES.ds.ess.MENU_NAME, "Your emergency contact has been added successfully.");
                },
                //----------
                // error
                //----------
                //errorCallback
                MessageService.showWebApiException
            );
        };

        //--------------------------------------------------------
        // checks if the model object is modified
        //--------------------------------------------------------
        $scope.isDataModified = function () {
            //return !angular.equals($scope.employeeEmergencyContact, originalData);
            var result =
                !angular.equals($scope.employeeEmergencyContact, originalData) ||
                    $scope.countryAndState.isModified();

            return result;
        };

        //--------------------------------------------------------
        //handles the cancel request
        //--------------------------------------------------------
        $scope.cancelEdit = function () {
            DsNavigationService.gotoProfile();
        };

    }
}
