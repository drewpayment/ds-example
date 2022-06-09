import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { TaxService } from "@ajs/taxes/tax/tax.service";
import { isUndefinedOrNullOrEmptyString, isUndefinedOrNull } from "@util/ds-common";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingW4Controller {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        TaxService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        '$q'
    ];

    constructor (
        protected $scope: ng.IScope,
        accountService: AccountService,
        MessageService,
        BubbleMessageService,
        DsOnboardingApi,
        taxSvc,
        protected workflowService: WorkflowService,
        $q
    ) {

        function init(userAccount, myWorkflow) {
            var isPageDirty = false;

            //Used to pass workflow object into workflow footer via directive. [my-workflow]
            //The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;

            $scope.circleNumber = myWorkflow.getCircleNumber();
            var currState = myWorkflow.getCurrState();
            $scope.employeeId = userAccount.employeeId;

            $scope.hasError = false;
            $scope.isLoading = true;
            $scope.userData = {};
            $scope.isSoftWarned = false;
            $scope.householdIncome = new Array(4);

            $scope.householdIncome[1] = [{ id: 1, desc: "Less Than $103,351" }, { id: 2, desc: "$103,351 to $345,850" }, { id: 3, desc: "$345,851 to $400,000" }, { id: 3, desc: "Over $400,000" }];
            $scope.householdIncome[2] = [{ id: 1, desc: "Less Than $71,201" }, { id: 2, desc: "$71,201 to $179,050" }, { id: 3, desc: "$179,051 to $200,000" }, { id: 3, desc: "Over $200,000" }];
            $scope.householdIncome[3] = [{ id: 1, desc: "Less Than $71,201" }, { id: 2, desc: "$71,201 to $179,050" }, { id: 3, desc: "$179,051 to $200,000" }, { id: 3, desc: "Over $200,000" }];

            $scope.maritalStatuses = [{ id: 1, desc: "Yes" }, { id: 2, desc: "No" }, { id: 3, desc: "Qualifying Widow(er)" }];

            var isCompleted = isUndefinedOrNullOrEmptyString(myWorkflow.isStateCompleted(currState));
            $scope.isCompleted = isUndefinedOrNullOrEmptyString(isCompleted) || isCompleted === true;

            taxSvc.getFilingStatuses().then(function (statuses) {
                $scope.filingStatuses = statuses.slice(0, 3);
            });

            DsOnboardingApi.getEmployeeW4Assist(userAccount.employeeId)
                .then(function (data) {
                    $scope.employeeW4AssistData = data;
                    if (isUndefinedOrNull($scope.employeeW4AssistData) || $scope.employeeW4AssistData === "null") $scope.employeeW4AssistData = {};
                    $scope.isLoading = false;
                    $scope.hasError = false;

                }).catch(getFailed);

            function getFailed() {
                alert("An error has occurred.");
            }

            function validRequiredFields() {
                return true;
            }

            function calculateTotalExemptions() {
                var totalExemptions = 0;
                if (!isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.isDependentCareClaim) && $scope.employeeW4AssistData.isDependentCareClaim == 0)
                    totalExemptions += 1;

                if (!isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.maritalStatus) && !isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.doYouHaveAnotherJob) && $scope.employeeW4AssistData.maritalStatus == 2 && $scope.employeeW4AssistData.doYouHaveAnotherJob == 0)
                    totalExemptions += 1;
                else if (!isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.isSecondJobAndSpouseEarnMoreThan1500) && $scope.employeeW4AssistData.isSecondJobAndSpouseEarnMoreThan1500==0)
                    totalExemptions += 1;

                if (!isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.maritalStatus) && $scope.employeeW4AssistData.maritalStatus == 1 && !isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.isSpouseEmployed) && $scope.employeeW4AssistData.isSpouseEmployed == 0)
                    totalExemptions += 1;

                if (!isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.dependentCount))
                    totalExemptions += $scope.employeeW4AssistData.dependentCount;

                if (!isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.taxFilingStatus) && $scope.employeeW4AssistData.taxFilingStatus == 3)
                    totalExemptions += 1;

                if (!isUndefinedOrNullOrEmptyString($scope.employeeW4AssistData.isDependentOrChildCareExpensesIsMoreThan2000) && $scope.employeeW4AssistData.isDependentOrChildCareExpensesIsMoreThan2000 == 1)
                    totalExemptions += 1;

                return totalExemptions;
            }

            function savePageData(withoutSoftWarn) {
                var deffered = $q.defer();
                BubbleMessageService.hideBubbleMessage();
                $scope.savingChanges = true;
                $("#ChildCount").removeClass("mandatory-field");
                //Validate required fields.
                if (validRequiredFields()) {

                    if ($scope.employeeW4AssistData.childCount > $scope.employeeW4AssistData.dependentCount) {
                        MessageService.setMessageWithIcon("Invalid child count. Please fill a value that is no more than dependents count.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                        $("#ChildCount").addClass("mandatory-field");
                        deffered.reject();
                        return deffered.promise;
                    }
                    var isWorkflowTaskComplete = !$scope.frm.$error.required;
                    if (isWorkflowTaskComplete || $scope.isSoftWarned || withoutSoftWarn) {
                        MessageService.sending(true);
                        $scope.isSoftWarned = false;
                        if ($scope.employeeW4AssistData.maritalStatus != 1) {
                            $scope.employeeW4AssistData.isSpouseEmployed = null;
                            $scope.employeeW4AssistData.isSpouseBlind = null;
                            $scope.employeeW4AssistData.isSpouseOver65 = null;
                        }
                        if ($scope.employeeW4AssistData.dependentCount == 0) {
                            $scope.employeeW4AssistData.childCount = null;
                            $scope.employeeW4AssistData.isDependentOrChildCareExpensesIsMoreThan2000 = null;
                        }
                        $scope.employeeW4AssistData.childTaxCredit = null;
                        var totalExemptions = calculateTotalExemptions();

                        if ($scope.employeeW4AssistData.childCount > 0)
                        {
                            if ($scope.employeeW4AssistData.householdIncomeStatus == 1) {
                                $scope.employeeW4AssistData.childTaxCredit = $scope.employeeW4AssistData.childCount * 4;
                            }
                            else if ($scope.employeeW4AssistData.householdIncomeStatus == 2)
                                $scope.employeeW4AssistData.childTaxCredit = $scope.employeeW4AssistData.childCount * 2;
                            else if ($scope.employeeW4AssistData.householdIncomeStatus == 3)
                                $scope.employeeW4AssistData.childTaxCredit = $scope.employeeW4AssistData.childCount;
                            else
                                $scope.employeeW4AssistData.childTaxCredit = 0;

                            totalExemptions += $scope.employeeW4AssistData.childTaxCredit;
                        }
                        $scope.employeeW4AssistData.totalExemptions = totalExemptions;
                        DsOnboardingApi.putEmployeeW4Assist($scope.employeeW4AssistData)
                            .then(function (data) {
                                MessageService.sending(false);

                                $scope.frm.$setPristine();
                                //Check for errors.
                                if (data.hasNoError) {

                                    myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
                                    deffered.resolve(isWorkflowTaskComplete);
                                } else {
                                    MessageService.setMessage("An error has occurred.", MessageService.messageTypes.error);
                                    deffered.reject();
                                }
                            });
                    }
                    else {
                        $scope.isSoftWarned = true;
                        BubbleMessageService.showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                        deffered.reject();
                    }

                } else {
                    $scope.isSoftWarned = true;
                    MessageService.setMessageWithIcon("Missing required information. Please fill the required fields.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                    deffered.reject();
                }
                return deffered.promise;
            }

            $scope.savePageData = savePageData;

            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
                //Check if page data is dirty.
                if ($scope.frm.$dirty) {
                    if (validRequiredFields()) {
                        savePageData(true).then(function () {
                        })
                        .catch(function () {
                            event.preventDefault();  //This does
                        });
                    }
                    else {
                        MessageService.sending(false);
                        event.preventDefault();
                    }
                }
            });

            function errorCallback(data) {
                $scope.hasError = true;
                $scope.isLoading = false;

                if (data.errors) {
                    angular.forEach(data.errors, function (error) {
                        // check if user does not have permission to view Taxes
                        if (error.messageType === MessageService.validationMessageTypes.actionNotAllowed) {
                            MessageService.setMessage("You do not have permission to view this page.", MessageService.messageTypes.error);
                        }
                    });
                }
            }

            function clearErrors() {
                $scope.hasError = false;
            }
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
