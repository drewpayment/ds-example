import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsOnboardingFormsApiService } from "@ajs/onboarding/forms/ds-onboarding-forms-api.service";
import { isUndefinedOrNullOrEmptyString } from "@util/ds-common";
import { EssLeaveTimeoffState } from '../../leave/time-off/header.state';
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingW4FederalController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        '$q',
        DsOnboardingFormsApiService.SERVICE_NAME
    ];
    constructor (
        protected $scope,
        accountService: AccountService,
        MessageService,
        BubbleMessageService,
        DsOnboardingApi: DsOnboardingEmployeeApiService,
        protected workflowService: WorkflowService,
        $q,
        formsApi
    ) {

        function init(userAccount, myWorkflow) {
            //Used to pass workflow object into workflow footer via directive. [my-workflow]
            //The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;
            $scope.downloadForm = downloadForm;
            $scope.form = null;
            $scope.circleNumber = myWorkflow.getCircleNumber();
            var currState = myWorkflow.getCurrState();
            $scope.savedChanges = false;
            $scope.hasError = false;
            $scope.isLoading = true;
            $scope.isSoftWarned = false;
            $scope.employeeId = userAccount.employeeId;
            $scope.federalW4Year = null;

            $scope.dirty = false;
            $scope.cnt = 0;
            $scope.isCurrYearValid = true;
            $scope.isLastYearValid = true;

            $scope.filingStatuses = [
                { id: 1, desc: "Married" },
                { id: 2, desc: "Single" },
                { id: 3, desc: "Head Of Household" }
            ];

            $scope.employeeW4FederalData = {};
            //Default values
            setDefaults();

            var isCompleted = isUndefinedOrNullOrEmptyString(myWorkflow.isStateCompleted(currState))

            $scope.isCompleted = isUndefinedOrNullOrEmptyString(isCompleted) || isCompleted === true;

            //load the status of the current employee's onboarding forms
            formsApi.getEmployeeFormStatus(userAccount.employeeId).then(function (forms) {

                if (forms) {
                    for (var i = 0; i < forms.length; i++) {
                        if (forms[i].formName === 'Federal W-4') {
                            $scope.form = forms[i];
                            $scope.federalW4Year = forms[i].formVersion;
                        }
                    }
                }

                $scope.isLoading = false;
            });



            //Load Data
            DsOnboardingApi.getEmployeeW4ByTaxCategory(userAccount.employeeId, 1)
                .then(function (data) {
                    if (data !== null && data !== undefined ) {
                        $scope.employeeW4FederalData = data;
                        if($scope.employeeW4FederalData.otherDependents != 0 || $scope.employeeW4FederalData.qualifyingChildren != 0) {$scope.incomeUnderDependentThreshold = true;}
                        if($scope.employeeW4FederalData.otherTaxableIncome != 0) {$scope.hasOtherTaxableIncome = true;}
                        if($scope.employeeW4FederalData.wageDeduction != 0) {$scope.hasWageDeduction = true;}

                        //clean up description for some filing statuses, setting one of the 6 new ones, back to 1,2,3 to display correctly, they are converted back before data is sent
                        if($scope.employeeW4FederalData.filingStatus == 17 || $scope.employeeW4FederalData.filingStatus == 18){
                            $scope.employeeW4FederalData.filingStatus = 2;
                        }
                        else if($scope.employeeW4FederalData.filingStatus == 19 || $scope.employeeW4FederalData.filingStatus == 20){
                            $scope.employeeW4FederalData.filingStatus = 1;
                        }
                        else if($scope.employeeW4FederalData.filingStatus == 21 || $scope.employeeW4FederalData.filingStatus == 22){
                            $scope.employeeW4FederalData.filingStatus = 3;
                        }
                    }

                    $scope.$watchCollection('employeeW4FederalData', function () {
                        if ($scope.cnt > 0) $scope.dirty = true;
                        $scope.cnt++;
                    }, true);
                    DsOnboardingApi.getEmployeeW4TotalExemptions(userAccount.employeeId).then(function (data) {
                            if (!isUndefinedOrNullOrEmptyString(data)) {
                                $scope.employeeW4FederalData.totalExemptions = data.w4TotalExemptions;
                                if (!$scope.employeeW4FederalData.filingStatus && data.filingStatus) { // If not available on EmployeeOnboardingW4 yet, assign the one from EmployeeOnboardingW4Assist
                                    $scope.employeeW4FederalData.filingStatus = data.filingStatus;
                                }
                            }
                            $scope.isLoading = false;
                            $scope.hasError = false;
                    });


                })
                .catch(getFailed);



            function getFailed() {
                alert("An error has occurred.");

            }

            function validRequiredFields() {
                $scope.isCurrYearValid = true;
                $scope.isLastYearValid = true;
                if ($scope.employeeW4FederalData.isTaxExempt &&  $scope.federalW4Year == '2019') {
                    if (!$scope.employeeW4FederalData.isTaxExemptionCurrYr) $scope.isCurrYearValid = false;
                    if (!$scope.employeeW4FederalData.isTaxExemptionLastYr) $scope.isLastYearValid = false;
                }
                return $scope.isCurrYearValid && $scope.isLastYearValid;

            }
            function setDefaults(){
                $scope.employeeW4FederalData.allowances = null;
                $scope.employeeW4FederalData.filingStatus = null;

                $scope.employeeW4FederalData.taxCategory = 1;
                $scope.employeeW4FederalData.createDt = null;
                $scope.employeeW4FederalData.totalExemptions = null;
                $scope.employeeW4FederalData.qualifyingChildren = null;
                $scope.employeeW4FederalData.otherDependents = null;
                $scope.employeeW4FederalData.qualifyingChildrenAmount = null;
                $scope.employeeW4FederalData.otherDependentsAmount = null;
                $scope.employeeW4FederalData.taxCredit = null;

                $scope.employeeW4FederalData.wageDeduction = null;
                $scope.employeeW4FederalData.additionalWithholdingAmt = null;
                $scope.employeeW4FederalData.otherTaxableIncome = null;

                $scope.hasWageDeduction = false;
                $scope.hasOtherTaxableIncome = false;
                $scope.incomeUnderDependentThreshold = false;
                $scope.employeeW4FederalData.isAdditionalAmountWithheld = false;
                $scope.employeeW4FederalData.hasMoreThanOneJob = false;
            }

            $scope.CheckExemptStatus = function () {
                if ($scope.employeeW4FederalData.isTaxExempt) {
                    setDefaults();
                }
                else {
                    $scope.employeeW4FederalData.isTaxExemptionCurrYr = null;
                    $scope.employeeW4FederalData.isTaxExemptionLastYr = null;
                }
            }

            function savePageData(withoutSoftWarn) {
                var deffered = $q.defer();
                BubbleMessageService.hideBubbleMessage();

                //Validate required fields.
                if (validRequiredFields()) {
                    var isWorkflowTaskComplete = !$scope.frm.$error.required;
                    if (isWorkflowTaskComplete || $scope.isSoftWarned || withoutSoftWarn) {
                        $scope.isSoftWarned = false;
                    $scope.savingChanges = true;
                    MessageService.sending(true);
                    $scope.employeeW4FederalData.using2020FederalW4Setup = $scope.federalW4Year == "2020" || $scope.federalW4Year == "2021" || $scope.federalW4Year == "2022";
                    $scope.employeeW4FederalData.qualifyingChildrenAmount = $scope.employeeW4FederalData.qualifyingChildren*2000.00;
                    $scope.employeeW4FederalData.otherDependentsAmount = $scope.employeeW4FederalData.otherDependents*500.00;
                    $scope.employeeW4FederalData.taxCredit = $scope.employeeW4FederalData.qualifyingChildrenAmount + $scope.employeeW4FederalData.otherDependentsAmount;

                    //change current filing status to send new 6 filing status ids based on filing status and whether the HasMoreThanOneJob box is checked or not

                    if( $scope.employeeW4FederalData.hasMoreThanOneJob){ //user has more than one job
                        if($scope.employeeW4FederalData.filingStatus == 1){ //married
                            $scope.employeeW4FederalData.filingStatus = 19 //married w/ box2
                        }
                        else if($scope.employeeW4FederalData.filingStatus == 2){ //single
                            $scope.employeeW4FederalData.filingStatus = 17 //single w/ box2
                        }
                        else if($scope.employeeW4FederalData.filingStatus == 3){ //Hoh
                            $scope.employeeW4FederalData.filingStatus = 21 //Hoh w/ box2
                        }
                    }
                    else{ //user didnt check has more than one job
                        if($scope.employeeW4FederalData.filingStatus == 1){ //married
                            $scope.employeeW4FederalData.filingStatus = 20 //married w/o box2
                        }
                        else if($scope.employeeW4FederalData.filingStatus == 2){ //single
                            $scope.employeeW4FederalData.filingStatus = 18 //single w/o box2
                        }
                        else if($scope.employeeW4FederalData.filingStatus == 3){ // Hoh
                            $scope.employeeW4FederalData.filingStatus = 22 //Hoh w/o box2
                        }
                    }

                    DsOnboardingApi.putEmployeeW4($scope.employeeW4FederalData)
                        .then(function (data) {

                            MessageService.sending(false);
                            $scope.savingChanges = false;
                            $scope.frm.$setPristine();
                            //Check for errors.
                            if (data.hasNoError) {
                                $scope.savedChanges = true;
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


            //Workflow Change
            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                //Check if page data is dirty.
                if ($scope.frm.$dirty && $scope.savedChanges !== true) {
                    if (validRequiredFields()) {
                        savePageData(true).then(function () {
                        })
                        .catch(function () {
                            event.preventDefault();  //This does
                        }
                    );

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

                        // check if user does not have permission to view page.
                        if (error.messageType === MessageService.validationMessageTypes.actionNotAllowed) {
                            MessageService.setMessage("You do not have permission to view this page.", MessageService.messageTypes.error);
                        }

                    });
                }
            }


            function clearErrors() {
                $scope.hasError = false;
            }

            /**
             * Downloads the saved version of a form.
             * @param {Object} form - The form to download.
             */
            function downloadForm(form) {
                formsApi
                    .downloadFormPreview(form);
                    //.catch(msgs.showWebApiException);
            }

            $scope.toggleCheckbox = function(event, checkboxName): void {
                if( event.target.checked == false){
                    if(checkboxName == 'hasWageDeduction'){
                        $scope.employeeW4FederalData.wageDeduction = 0;
                    }
                    else if(checkboxName == 'isAdditionalAmountWithheld'){
                        $scope.employeeW4FederalData.additionalWithholdingAmt = 0;
                    }
                    else if(checkboxName == "hasOtherTaxableIncome"){
                        $scope.employeeW4FederalData.otherTaxableIncome = 0;
                    }
                    else if(checkboxName == "incomeUnderDependentThreshold"){
                        $scope.employeeW4FederalData.qualifyingChildren = 0;
                        $scope.employeeW4FederalData.otherDependents = 0;
                    }
                }
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
