import * as angular from "angular";
import * as util from "../../../../../Scripts/util/ds-common";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsEmployeeContactInfoService } from "@ajs/employee/contact-info.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { DsUtilityServiceProvider } from "@ajs/core/ds-utility/ds-utility.service";
import { IUserInfo } from "@ajs/user";
import { STATES } from "../../shared/state-router-info";
import { CountryAndStateControllerBase } from "../../common/base/country-state.controller";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";

export class EssContactInfoEditController {
    static readonly $inject = [
        '$scope',
        DsMsgService.SERVICE_NAME,
        DsEmployeeContactInfoService.SERVICE_NAME,
        'pageData',
        DsInlineValidatedInputService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        '$controller',
        DsUtilityServiceProvider.SERVICE_NAME,
        STATES.ds.ess.RESOLVE.userAccount
    ];

    constructor($scope: any, MessageService: DsMsgService, 
        EmployeeContactInformationService: DsEmployeeContactInfoService, 
        pageData, DsInlineValidatedInputService: DsInlineValidatedInputService, DsNavigationService: DsNavigationService, 
        $controller, DsUtility: DsUtilityServiceProvider, userAccount: IUserInfo) {

        //------------------------------------------------------------------------
        //initialize variables
        //------------------------------------------------------------------------
        $controller(CountryAndStateControllerBase.CONTROLLER_NAME, { $scope: $scope });
        var originalData = {};
        if (pageData.employeeContactInformationData.driversLicenseIssuingStateId == 0) {
            pageData.employeeContactInformationData.driversLicenseIssuingStateId = pageData.employeeContactInformationData.stateId;
        }
        originalData = angular.copy(pageData.employeeContactInformationData);
        $scope.employeeContactInformationData = angular.copy(pageData.employeeContactInformationData);

        function init() {
            $scope.userData = userAccount;
        }
        init();

        //set country and state controller data, and initialize it
        $scope.countryAndState.initialize({
            countryId: $scope.employeeContactInformationData.countryId,
            stateId: $scope.employeeContactInformationData.stateId,
            countyId: $scope.employeeContactInformationData.countyId,
            countries: pageData.countriesData,
            states: pageData.countryStatesData,
            counties: pageData.countyData
        });
        
        $scope.employeeContactInformationData.driversLicenseExpirationDate = DsUtility.parseDateInput($scope.employeeContactInformationData.driversLicenseExpirationDate, "MM/DD/YYYY");
        $scope.countryAndState.setModifiedSnapshot();
        

        //--------------------------------------------------------
        //todo: refactor: (jay): create a submit button that can take this codes place and call the controllers submit if everything is good 
        //this will validate all dsInlineValidtedInput controls before it goes to the server
        //--------------------------------------------------------
        $scope.validateThenSubmit = function () {
            DsInlineValidatedInputService.validateAll()
            .then(
                function () {
                    $scope.updateContactInformation();
                },
                function () {
                    MessageService.setMessage(
                        window.MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                        MessageTypes.error);
                }
            );
        };

        //--------------------------------------------------------
        //This function is called when the 'Save' (submit) button is clicked.
        //This will push the data to the server via the 
        //EmployeeContactInformation Service
        //--------------------------------------------------------
        $scope.updateContactInformation = function() {
            //console.log('GOING TO THE SERVER');
            MessageService.sending(true);

            //the contry and state controller stores all the country data, so we need to transfer that to the entity before saving.
            $scope.employeeContactInformationData.countryId = $scope.countryAndState.countryId;
            $scope.employeeContactInformationData.stateId = $scope.countryAndState.stateId;
            $scope.employeeContactInformationData.countyId = $scope.countryAndState.countyId;

            //push the data to the server
            EmployeeContactInformationService
                .putEmployeeContactInfo($scope.employeeContactInformationData, true)
                .then(successfulSave)
                .catch(MessageService.showWebApiException);            
        };

        //--------------------------------------------------------
        // this is called if save (or request change) is successful
        //--------------------------------------------------------
        function successfulSave() {
            DsNavigationService.gotoProfileThenShowSuccessMessage();
        }

        //--------------------------------------------------------
        // On cancel redirect to the contact info view page
        // todo: refactor: (jay) this function is duplicated in several controllers
        //--------------------------------------------------------
        $scope.cancelEdit = function () {
            DsNavigationService.gotoProfile();
        };

        //--------------------------------------------------------
        // Indicates if the scope's contact info has changed from
        // the original data.
        //--------------------------------------------------------
        $scope.isDataModified = function () {
            //return !angular.equals($scope.employeeContactInformationData, originalData);
            var result =
                !angular.equals($scope.employeeContactInformationData, originalData) ||
                    $scope.countryAndState.isModified();

            return result;
        };
    }
}
