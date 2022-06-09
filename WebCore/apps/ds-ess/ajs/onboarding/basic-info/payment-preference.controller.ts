import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsEmployeeDirectDepositService } from "@ajs/employee/direct-deposit.service";
import { isUndefinedOrNullOrEmptyString } from "@util/ds-common";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';
import { TaxFrequencyService } from '@ds/payroll/shared/tax-frequency.service';

export class OnboardingPaymentPreferenceController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        '$q',
        DsEmployeeDirectDepositService.SERVICE_NAME
    ];

    constructor (
        protected $scope: ng.IScope,
        accountService: AccountService,
        MessageService,
        BubbleMessageService,
        DsOnboardingApi,
        protected workflowService: WorkflowService,
        $q,
        DirectDepositService
    ) {

        function init(userAccount, myWorkflow) {
            $scope.hasError = false;
            $scope.isLoading = true;
            $scope.isLoaded = false;
            $scope.isSoftWarned = false;
            var isPageDirty = false;

            $scope.prefStatusId = 0;
            $scope.defaultPaymentOptionForClient = 0;
            $scope.directDepositData = [];
            $scope.employeeId = userAccount.employeeId;

            $scope.valuePaystubOption = "";
            $scope.allowPaystubEmails = false;

            $scope.isWorkflowTaskComplete = true;
            $scope.myWorkflow = myWorkflow;
            $scope.circleNumber = myWorkflow.getCircleNumber();
            $scope.ClientEssOptions = {};
            var currState = myWorkflow.getCurrState();
            var isCompleted = isUndefinedOrNullOrEmptyString(myWorkflow.isStateCompleted(currState))
            $scope.isCompleted = isUndefinedOrNullOrEmptyString(isCompleted) || isCompleted === true;

            $scope.showHardStop = false;
            $scope.invalidOption = false;
            $scope.invalidPayOption = false;
            $scope.isRoutingValid = true;
            $scope.isAccountValid = true;
            $scope.isConfirmRoutingValid=true;
            $scope.isConfirmAccountValid=true;
            $scope.isSecRoutingValid = true;
            $scope.isSecAccountValid = true;
            $scope.isConfirmSecRoutingValid=true;
            $scope.isConfirmSecAccountValid=true;

            $scope.setPrefStatus = function setPrefStatus(status) {
                if($scope.prefStatusId != status)
                    $scope.frmPreference.$dirty = true
                $scope.prefStatusId = status;

                BubbleMessageService.hideBubbleMessage();
            }

            clearErrors();


            DsOnboardingApi.getClientEssOptions(userAccount.clientId)
                .then(function (essOptions) {
                    if (isUndefinedOrNullOrEmptyString(essOptions.directDepositLimit))
                        essOptions.directDepositLimit = 98;

                    if (!isUndefinedOrNullOrEmptyString(essOptions.allowPaystubEmails))
                        $scope.allowPaystubEmails = essOptions.allowPaystubEmails;

                    $scope.ClientEssOptions = essOptions;

                    if ($scope.ClientEssOptions.allowCheck && !$scope.ClientEssOptions.allowDirectDeposit && !$scope.ClientEssOptions.allowPaycard) {
                        $scope.defaultPaymentOptionForClient = 1;
                    }

                    if (!$scope.ClientEssOptions.allowCheck && $scope.ClientEssOptions.allowDirectDeposit && !$scope.ClientEssOptions.allowPaycard) {
                        $scope.defaultPaymentOptionForClient = 2;
                    }

                    if (!$scope.ClientEssOptions.allowCheck && !$scope.ClientEssOptions.allowDirectDeposit && $scope.ClientEssOptions.allowPaycard) {
                        $scope.defaultPaymentOptionForClient = 3;
                    }

                    if ($scope.defaultPaymentOptionForClient != 0) {
                        $scope.setPrefStatus($scope.defaultPaymentOptionForClient);
                    }

                    DsOnboardingApi.getEmployeeHireDate(userAccount.employeeId).then(function (emp) {
                        $scope.payStubOption = null;
                        if (!isUndefinedOrNullOrEmptyString(emp.data.payStubOption)) {
                            if (emp.data.payStubOption == 1)
                                $scope.payStubOption = true;
                            else
                                $scope.payStubOption = false;
                        }
                    });

                    DsOnboardingApi.getEmployeeOnboardingField(userAccount.employeeId, 1)
                        .then(function (employeeOnboardingField) {
                            if (employeeOnboardingField.decimalValue) {
                                $scope.prefStatusId = employeeOnboardingField.decimalValue;
                            }
                            $scope.employeeOnboardingField = employeeOnboardingField;
                            DsOnboardingApi.getEmployeeDirectDeposit(userAccount.employeeId)
                                .then(function (directDepositData) {
                                    $scope.directDepositData = directDepositData;

                                    if ($scope.directDepositData == null || $scope.directDepositData.length == 0) {
                                        $scope.directDepositData = [];
                                        $scope.directDepositData.push({ routingNumber: '', accountNumber: '',confirmRoutingNumber: '', confirmAccountNumber: '', amount: '', accountType: 1 });
                                    }
                                    else{
                                        $scope.directDepositData = _.sortBy( $scope.directDepositData, ['amountType','sortOrderIndex']).reverse();
                                        _.forEach($scope.directDepositData, (item, i) => {
                                            if(isValidMask(item.maskedRoutingNumber)) {
                                                item.routingNumber = item.maskedRoutingNumber;
                                                item.confirmRoutingNumber = item.maskedRoutingNumber;
                                            }
                                            else item.maskedRoutingNumber = "";

                                        if(isValidMask(item.maskedAccountNumber)) {
                                            item.accountNumber = item.maskedAccountNumber;
                                            item.confirmAccountNumber = item.maskedAccountNumber;
                                        }
                                        else item.maskedAccountNumber = "";
                                        //item.accountNumber = item.maskedAccountNumber;

                                            if(item.routingNumber && item.accountNumber)  item.disabled = true;
                                        });
                                    }

                                    $scope.isLoading = false;
                                    $scope.isLoaded = true;
                                    $scope.hasError = false;

                                });
                        });
                });

            $scope.addSecondaryAccount = function () {
                $scope.directDepositData.push({ routingNumber: '', accountNumber: '', confirmRoutingNumber: '', confirmAccountNumber: '', amount: '', accountType: 1 });
                $scope.showHardStop = false;
            }

            function isValidMask(maskedNumber:string) {
                if(!!maskedNumber)
                    return ( maskedNumber.replace(/\*+/g, '') != '' );
                else return false;
            }

            $scope.removeSecondaryAccount = function (account) {
                var index = $scope.directDepositData.indexOf(account);

                if (index > -1) {
                    $scope.directDepositData.splice(index, 1);
                    $scope.frmPreference.$setDirty();
                }
            }
            $scope.enableDepositEdit = function(account){
                DsOnboardingApi.getDirectDepositAccountNumber(account.employeeDeductionId).then(function (accInfo) {
                    account.accountNumber = accInfo.key;
                    account.routingNumber = accInfo.value;
                    account.confirmAccountNumber = accInfo.key;
                    account.confirmRoutingNumber = accInfo.value;
                    account.disabled = false;
                }).catch(function () {
                    BubbleMessageService.setMessageWithIcon("Unabled to retrieve account number.",
                        MessageService.messageTypes.error, MessageService.iconTypes.error);
                    account.disabled = false;
                });
            }

            function checkAllRoutingNumbers () {
                for (var i = 0; i < $scope.directDepositData.length; i++) {
                    if ($scope.directDepositData[i].routingNumber.length > 0) {
                        if (DirectDepositService.validateRoutingNumber($scope.directDepositData[i].routingNumber) == false) {
                            return false;
                        }
                    }
                }

                return true;
            }

            $scope.checkRoutingNumber = function (routingNumber) {
                return DirectDepositService.validateRoutingNumber(routingNumber);
            }

            function getFailed(errorType) {
                $scope.hasError = true;
                $scope.isLoaded = false;
                if (errorType == 1)
                    alert("An error has loading client options.");
                else if (errorType == 2)
                    alert("An error has occurred loading your preferences.");
            }


            function validateRequiredFields() {
                if ($scope.prefStatusId == 2) {
                    if (!$scope.directDepositData[0].disabled &&
                        $scope.directDepositData[0].routingNumber && !$scope.checkRoutingNumber($scope.directDepositData[0].routingNumber)) {
                        MessageService.setMessageWithIcon("Invalid routing number. Please enter a valid routing number.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                        $scope.showHardStop = true;
                        return false;
                    }
                    if (!$scope.directDepositData[0].disabled &&
                        $scope.directDepositData[0].routingNumber && $scope.directDepositData[0].routingNumber!=$scope.directDepositData[0].confirmRoutingNumber) {
                        MessageService.setMessageWithIcon("Routing Numbers do not match.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                        $scope.showHardStop = true;
                        $scope.isConfirmRoutingValid=false;
                        return false;
                    }
                    if (!$scope.directDepositData[0].disabled &&
                        $scope.directDepositData[0].routingNumber && !$scope.directDepositData[0].accountNumber) {
                        MessageService.setMessageWithIcon("Please enter a valid account number.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                        $scope.showHardStop = true;
                        return false;
                    }
                    if (!$scope.directDepositData[0].disabled &&
                        $scope.directDepositData[0].routingNumber && $scope.directDepositData[0].accountNumber!=$scope.directDepositData[0].confirmAccountNumber) {
                        MessageService.setMessageWithIcon("Account Numbers do not match.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                        $scope.showHardStop = true;
                        $scope.isConfirmAccountValid=false;
                        return false;
                    }
                    if ($scope.directDepositData.length > 1) {
                        for (var i = 1; i < $scope.directDepositData.length; i++) {
                            if (!$scope.directDepositData[i].disabled &&
                                !$scope.directDepositData[i].routingNumber || !$scope.directDepositData[i].accountNumber || !$scope.directDepositData[i].amount) {
                                MessageService.setMessageWithIcon("Please enter a valid routing number, account number and amount.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                                $scope.showHardStop = true;
                                return false;
                            }
                            else if (!$scope.directDepositData[i].disabled &&
                                $scope.directDepositData[i].routingNumber && !$scope.checkRoutingNumber($scope.directDepositData[i].routingNumber)) {
                                MessageService.setMessageWithIcon("Invalid routing number. Please enter a valid routing number.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                                $scope.showHardStop = true;
                                return false;
                            }
                            else if(!$scope.directDepositData[i].disabled && $scope.directDepositData[i].routingNumber != $scope.directDepositData[i].confirmRoutingNumber){
                                MessageService.setMessageWithIcon("Routing Numbers do not match.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                                $scope.showHardStop = true;
                                $scope.isConfirmSecRoutingValid=false;
                                return false;
                            }
                            else if(!$scope.directDepositData[i].disabled && $scope.directDepositData[i].accountNumber != $scope.directDepositData[i].confirmAccountNumber){
                                MessageService.setMessageWithIcon("Account Numbers do not match.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                                $scope.showHardStop = true;
                                $scope.isConfirmSecAccountValid=false;
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

            function setIsWorkflowCompleted() {
                var k = true;

                if ($scope.prefStatusId == 0) {
                    k = false;
                }

                if ($scope.allowPaystubEmails && isUndefinedOrNullOrEmptyString($scope.payStubOption)) {
                    k = false;
                }

                if ($scope.prefStatusId == 2 && (!$scope.directDepositData[0].accountNumber || !$scope.directDepositData[0].routingNumber)) {
                    k = false;
                }

                $scope.checkWorkfowItemCompletionStatus( k );
            }

            $scope.checkWorkfowItemCompletionStatus = function(status:boolean){
                $scope.isWorkflowTaskComplete = status;
                $scope.myWorkflow.updateWorkflowTask(currState, status);
            }

            function savePageData(withoutSoftWarn) {

                BubbleMessageService.hideBubbleMessage();
                var deffered = $q.defer();

                $scope.employeeOnboardingField.DecimalValue = $scope.prefStatusId;

                if ($scope.frmPreference.$dirty && !validateRequiredFields()) {
                    deffered.reject();
                    return deffered.promise;
                }

                if (!withoutSoftWarn) {
                    if (!$scope.isSoftWarned && $scope.prefStatusId == 0) {
                        $scope.isSoftWarned = true;
                        $scope.invalidPayOption = true;
                        $scope.checkWorkfowItemCompletionStatus(false);
                        BubbleMessageService
                            .showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                        deffered.reject();
                        return deffered.promise;
                    }
                }

                if (!withoutSoftWarn && $scope.allowPaystubEmails) {
                    if (!$scope.isSoftWarned && isUndefinedOrNullOrEmptyString($scope.payStubOption)) {
                        $scope.isSoftWarned = true;
                        $scope.invalidOption = true;
                        $scope.checkWorkfowItemCompletionStatus(false);
                        BubbleMessageService
                            .showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                        deffered.reject();
                        return deffered.promise;
                    }
                }

                if (!withoutSoftWarn && $scope.prefStatusId == 2) {
                    if (!$scope.isSoftWarned && !$scope.directDepositData[0].accountNumber && !$scope.directDepositData[0].routingNumber) {
                        $scope.isSoftWarned = true;
                        $scope.checkWorkfowItemCompletionStatus(false);
                        BubbleMessageService
                            .showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                        deffered.reject();
                        return deffered.promise;
                    }
                }

                setIsWorkflowCompleted();

                if (!$scope.frmPreference.$dirty) {
                    deffered.resolve();
                    return deffered.promise;
                }

                $scope.isSoftWarned = true;
                $scope.invalidOption = true;
                $scope.showHardStop = false;
                $scope.frmPreference.$setPristine();

                DsOnboardingApi.updateEmployeeOnboardingField($scope.employeeOnboardingField).then(function (reEmployeeOnboardingField) {
                    $scope.intAllowPaystubEmails = $scope.allowPaystubEmails ? 1 : 0;
                    $scope.intPayStubOption = -1;
                    if (!isUndefinedOrNullOrEmptyString($scope.payStubOption)) {
                        if ($scope.payStubOption == true)
                            $scope.intPayStubOption = 1;
                        else
                            $scope.intPayStubOption = 0;
                    }

                    if ($scope.prefStatusId == 2) {
                        $scope.directDepositData[0].amount = 100;
                        DsOnboardingApi.saveDirectDeposits($scope.employeeId, $scope.intPayStubOption, $scope.intAllowPaystubEmails, $scope.directDepositData).then(function (reDirectDepositData) {
                            myWorkflow.updateWorkflowTask(currState, $scope.isWorkflowTaskComplete);
                            deffered.resolve();
                        });
                    }
                    else
                    {
                        $scope.directDepositData = [];
                        DsOnboardingApi.saveDirectDeposits($scope.employeeId, $scope.intPayStubOption, $scope.intAllowPaystubEmails, $scope.directDepositData).then(function (reDirectDepositData) {
                            myWorkflow.updateWorkflowTask(currState, $scope.isWorkflowTaskComplete);
                            deffered.resolve();
                        });
                    }

                });
                return deffered.promise;
            }

            $scope.savePageData = savePageData;

            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                BubbleMessageService.hideBubbleMessage();
                //Check if page data is dirty.
                if ($scope.frmPreference.$dirty) {
                    if (validateRequiredFields()) {
                        savePageData(true).then(function () {
                        })
                        .catch(function () {
                            event.preventDefault();  //This does
                        }
                    );

                    }
                    else {
                        MessageService.sending(false);
                        MessageService.setMessageWithIcon("Missing required information. Please fill the required fields.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                        event.preventDefault();
                    }
                }

            });


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
