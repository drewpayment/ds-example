import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsEmployeeDependentService } from "@ajs/employee/dependent.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { valdateUsingRegExp } from "@util/ds-common";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingDependentsController {
    static readonly $inject = [
        "$rootScope",
        "$scope",
        AccountService.SERVICE_NAME,
        "$location",
        DsMsgService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        "$modal",
        DsInlineValidatedInputService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        '$q',
        DsEmployeeDependentService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME
    ];


    constructor ($rootScope, protected $scope: ng.IScope, accountService: AccountService, $location,
        MessageService, DsNavigationService, $modal, DsInlineValidatedInputService,
        DsOnboardingApi, protected workflowService: WorkflowService, $q, EmployeeDependentService, BubbleMessageService
    ) {
        function init(userAccount, myWorkflow) {
            $scope.nav = DsNavigationService;

            $scope.hasError = false;
            $scope.isLoading = true;
            $scope.isLoaded = false;
            $scope.isSoftWarned = false;
            $scope.isHardStop = false;
            $scope.myWorkflow = myWorkflow;
            $scope.ssnDisabled = false;
            $scope.circleNumber = myWorkflow.getCircleNumber();
            var currState = myWorkflow.getCurrState();



            $scope.SSNExp = "^(?!\\b(\\d)1+-(\\d)1+-(\\d)1+\\b)(?!123-45-6789|219-09-9999|078-05-1120)(?!666|000|9\\d{2})\\d{3}-(?!00)\\d{2}-(?!0{4})\\d{4}$";
            $scope.userData = {};
            $scope.gender = [{ value: "", desc: "Select Gender" }, { value: "M", desc: "Male" }, { value: "F", desc: "Female" }];
            // DOB STart
            var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var monthDays = [];
            var monthDaysStatic = [];
            var monthDayCount = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            var years = [];

            var d = new Date();
            var n = d.getFullYear();

            var beginYear = n - 111;
            var endYear = n;


            $scope.monthNames = [];
            var i;
            var x;

            for (i = 1; i < monthNames.length + 1; i++) {
                x = { value: i, desc: monthNames[i - 1] };
                $scope.monthNames.push(x);
            }

            for (i = 1; i <= 31; i++) {
                x = { value: i, desc: i.toString(), show: true, selected: false };
                monthDaysStatic.push(x);
            }

            for (i = endYear; i > beginYear; i--) {
                x = { value: i, desc: i.toString() };
                years.push(x);
            }

            $scope.monthDays = monthDays;
            $scope.years = years;
            var dummyMonthName = { value: 0, desc: "Month" };
            $scope.monthNames.unshift(dummyMonthName);
            var dummyMonthDay = { value: 0, desc: "Day" };
            $scope.monthDays.unshift(dummyMonthDay);
            var dummyYears = { value: 0, desc: "Year" };
            $scope.years.unshift(dummyYears);

            // DOB End
            $scope.isAdding = false;
            $scope.isEditing = false;
            //$scope.selected.gender = "";
            $scope.flagNoDependent = false;
            $scope.isConfirmSSNValid=true;
            var isCompleted = myWorkflow.isStateCompleted(currState);

            $scope.hideSoftwarn = !isCompleted || isCompleted === true;
            $scope.isCompleted = isCompleted;

            $scope.validateSSN = function validateSSN() {
                $scope.validSSN = valdateUsingRegExp($scope.SSNExp, $scope.selected.unmaskedSocialSecurityNumber);
            }

            $scope.changeMonthDays = function () {
                var month;
                if ($scope.selected.DOBmonth != 0) {
                    month = $scope.selected.DOBmonth;
                }
                var endDay = monthDayCount[month - 1];

                if (month === 2) {
                    if (IsLeapYear($scope.dobYear)) {
                        endDay = 29;
                    }
                }

                $scope.monthDays = monthDaysStatic.slice(0, endDay);
                var dummyMonthDay = { value: 0, desc: "Day" };
                $scope.monthDays.unshift(dummyMonthDay);
            }

            function initUserData() {
                $scope.userData = userAccount;
                $scope.selected = {};
                EmployeeDependentService.getDependents(userAccount.employeeId).then(function (dependentsData) {
                    $scope.EmployeeOnboardingDependentsList = dependentsData;
                    if ($scope.EmployeeOnboardingDependentsList.length === 0 && $scope.isCompleted) {
                        $scope.flagNoDependent = true;
                    }
                    else
                    {
                        $scope.flagNoDependent = false;
                    }

                });

                DsOnboardingApi.getEmployeeOnboardingField(userAccount.employeeId, 3)
                        .then(function (employeeOnboardingField) {
                            $scope.employeeOnboardingField = employeeOnboardingField;
                            if ($scope.employeeOnboardingField.isBoolValue == null)
                                $scope.flagNoDependent = false;
                            else
                                $scope.flagNoDependent = $scope.employeeOnboardingField.isBoolValue;
                        });

                EmployeeDependentService.getRelationshipList().then(function (relData) {
                    $scope.EmployeeDependentRelationshipsList = relData;
                });

                $scope.selected.DOBday = 0;
                $scope.selected.DOBmonth = 0;
                $scope.selected.DOByear = 0;
                $scope.selected.gender = "";
                $scope.selected.relationship = " ";
                $scope.changeMonthDays();
                $scope.selected.isAStudent = false;
                $scope.selected.hasADisability = false;
                $scope.selected.tobaccoUser = false;
                $scope.isLoading = false;
                $scope.isLoaded = true;
            }
            initUserData();

            function IsLeapYear(year) {
                if (parseInt(year) % 4 === 0 && (parseInt(year) % 100 !== 0 || parseInt(year) % 400 !== 0)) {
                    return true;
                }

                return false;
            }

            function unPackDOB() {
                var dateOfBirth = $scope.selected.birthDate;
                if (dateOfBirth) {
                    $scope.selected.DOBday = Number(dateOfBirth.substring(8, 10));
                    $scope.selected.DOBmonth = Number(dateOfBirth.substring(5, 7));
                    $scope.selected.DOByear = Number(dateOfBirth.substring(0, 4));
                } else {
                    $scope.selected.DOBday = 0;
                    $scope.selected.DOBmonth = 0;
                    $scope.selected.DOByear = 0;
                }
            }

            function packDOB() {
                var dobM;
                var dobD;
                if ($scope.selected.DOBmonth.toString().length == 1) {
                    dobM = "0" + $scope.selected.DOBmonth.toString();
                }
                else {
                    dobM = $scope.selected.DOBmonth.toString();
                }
                if ($scope.selected.DOBday.toString().length == 1) {
                    dobD = "0" + $scope.selected.DOBday.toString();
                }
                else {
                    dobD = $scope.selected.DOBday.toString();
                }
                var dobString = $scope.selected.DOByear.toString() + "-" + dobM + "-" + dobD;

                return parseISO8601(dobString);
            }
            function parseISO8601(dateStringInRange) {
                var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s*$/,
                    date = new Date(NaN), month,
                    parts = isoExp.exec(dateStringInRange);

                if (parts) {
                    month = +parts[2];
                    date.setFullYear(+parts[1], month - 1, +parts[3]);
                    if (month != date.getMonth() + 1) {
                        date.setTime(NaN);
                    }
                }
                return date;
            }


            $scope.isEdit = function (contact) {
                if (contact.employeeDependentId === $scope.selected.employeeDependentId) {
                    return true;
                }

                return false;
            }

            $scope.editContact = function (contact) {

                $scope.selected = angular.copy(contact);
                if (!$scope.selected.unmaskedSocialSecurityNumber) {
                    $scope.ssnDisabled = true;
                }
                else {
                    $scope.ssnDisabled = false;
                    $scope.selected.unmaskedConfirmSocialSecurityNumber = $scope.selected.unmaskedSocialSecurityNumber;
                }
                $scope.isEditing = true;
                $scope.isAdding = false;
                unPackDOB();
                $scope.changeMonthDays();
            };

            $scope.addContact = function () {
                $scope.selected = {};
                $scope.selected.DOBmonth = 0;
                $scope.selected.DOBday = 0;
                $scope.selected.DOByear = 0;
                $scope.selected.isAStudent = false;
                $scope.selected.hasADisability = false;
                $scope.selected.tobaccoUser = false;
                $scope.isAdding = true;
                $scope.isEditing = false;
                $scope.selected.gender = "";
                $scope.flagNoDependent = false;
                $scope.ssnDisabled = false;
            };
            $scope.deleteContact = function () {

                EmployeeDependentService.deleteDependent($scope.selected).then(function (resData) {
                    var indexToRemove;
                    for (i = 0; i < $scope.EmployeeOnboardingDependentsList.length; i++) {
                        var contact = $scope.EmployeeOnboardingDependentsList[i];
                        if (contact.employeeDependentId === $scope.selected.employeeDependentId) {

                            indexToRemove = i;
                            break;

                        }
                    }
                    $scope.EmployeeOnboardingDependentsList.splice(indexToRemove, 1);
                    $scope.selected = {};
                    $scope.isAdding = false;
                    $scope.isEditing = false;
                    $scope.flagNoDependent = true;
                });

            };

            $scope.cancel = function () {
                $scope.isAdding = false;
                $scope.isEditing = false;
                $scope.selected = {};
            };

            $scope.changeSSN = function changeSSN() {
                if (!$scope.selected.unmaskedSocialSecurityNumber || $scope.selected.unmaskedSocialSecurityNumber.length == 11) {
                    $("#EditSSN").removeClass("mandatory-field");
                    $("#AddSSN").removeClass("mandatory-field");
                    $("#EditConfirmSSN").removeClass("mandatory-field");
                    $("#AddConfirmSSN").removeClass("mandatory-field");
                }
            }

            $scope.changeSSNDisables = function changeSSNDisables($event) {
                var checkbox = $event.target;
                if (checkbox.checked)
                {
                    $scope.selected.unmaskedSocialSecurityNumber = '';
                    $scope.ssnDisabled = true;
                }
                else
                {
                    $scope.ssnDisabled = false;
                }
            }

            $scope.addOrUpdateNewContact = function () {
                $("#EditSSN").removeClass("mandatory-field");
                $("#AddSSN").removeClass("mandatory-field");
                $("#EditConfirmSSN").removeClass("mandatory-field");
                $("#AddConfirmSSN").removeClass("mandatory-field");
                $scope.validateSSN();
                MessageService.clearMessage();
                if ($scope.isAdding === false) {
                    if ($scope.frmDependent.EditFirstName.$valid && $scope.frmDependent.EditLastName.$valid && $scope.frmDependent.EditRelationship.$valid) {
                        if (($scope.ssnDisabled && !$scope.selected.unmaskedSocialSecurityNumber) || ($scope.validSSN && $scope.selected.unmaskedSocialSecurityNumber==$scope.selected.unmaskedConfirmSocialSecurityNumber)) { //$scope.selected.unmaskedSocialSecurityNumber.length == 11) {
                            //console.log('update')
                            //do the work
                            $scope.selected.employeeId = $scope.userData.employeeId;
                            $scope.selected.birthDate = packDOB();

                            EmployeeDependentService.updateDependent($scope.selected, {isChangeRequest:false}).then(function (resData) {
                                myWorkflow.updateWorkflowTask(currState, true);
                                for (i = 0; i < $scope.EmployeeOnboardingDependentsList.length; i++) {
                                    var contact = $scope.EmployeeOnboardingDependentsList[i];
                                    if (contact.employeeDependentId === resData.employeeDependentId) {
                                        //console.log("i: " + i);
                                        $scope.EmployeeOnboardingDependentsList[i] = $scope.selected;
                                        break;
                                    }
                                }
                                $scope.selected = {};
                                $scope.isAdding = false;
                                $scope.isEditing = false;
                                $scope.flagNoDependent = true;
                            });
                        }
                        else {
                            if($scope.validSSN && $scope.selected.unmaskedSocialSecurityNumber!=$scope.selected.unmaskedConfirmSocialSecurityNumber)
                            {
                                $("#EditConfirmSSN").addClass("mandatory-field");
                                MessageService.setMessageWithIcon("Social Security Numbers do not match.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                            }
                            else{
                                $("#EditSSN").addClass("mandatory-field");
                                MessageService.setMessageWithIcon("Please use the correct format for Social Security Number ( XXX-XX-XXXX )", MessageService.messageTypes.error, MessageService.iconTypes.error);
                            }
                        }
                    }
                    else {
                        $scope.isHardStop = true;
                        MessageService.setMessageWithIcon("Missing required information. Please fill the required fields.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                    }
                }
                else {
                    if ($scope.frmDependent.AddFirstName.$valid && $scope.frmDependent.AddLastName.$valid && $scope.frmDependent.AddRelationship.$valid) {
                        if (($scope.ssnDisabled && !$scope.selected.unmaskedSocialSecurityNumber) || ($scope.validSSN && $scope.selected.unmaskedSocialSecurityNumber==$scope.selected.unmaskedConfirmSocialSecurityNumber)) { //$scope.selected.unmaskedSocialSecurityNumber.length == 11) {
                            //console.log('Add')

                            $scope.selected.employeeId = $scope.userData.employeeId;
                            $scope.selected.birthDate = packDOB();
                            EmployeeDependentService.addDependent($scope.selected,{isChangeRequest:false}).then(function (resData) {
                                myWorkflow.updateWorkflowTask(currState, true);
                                $scope.flagNoDependent = true;

                                $scope.EmployeeOnboardingDependentsList[$scope.EmployeeOnboardingDependentsList.length] = resData;
                                $scope.isAdding = false;
                                $scope.isEditing = false;
                                $scope.selected = {};
                                $scope.isHardStop = false;

                                if ($scope.flagNoDependent) {
                                    $scope.employeeOnboardingField.isBoolValue = false;
                                    DsOnboardingApi.updateEmployeeOnboardingField($scope.employeeOnboardingField).then(function (reEmployeeOnboardingField) {

                                    });
                                }
                            });
                        }
                        else {
                            if($scope.validSSN && $scope.selected.unmaskedSocialSecurityNumber!=$scope.selected.unmaskedConfirmSocialSecurityNumber)
                            {
                                $("#AddConfirmSSN").addClass("mandatory-field");
                                MessageService.setMessageWithIcon("Social Security Numbers do not match.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                            }
                            else{
                                $("#AddSSN").addClass("mandatory-field");
                                MessageService.setMessageWithIcon("Please use the correct format for Social Security Number ( XXX-XX-XXXX )", MessageService.messageTypes.error, MessageService.iconTypes.error);
                            }
                        }
                    }
                    else {
                        $scope.isHardStop = true;
                        MessageService.setMessageWithIcon("Missing required information. Please fill the required fields.", MessageService.messageTypes.error, MessageService.iconTypes.error);

                    }
                }
            }

            $scope.setFlagNoDependent = function () {
                if ($scope.flagNoDependent === false) {
                    $scope.flagNoDependent = true;

                    $scope.isAdding = false;
                }
                else if ($scope.flagNoDependent === true) {
                    $scope.flagNoDependent = false;

                }
            }

            function savePageData(withoutSoftWarn) {
                MessageService.clearMessage();
                BubbleMessageService.hideBubbleMessage();
                var deffered = $q.defer();

                if ($scope.EmployeeOnboardingDependentsList.length == 0 && $scope.flagNoDependent) {
                    $scope.employeeOnboardingField.isBoolValue = $scope.flagNoDependent;
                    DsOnboardingApi.updateEmployeeOnboardingField($scope.employeeOnboardingField).then(function (reEmployeeOnboardingField) {
                        $scope.savedChanges = true;
                        myWorkflow.updateWorkflowTask(currState, true);
                        deffered.resolve(true);
                    });
                }
                else if ($scope.EmployeeOnboardingDependentsList.length == 0 && !$scope.flagNoDependent && !withoutSoftWarn && !$scope.isSoftWarned)
                {
                    $scope.isSoftWarned = true;
                    $("#AddFirstName").removeClass("mandatory-field");
                    $("#AddFirstName").addClass("required-field");
                    $("#AddLastName").removeClass("mandatory-field");
                    $("#AddLastName").addClass("required-field");
                    $("#AddRelationship").removeClass("mandatory-field");
                    $("#AddRelationship").addClass("required-field");
                    BubbleMessageService.showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                    deffered.reject();
                }
                else if ($scope.EmployeeOnboardingDependentsList.length == 0 && !$scope.flagNoDependent && (withoutSoftWarn || $scope.isSoftWarned))
                {
                    $scope.isSoftWarned = false;
                    $scope.employeeOnboardingField.isBoolValue = $scope.flagNoDependent;
                    DsOnboardingApi.updateEmployeeOnboardingField($scope.employeeOnboardingField).then(function (reEmployeeOnboardingField) {
                        $scope.savedChanges = true;
                        myWorkflow.updateWorkflowTask(currState, false);
                        deffered.resolve($scope.EmployeeOnboardingDependentsList.length);
                    });
                }
                else {
                    deffered.resolve(true);
                }
                return deffered.promise;
            }

            $scope.savePageData = savePageData;

            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                //Check if page data is dirty.
                if ($scope.savedChanges !== true) {
                    savePageData(true).then(function () {
                    }).catch(function () {
                        event.preventDefault();  //This does
                    });
                }
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
