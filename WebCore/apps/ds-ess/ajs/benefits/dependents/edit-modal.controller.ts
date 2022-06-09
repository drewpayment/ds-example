import * as angular from "angular";
import * as util from "../../../../../Scripts/util/ds-common";
import { DsSsnMaskService } from "@ajs/core/ssn-mask/ds-ssn-mask.service";
import { DsEmployeeDependentService } from "@ajs/employee/dependent.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { IEmployeeDependent } from "@ajs/employee/models";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";

declare var _:any;

export class BenefitDependentEditModalController {
    static readonly $inject = [
        '$scope',
        'dependent',
        '$location',
        '$filter',
        DsEmployeeDependentService.SERVICE_NAME,
        DsInlineValidatedInputService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsSsnMaskService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        '$modalInstance'
    ];

    constructor(
        $scope: any, 
        dependent: IEmployeeDependent, 
        $location: ng.ILocationService, 
        $filter: ng.IFilterService, 
        EmployeeDependentService: DsEmployeeDependentService, 
        DsInlineValidatedInputService: DsInlineValidatedInputService, 
        MessageService: DsMsgService, 
        ssnMask: DsSsnMaskService, 
        DsNavigationService: DsNavigationService, 
        $modalInstance) {

        //--------------------------------------------------------
        //initialize variables
        //--------------------------------------------------------
        $scope.employeeDependent = {};
        $scope.age = '';
        var originalDependentData = {};

        $scope.genderTxtValuePairs = [{ txt: 'Male', val: 'M' }, { txt: 'Female', val: 'F' }];
        $scope.YesNoPairs = [{ txt: 'Yes', val: true }, { txt: 'No', val: false }];

        $scope.relationshipChanged = relationshipChanged;

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

            DsInlineValidatedInputService.clearAllRegistrations();

        }());

        
        //--------------------------------------------------------
        //This will validate all dsInlineValidtedInput controls before it goes to the server
        //--------------------------------------------------------
        $scope.validateThenSubmitWithoutChangeRequest = function () {
            $scope.editDependentForm.$setSubmitted();
            DsInlineValidatedInputService.validateAll()
                .then(
                    function () {
                        $scope.updateEmployeeDependentWithoutChangeRequest();
                    },
                    function () {
                        MessageService.setMessage(
                            window.MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                            MessageTypes.error);
                    }
                );
        };
        
        //--------------------------------------------------------
        //This updates the dependent without going through the change request process
        //--------------------------------------------------------
        $scope.updateEmployeeDependentWithoutChangeRequest = function () {
            MessageService.sending(true);

            var promise;
            if($scope.employeeDependent.employeeDependentId) {
                promise = EmployeeDependentService.updateDependent($scope.employeeDependent, {isChangeRequest:false});
            } else {
                promise = EmployeeDependentService.addDependent($scope.employeeDependent, {isChangeRequest:false});
            }

            promise
                .then(function(updated) {
                    MessageService.setTemporarySuccessMessage("Your dependent was saved successfully.");
                    $modalInstance.close(angular.extend($scope.employeeDependent, updated));
                })
                .catch(MessageService.showWebApiException);
        };
        
        
        //--------------------------------------------------------
        // checks if the current dependent data on the scope has been modified from its original values.
        //--------------------------------------------------------
        $scope.isDataModified = function () {
            return !angular.equals($scope.employeeDependent, originalDependentData);
        };

        // ------------------------------------------------------------------------------------------
        // Close Modal
        // ------------------------------------------------------------------------------------------
        $scope.close = function () {
            MessageService.clearMessage();
            $modalInstance.dismiss();
        };

        /**
        * Updates the Unmasked SSN variable on the current dependent using the web service.
        *
        * @ngmethod
        * @name EditDependentController#updateSsnMask
        * @returns Angular promise that can be used to handle success and error data created while masking the SSN.
        */
        $scope.updateSsnMask = function () {
            return ssnMask.getMaskedSsn($scope.employeeDependent.unmaskedSocialSecurityNumber)
                .then(function (masked) {
                    $scope.employeeDependent.maskedSocialSecurityNumber = masked;
                });
        };

        $scope.isEditDependentFormSubmitted= function(){
            return $scope.editDependentForm.$submitted;
        }

        EmployeeDependentService.getRelationshipList().then(function (relData) {
            $scope.EmployeeDependentRelationshipsList = relData;
            $scope.selectedRelationship = _.find(relData,
            { employeeDependentsRelationshipId: $scope.employeeDependent.employeeDependentsRelationshipId });
        });

        function relationshipChanged(relationship) {
            if (relationship) {
                $scope.employeeDependent.employeeDependentsRelationshipId = relationship.employeeDependentsRelationshipId;
                $scope.employeeDependent.relationship = relationship.description;
                $scope.employeeDependent.isChild = relationship.isChild;
                $scope.employeeDependent.isSpouse = relationship.isSpouse;
            } else {
                $scope.employeeDependent.employeeDependentsRelationshipId = null;
                $scope.employeeDependent.relationship = "";
                $scope.employeeDependent.isChild = false;
                $scope.employeeDependent.isSpouse = false;
            }
        }
    }
}
