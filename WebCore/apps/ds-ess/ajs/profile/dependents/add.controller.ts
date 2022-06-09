import { STATES } from "../../shared/state-router-info";
import { DsEmployeeDependentService } from "@ajs/employee/dependent.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsSsnMaskService } from "@ajs/core/ssn-mask/ds-ssn-mask.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { MESSAGE_FAILURE_FORM_SUBMISSION_ESS } from "@util/ds-common";
import { calculateBirthdateWithText } from "@util/dateUtilities";
import * as angular from "angular";

export class AddDependentController {
    static readonly CONTROLLER_NAME = 'AddDependentController';
    static readonly $inject = [
        '$scope',
        '$filter',
        DsEmployeeDependentService.SERVICE_NAME,
        STATES.ds.ess.RESOLVE.userAccount,
        DsInlineValidatedInputService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsSsnMaskService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME
    ];

    constructor($scope, $filter, EmployeeDependentService, userAccount, DsInlineValidatedInputService, MessageService, ssnMask, DsNavigationService) {
        //--------------------------------------------------------
        //initialize variables
        //--------------------------------------------------------
        $scope.employeeDependent = {};
        var originalDependentData = <any>{};

        originalDependentData.birthDate = '';

        //--------------------------------------------------------
        //todo: refactor: (jay) this is duplicated code in the add and edit dependendents
        //--------------------------------------------------------
        $scope.genderTxtValuePairs = [{ txt: 'Male', val: 'M' }, { txt: 'Female', val: 'F' }];
        $scope.YesNoPairs = [{ txt: 'Yes', val: true }, { txt: 'No', val: false }];
        
        //--------------------------------------------------------
        //todo: refactor: (jay): create a submit button (directive) that can take this codes place and call the controllers submit if everything is good 
        //this will validate all dsInlineValidtedInput controls before it goes to the server
        //--------------------------------------------------------
        $scope.validateThenSubmit = function () {
            DsInlineValidatedInputService.validateAll()
            .then(
                function () {
                    $scope.addEmployeeDependent();
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
        $scope.addEmployeeDependent = function () {
            MessageService.sending(true);

            //These are the current logged in users values (employeeId).
            $scope.employeeDependent.employeeId = userAccount.employeeId;

            EmployeeDependentService.addDependent($scope.employeeDependent).then(
                function(data, status) {
                    $scope.employeeDependent = data;

                    //Format the data in the same format as the database so there is no 'change' detected when submitted.
                    $scope.employeeDependent.birthDate = $filter('date')(data.birthDate, 'MM/dd/yyyy');

                    DsNavigationService.gotoProfileThenShowSuccessMessage();
                },
                MessageService.showWebApiException
            );
        };
        
        ////--------------------------------------------------------
        ////watch for changes on the birth date so we can calc the age
        ////--------------------------------------------------------
        $scope.$watch('employeeDependent.birthDate', function () {
            $scope.updateAge();
        });

        //--------------------------------------------------------
        //udpates the age
        //--------------------------------------------------------
        $scope.updateAge = function () {
            var date = '';

            if (typeof $scope.employeeDependent.birthDate != "undefined") {
                date = $scope.employeeDependent.birthDate;
            }

            //console.log('ADD DEPENDENT BDAY WATCH: ' + date);

            $scope.age = calculateBirthdateWithText(date);
        };

        //--------------------------------------------------------
        // checks if the current dependent data on the scope has been modified from its original values.
        //--------------------------------------------------------
        $scope.isDataModified = function () {
            //console.log('IS DATA MODIFIED CHECK');
            return !angular.equals($scope.employeeDependent, originalDependentData);
        };



        //--------------------------------------------------------
        // On cancel redirect to the contact info view page
        // todo: refactor: (jay) this function is duplicated in several controllers
        //--------------------------------------------------------
        $scope.cancelEdit = function () {
            DsNavigationService.gotoProfile();
        };

        //--------------------------------------------------------
        //used for all async errors
        //--------------------------------------------------------
        function errorCallback(data, status, headers, config) {
            MessageService.xhrMessageHandler(data);
        }
        
        /**
        * Updates the Unmasked SSN variable on the current dependent using the web service.
        *
        * @ngmethod
        * @name AddDependentController#updateSsnMask
        * @returns Angular promise that can be used to handle success and error data created while masking the SSN.
        */
        $scope.updateSsnMask = function () {
            return ssnMask.getMaskedSsn($scope.employeeDependent.unmaskedSocialSecurityNumber)
                .then(function (masked) {
                    $scope.employeeDependent.maskedSocialSecurityNumber = masked;
                });
        };
    }
}