import * as util from "../../../../../Scripts/util/ds-common";
import * as dateUtil from "../../../../../Scripts/util/dateUtilities";

import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { STATES } from "../../shared/state-router-info";
import { AccountService } from "@ajs/core/account/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import * as benefitSvc from "@ajs/benefits/benefits.service";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { IUserInfo } from "@ajs/user";

export class BenefitConfirmationController {
    static readonly CONTROLLER_NAME = "BenefitConfirmationController";
    static readonly $inject = [
        '$scope',
        DsStateService.SERVICE_NAME,
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsInlineValidatedInputService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        benefitSvc.SERVICE_NAME
    ];
    static readonly CONFIG = {
        template: require('./confirmation.html')
    };

    constructor(
        $scope: any,
        state: DsStateService,
        accountSvc: AccountService,
        MessageService: DsMsgService,
        DsInlineValidatedInputService: DsInlineValidatedInputService,
        DsNavigationService: DsNavigationService,
        BenefitsService) {

        var origUser = {},
            context;

        $scope.user = {};
        $scope.currentUser = {};
        $scope.hasExistingSignedRecord = false;
        $scope.existingSignedRecordDate = '';
        $scope.hasSuccess = false; //When attempting to confirm (update the db)
        $scope.confirmationAcknowledgement = false;

        /** perform controller/scope intialization */
        function init(userAccount) {

            context = BenefitsService.getBenefitContext();

            if (!context.hasSelectedEnrollment()) {
                //state.router.go("ess.benefits.home");
                BenefitsService.redirect("home");
            } else {

                //Check to see if the record is already signed.  If so, lock things down and display message.
                //User will have to start from the beginning.
                if (context.selectedOpenEnrollment.employeeOpenEnrollment.isSigned) {
                    $scope.hasExistingSignedRecord = true;
                    $scope.existingSignedRecordDate = context.selectedOpenEnrollment.employeeOpenEnrollment.dateSigned;
                    $scope.hasSuccess = false;
                } else {
                    $scope.currentUser = userAccount;
                }
            }
        }

        accountSvc.getUserInfo().then(user => init(user));

        /**
        * @ngdoc method
        * @name #checkCurrentPassword
        * @methodOf Ess.Controllers:BasicUserProfileController
        *
        * @description
        * Checks if the current-password is valid for the current user.
        */
        $scope.checkCurrentPassword = function () {
            // see if a current-password was specified
            $scope.password.currentSpecified = $scope.password.current ? true : false;

            // check the current-password by calling the server
            return accountSvc.verifyPassword($scope.currentUser.userId, $scope.currentUser.authUserId, $scope.password.current).then(
                function (data) {

                    $scope.password.currentPasswordValid = data.isValid;
                    return !data.isValid
                        ? ['Current password does not match.']
                        : [];
                },
                function (data) {
                    $scope.password.currentPasswordValid = false;
                    MessageService.showWebApiException(data);
                }
            );
        };


        //--------------------------------------------------------
        //Validates the form to make sure its valid.  Then makes a call
        //to update the database.
        //--------------------------------------------------------
        $scope.validateThenSubmit = function (formName) {

            DsInlineValidatedInputService.validateAll(formName).then(
                function () {

                    $scope.updateEmployeeOpenEnrollment();

                },
                function () {
                    MessageService.setMessage(
                        window.MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                        MessageTypes.error);

                    $scope.hasExistingSignedRecord = false;
                    $scope.hasSuccess = false;
                }
            );
        };

        //--------------------------------------------------------
        //handles the cancel request
        //--------------------------------------------------------
        $scope.cancelEdit = function (key) {
            BenefitsService.redirect("summary");
        };


        $scope.updateEmployeeOpenEnrollment = function () {

            MessageService.sending(true);

            var dto = {
                OpenEnrollmentId: context.selectedOpenEnrollment.openEnrollmentId,
                EmployeeOpenEnrollmentId: !context.selectedOpenEnrollment.employeeOpenEnrollment.employeeOpenEnrollmentId ? 0 : context.selectedOpenEnrollment.employeeOpenEnrollment.employeeOpenEnrollmentId,
                clientId: $scope.currentUser.clientId,
                EmployeeId: $scope.currentUser.employeeId,
                IsOpen: true,
                IsSigned: true,
                DateSigned: window.timeStamp(),
                OpenEnrollmentEmployeeId: !context.selectedOpenEnrollment.employeeId ? 0 : context.selectedOpenEnrollment.employeeId,
                IsConfirmation: true, //[#53810095] - triggers email to CA
                EventDate: !context.selectedOpenEnrollment.eventDate ? "" : context.selectedOpenEnrollment.eventDate,
                EventDescription: !context.selectedOpenEnrollment.description ? "" : context.selectedOpenEnrollment.description
            };

            BenefitsService.putEmployeeOpenEnrollment(dto).then(
                 // success
                function (data, status) {
                    MessageService.setTemporarySuccessMessage("Selection was successful", 5000);
                    BenefitsService.redirect("home");
                    $scope.hasSuccess = true;
                }
            )
            .catch(function(data){
                $scope.hasExistingSignedRecord = false;
                $scope.hasSuccess = false;
                MessageService.showErrorMsg("An error occured while confirming selections.");
            });
        };
    }
}
