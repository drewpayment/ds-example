import * as angular from "angular";
import { MESSAGE_FAILURE_FORM_SUBMISSION_ESS } from "@util/ds-common";
import { calculateBirthdateWithText } from "@util/dateUtilities";

export class EditDependentController {
    static readonly $inject = [
        '$scope',
        'dependent',
        '$location',
        '$filter',
        'EmployeeDependentService',
        'DsInlineValidatedInputService',
        'DsMsg',
        'DsSsnMaskService',
        'DsNavigationService'
    ];

    constructor($scope, dependent, $location, $filter, EmployeeDependentService, DsInlineValidatedInputService, MessageService, ssnMask, DsNavigationService) {

        //--------------------------------------------------------
        //initialize variables
        //--------------------------------------------------------
        $scope.currentUser = {};
        $scope.employeeDependent = {};
        $scope.age = '';
        var originalDependentData = {};

        //--------------------------------------------------------
        //todo: refactor: (jay) this is duplicated code in the add and edit dependendents
        //--------------------------------------------------------
        $scope.genderTxtValuePairs = [{ txt: 'Male', val: 'M' }, { txt: 'Female', val: 'F' }];
        $scope.YesNoPairs = [{ txt: 'Yes', val: true }, { txt: 'No', val: false }];
        //--------------------------------------------------------
        // initialize the controller with the given dependent's info 
        //--------------------------------------------------------
        (function initCtrl() {
            // save a copy of the dependent info on the scope 
            $scope.employeeDependent = angular.copy(dependent);

            //Format the data in the same format as the database so there is no 'change' detected when submitted.
            $scope.employeeDependent.birthDate = $filter('date')(dependent.birthDate, 'MM/dd/yyyy');

            // save an original copy after the birthdate has been filtered
            originalDependentData = angular.copy($scope.employeeDependent);
        }());

        //--------------------------------------------------------
        //todo: refactor: (jay): create a submit button (directive) that can take this codes place and call the controllers submit if everything is good 
        //this will validate all dsInlineValidtedInput controls before it goes to the server
        //--------------------------------------------------------
        $scope.validateThenSubmit = function() {
            DsInlineValidatedInputService.validateAll()
                .then(
                    function() {
                        $scope.updateEmployeeDependent();
                    },
                    function() {
                        MessageService.setMessage(
                            MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                            MessageService.messageTypes.error);
                    }
                );
        };


        //--------------------------------------------------------
        //This function is called when the 'Save' (submit) button is clicked.
        //--------------------------------------------------------
        $scope.updateEmployeeDependent = function() {
            MessageService.sending(true);
            EmployeeDependentService.updateDependent($scope.employeeDependent).then(
                // success
                function(data, status) {
                    DsNavigationService.gotoProfileThenShowSuccessMessage();
                },
                // error
                MessageService.showWebApiException
            );
        };

        
        ////--------------------------------------------------------
        ////watch for changes on the birth date so we can calc the age
        ////--------------------------------------------------------
        $scope.$watch('employeeDependent.birthDate', function() {
            $scope.updateAge();
        });

        //--------------------------------------------------------
        //udpates the age
        //--------------------------------------------------------
        $scope.updateAge = function() {
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
        $scope.isDataModified = function() {
            return !angular.equals($scope.employeeDependent, originalDependentData);
        };

        //--------------------------------------------------------
        // On cancel redirect to the contact info view page
        // todo: refactor: (jay) this function is duplicated in several controllers
        //--------------------------------------------------------
        $scope.cancelEdit = function() {
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
        * @name EditDependentController#updateSsnMask
        * @returns Angular promise that can be used to handle success and error data created while masking the SSN.
        */
        $scope.updateSsnMask = function() {
            return ssnMask.getMaskedSsn($scope.employeeDependent.unmaskedSocialSecurityNumber)
                .then(function(masked) {
                    $scope.employeeDependent.maskedSocialSecurityNumber = masked;
                });
        };
    }
}
