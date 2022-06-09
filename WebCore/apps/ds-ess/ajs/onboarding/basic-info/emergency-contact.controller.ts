import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { DsEmployeeEmergencyContactService } from "@ajs/employee/emergency-contact.service";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingEmergencyContactController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        DsEmployeeEmergencyContactService.SERVICE_NAME,
        '$q'
    ];

    constructor (protected $scope: ng.IScope,
        accountService: AccountService,
        msg,
        BubbleMessageService,
        protected workflowService: WorkflowService,
        contactService,
        $q
    ) {

        function init(userAccount, myWorkflow) {
            $scope.hasError = false;
            $scope.isLoading = true;
            $scope.isLoaded = false;
            $scope.isAdding = false;
            $scope.isSoftWarned = false;
            $scope.isHardStop = false;
            //Used to pass workflow object into workflow footer via directive. [my-workflow]
            //The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;

            $scope.circleNumber = myWorkflow.getCircleNumber();
            var currState = myWorkflow.getCurrState();

            $scope.isCompletedStatus = myWorkflow.isStateCompleted(currState);
            var isCompleted = !myWorkflow.isStateCompleted(currState);
            $scope.isCompleted = isCompleted || isCompleted === true;
            $scope.userData = {};

            function initUserData() {
                $scope.userData = userAccount;
                $scope.selected = {};

                contactService.getEmergencyContacts(userAccount.employeeId).then(function (emegencycontactData) {
                    $scope.EmployeeOnboardingEmergencyContactList = emegencycontactData;
                    if ($scope.EmployeeOnboardingEmergencyContactList.length === 0) {
                        $scope.isAdding = true;
                        //if ($scope.isCompletedStatus !== null) {
                        //    $scope.turnYellow = true;
                        //} else {
                        //    $scope.turnYellow = false;
                        //}

                    }

                });

                contactService.getRelationshipList().then(function (relData) {
                    $scope.EmergencyContactRelationshipList = relData;
                });

                $scope.isLoading = false;
                $scope.isLoaded = true;

            }
            initUserData();

            $scope.isEdit = function (contact) {
                if (contact.employeeEmergencyContactId === $scope.selected.employeeEmergencyContactId) {
                    return true;
                }

                return false;
            }

            $scope.addContact = function () {
                $scope.selected = {};
                $scope.isAdding = true;
            }

            $scope.editContact = function (contact) {
                $scope.selected = angular.copy(contact);
                $scope.isAdding = false;
            };

            $scope.deleteContact = function () {
                if ($scope.EmployeeOnboardingEmergencyContactList.length === 1) {
                    // alert('You must have at least 1 emergency contact. This contact cannot be removed unless another contact is added.');

                    msg.setTemporaryMessage('You must have at least 1 emergency contact. This contact cannot be removed unless another contact is added.', "alert-danger", 6000);
                }
                else {

                    contactService.deleteEmergencyContact($scope.selected).then(function (resData) {
                        var indexToRemove;
                        for (let i = 0; i < $scope.EmployeeOnboardingEmergencyContactList.length; i++) {
                            var contact = $scope.EmployeeOnboardingEmergencyContactList[i];
                            if (contact.employeeEmergencyContactId === $scope.selected.employeeEmergencyContactId) {

                                indexToRemove = i;
                                break;

                            }
                        }
                        $scope.EmployeeOnboardingEmergencyContactList.splice(indexToRemove, 1);

                        $scope.selected = {};

                    });
                }




            };

            $scope.cancel = function () {
                msg.clearMessage();
                $scope.isAdding = false;
                $scope.selected = {};
            };


            $scope.addOrUpdateNewContact = function () {
                msg.clearMessage();
                //$scope.turnYellow = false;
                BubbleMessageService.hideBubbleMessage();
                // var isWorkflowTaskComplete = !$scope.frmEmergencyContact.$error.required;
                if ($scope.isAdding === false) {
                    if ($scope.frmEmergencyContact.EditFirstName.$valid && $scope.frmEmergencyContact.EditLastName.$valid && $scope.frmEmergencyContact.EditPhoneNumber.$valid && $scope.selected.homePhoneNumber.length == 12) {
                        if ($scope.frmEmergencyContact.EditEmail.$valid) {
                            $scope.selected.employeeId = $scope.userData.employeeId;
                            contactService.updateEmergencyContact($scope.selected, {isChangeRequest:false}).then(function (resData) {
                                myWorkflow.updateWorkflowTask(myWorkflow.getCurrState(), true);


                                for (let i = 0; i < $scope.EmployeeOnboardingEmergencyContactList.length; i++) {
                                    var contact = $scope.EmployeeOnboardingEmergencyContactList[i];
                                    if (contact.employeeEmergencyContactId === resData.employeeEmergencyContactId) {

                                        $scope.EmployeeOnboardingEmergencyContactList[i] = $scope.selected;
                                        break;
                                    }
                                }
                                $scope.selected = {};
                            });
                        }
                        else {
                            //$scope.isSoftWarned = true;
                            msg.setMessageWithIcon("Incorrect email format. Please enter a valid email address.", msg.messageTypes.error, msg.iconTypes.error);
                        }
                    }
                    else {
                        //$scope.isSoftWarned = true;
                        msg.setMessageWithIcon("Missing required information. Please fill the required fields.", msg.messageTypes.error, msg.iconTypes.error);

                    }
                }
                else {
                    if ($scope.frmEmergencyContact.AddFirstName.$valid && $scope.frmEmergencyContact.AddLastName.$valid && $scope.frmEmergencyContact.AddPhoneNumber.$valid && $scope.frmEmergencyContact.AddRelationship.$valid  && $scope.selected.homePhoneNumber.length == 12) {
                        if ($scope.frmEmergencyContact.AddEmail.$valid) {
                            $scope.selected.employeeId = $scope.userData.employeeId;
                            contactService.addEmergencyContact($scope.selected, {isChangeRequest:false}).then(function (resData) {
                                myWorkflow.updateWorkflowTask(myWorkflow.getCurrState(), true);
                                $scope.isAdding = false;
                                $scope.EmployeeOnboardingEmergencyContactList[$scope.EmployeeOnboardingEmergencyContactList.length] = resData;
                                $scope.selected = {};
                            });
                        }
                        else {
                            $scope.isHardStop = true;
                            msg.setMessageWithIcon("Incorrect email format. Please enter a valid email address.", msg.messageTypes.error, msg.iconTypes.error);
                        }
                    }
                    else {
                        $scope.isHardStop = true;
                        msg.setMessageWithIcon("Missing required information. Please fill the required fields.", msg.messageTypes.error, msg.iconTypes.error);
                    }
                }

            };

            function savePageData(withoutSoftWarn) {
                var deffered = $q.defer();
                //deffered.resolve();
                msg.clearMessage();
                BubbleMessageService.hideBubbleMessage();

                if ($scope.EmployeeOnboardingEmergencyContactList.length == 0 && !$scope.isSoftWarned && !withoutSoftWarn) {
                    $scope.isSoftWarned = true;
                    $scope.isHardStop = false;
                    //TODO this should be controlled with a variable and not jquery. it is overriding angular changes. please change on next edit.
                    $("#AddFirstName").removeClass("mandatory-field");
                    $("#AddFirstName").addClass("required-field");
                    $("#AddLastName").removeClass("mandatory-field");
                    $("#AddLastName").addClass("required-field");
                    $("#AddRelationship").removeClass("mandatory-field");
                    $("#AddRelationship").addClass("required-field");
                    $("#AddPhoneNumber").removeClass("mandatory-field");
                    $("#AddPhoneNumber").addClass("required-field");
                    BubbleMessageService.showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                    deffered.reject();
                }
                else {
                    $scope.isSoftWarned = false;
                    if ($scope.EmployeeOnboardingEmergencyContactList.length == 0 && withoutSoftWarn) {
                        myWorkflow.updateWorkflowTask(currState, false);
                        deffered.resolve($scope.EmployeeOnboardingEmergencyContactList.length);
                    }
                    else {
                        deffered.resolve(true);
                    }
                }
                return deffered.promise;
            }

            $scope.savePageData = savePageData;

            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                //Check if page data is dirty.

                savePageData(true).then(function () {
                }).catch(function () {
                    event.preventDefault();  //This does
                });
            });
        }

        accountService.getUserInfo().then(user => {
            workflowService.getUserWorkflowPromise().then(workflow => {
                init(user, workflow);
            });
        });
    }

    clickSavePageData(direction: number) {
        this.$scope.savePageData(false).then(isWorkflowTaskComplete => {
            const currState = this.workflowService.getCurrState();

            if (isWorkflowTaskComplete) {
                this.workflowService.updateWorkflowTask(currState, isWorkflowTaskComplete);
            }

            this.workflowService.getNextPrevPage(currState, direction);
        });
    }
}
