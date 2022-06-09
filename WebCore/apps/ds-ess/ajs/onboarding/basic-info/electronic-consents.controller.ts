import { STATES } from "../../shared/state-router-info";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingElectronicConsentsController {
    static readonly $inject = [
        "$rootScope",
        "$scope",
        AccountService.SERVICE_NAME,
        "$location",
        DsMsgService.SERVICE_NAME,
        "$modal",
        DsInlineValidatedInputService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        '$q'
    ];

    constructor ($rootScope,
        protected $scope: ng.IScope,
        accountService: AccountService,
        $location, MessageService, $modal, DsInlineValidatedInputService,
        DsOnboardingApi: DsOnboardingEmployeeApiService, protected workflowService: WorkflowService, $q) {
        function servicesProvider(userAccount, myWorkflow) {
            $scope.hasError = false;
            $scope.isLoading = true;
            $scope.savedChanges = false;

            //Used to pass workflow object into workflow footer via directive. [my-workflow]
            //The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;

            $scope.circleNumber = myWorkflow.getCircleNumber();
            var currState = myWorkflow.getCurrState();
            $scope.userData = {};
            $scope.value1095C = '1095COnlineOnly';
            $scope.valueW2 = 'W2OnlineOnly';
            $scope.value = '';
            //$scope.emailAddress = '';
            $scope.isConsent = true;
            $scope.isW2AgreeShow = true;

            $scope.isAcaEnabled = true;

            function init() {
                $scope.userData = userAccount;

                $scope.employeeId = userAccount.employeeId;

                DsOnboardingApi.getEmployeeOnboardingElectronicConsent(userAccount.employeeId).then(function (electronicConsentData) {

                    $scope.EmployeeOnboardingElectronicConsent = electronicConsentData;
                    $scope.isAcaEnabled = $scope.EmployeeOnboardingElectronicConsent.isAcaEnabled;
                    decideInsertOrUpdateForBothEntities();
                    if ($scope.EmployeeOnboardingElectronicConsent.employeeW2ConsentId === 0) {
                        //No data in DB means execute this
                        getEmployeeEmailAddress();
                        $scope.EmployeeOnboardingElectronicConsent.consentDateW2 = new Date();
                        $scope.frmElectronicConsents.$setDirty();
                        //ifNoDataInDBExistsDoFormalities();
                    } else {
                        //Data available in DB
                        dbToRadioW2();
                    }

                    if ($scope.EmployeeOnboardingElectronicConsent.has1095C === true) {
                        //No data in DB means execute this

                        dbToRadio1095C();

                    }
                    else {
                        $scope.EmployeeOnboardingElectronicConsent.consentDate1095C = new Date();
                    }
                    $scope.isLoading = false;

                });
            }
            init();

            function dbToRadioW2() {
                if ($scope.EmployeeOnboardingElectronicConsent.consentDateW2 === null) {

                    $scope.valueW2 = 'W2PrintedAndOnline';
                }
                else {
                    $scope.valueW2 = 'W2OnlineOnly';
                }





            }

            function dbToRadio1095C() {


                if ($scope.EmployeeOnboardingElectronicConsent.consentDate1095C === null) {

                    $scope.value1095C = '1095CPrintedAndOnline';
                }
                else {
                    $scope.value1095C = '1095COnlineOnly';
                }


            }

            function getEmployeeEmailAddress() {
                DsOnboardingApi.getEmployeeInfo(userAccount.employeeId).then(function (edata) {

                    //$scope.employeeContactInformationData = edata;
                    //console.log('$scope.employeeContactInformationData.emailAddress : ' + edata.emailAddress);
                    $scope.EmployeeOnboardingElectronicConsent.primaryEmailAddress = edata.emailAddress; // Here is the required firstname
                    //$scope.value = '';
                    //getW2Consent();
                });
            }

            function printedAndOnlineFormalitiesW2() {
                $scope.EmployeeOnboardingElectronicConsent.consentDateW2 = null;
                $scope.EmployeeOnboardingElectronicConsent.withdrawalDateW2 = new Date();
            }

            function printedAndOnlineFormalities1095C() {
                $scope.EmployeeOnboardingElectronicConsent.consentDate1095C = null;
                // console.log('consentDate1095C:' + 'COnsentdate 1095C set to null' + $scope.EmployeeOnboardingElectronicConsent.consentDate1095C);
                $scope.EmployeeOnboardingElectronicConsent.withdrawalDate1095C = new Date();
                //console.log('Withdrawaldate1095:' + 'Withdrawal date 1095C  set to date' + $scope.EmployeeOnboardingElectronicConsent.withdrawalDate1095C);
            }

            function onlineOnlyFormalitiesW2() {
                $scope.EmployeeOnboardingElectronicConsent.consentDateW2 = new Date();
                $scope.EmployeeOnboardingElectronicConsent.withdrawalDateW2 = null;
            }

            function onlineOnlyFormalities1095C() {
                $scope.EmployeeOnboardingElectronicConsent.consentDate1095C = new Date();
                $scope.EmployeeOnboardingElectronicConsent.withdrawalDate1095C = null;
            }

            $scope.w2CheckStuff = function (value) {
                if ($scope.EmployeeOnboardingElectronicConsent.employeeW2ConsentId === 0) {
                    if (value === 'W2PrintedAndOnline') {

                        printedAndOnlineFormalitiesW2();

                    }
                    else if (value === 'W2OnlineOnly') {

                        onlineOnlyFormalitiesW2();
                    }

                }
                else {
                    $scope.valueW2 = value;
                }
            }

            $scope.fn1095CCheckStuff = function (value) {
                if ($scope.EmployeeOnboardingElectronicConsent.has1095C === false) {
                    if (value === '1095CPrintedAndOnline') {

                        // console.log('New consent id for 1095Cprinted and online.NODB DATA');
                        printedAndOnlineFormalities1095C();

                    }
                    else if (value === '1095COnlineOnly') {

                        //console.log('New consent id for 1095Conlineonly.NODB DATA');
                        onlineOnlyFormalities1095C();
                    }
                }
                else {
                    $scope.value1095C = value;
                }




            }



            $scope.fnW2AgreementShow = function (element) {
                $scope.isW2AgreeShow = true;
                //console.log('Show');

            }

            $scope.fnW2AgreementHide = function (element) {
                $scope.isW2AgreeShow = false;
                //console.log('Show');
            }
            $scope.toggleW2Consent = function () {
                $scope.isConsent = false;
                $scope.isW2AgreeShow = true;
            }
            $scope.toggle1095CConsent = function () {
                $scope.isConsent = false;
                $scope.isW2AgreeShow = false;
            }

            $scope.toggleW2Agreement = function () {
                $scope.isConsent = true;
            }
            function addOrUpdateConsents() {
                DsOnboardingApi.addOnboardingElectronicConsent($scope.EmployeeOnboardingElectronicConsent).then(function (resData) {

                });
            }
            function updateConsents() {
                DsOnboardingApi.updateOnboardingElectronicConsent($scope.EmployeeOnboardingElectronicConsent).then(function (resData) {

                });
            }
            function ifDataInDBExistsDoFormalities() {
                if (($scope.valueW2 === 'W2PrintedAndOnline') && ($scope.EmployeeOnboardingElectronicConsent.consentDateW2 != null)) {
                    printedAndOnlineFormalitiesW2();
                }
                if (($scope.valueW2 === 'W2OnlineOnly') && ($scope.EmployeeOnboardingElectronicConsent.withdrawalDateW2 != null)) {
                    onlineOnlyFormalitiesW2();
                }
                if (($scope.value1095C === '1095CPrintedAndOnline') && ($scope.EmployeeOnboardingElectronicConsent.consentDate1095C != null)) {
                    printedAndOnlineFormalities1095C();
                }
                if (($scope.value1095C === '1095COnlineOnly') && ($scope.EmployeeOnboardingElectronicConsent.withdrawalDate1095C != null)) {
                    onlineOnlyFormalities1095C();
                }
            }


            function decideInsertOrUpdateForBothEntities() {
                if ($scope.EmployeeOnboardingElectronicConsent.employeeW2ConsentId === 0) {
                    //No DB Data so insert
                    $scope.EmployeeOnboardingElectronicConsent.isW2Insert = true;
                }

                if ($scope.EmployeeOnboardingElectronicConsent.has1095C === false) {
                    $scope.EmployeeOnboardingElectronicConsent.is1095CInsert = true;
                }
            }

            $scope.addOrUpdateNewConsent = function () {
                if ($scope.EmployeeOnboardingElectronicConsent.employeeW2ConsentId === 0) {
                    //No DB Data so insert
                    //$scope.EmployeeOnboardingElectronicConsent.employeeW2ConsentId = null;
                    $scope.EmployeeOnboardingElectronicConsent.isW2Insert = true;
                    $scope.EmployeeOnboardingElectronicConsent.employeeId = $scope.employeeId;
                    // console.log('$scope.EmployeeOnboardingElectronicConsent.consentDate1095C:' + $scope.EmployeeOnboardingElectronicConsent.consentDate1095C);
                    addOrUpdateConsents();

                }
                else {
                    //DB Data present so Update
                    ifDataInDBExistsDoFormalities();

                    //End Constraint Check for previous DB Data
                    $scope.EmployeeOnboardingElectronicConsent.employeeId = $scope.employeeId;
                    //console.log('$scope.EmployeeOnboardingElectronicConsent.consentDate1095C:' + $scope.EmployeeOnboardingElectronicConsent.consentDate1095C);
                    addOrUpdateConsents();



                }
            }

            function savePageData() {
                var deffered = $q.defer();
                var isWorkflowTaskComplete = false;

                MessageService.sending(true);
                $scope.isSaving = true;
                if ($scope.EmployeeOnboardingElectronicConsent.employeeW2ConsentId === 0) {
                    $scope.EmployeeOnboardingElectronicConsent.isW2Insert = true;
                    $scope.EmployeeOnboardingElectronicConsent.employeeId = $scope.employeeId;
                    DsOnboardingApi.addOnboardingElectronicConsent($scope.EmployeeOnboardingElectronicConsent).then(function (resData) {
                        if (resData.hasNoError) {
                            $scope.savedChanges = true;
                            isWorkflowTaskComplete = true;
                            $scope.frmElectronicConsents.$setPristine();
                            myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);

                            MessageService.sending(false);
                            deffered.resolve();

                        } else {
                            MessageService.sending(false);
                            MessageService.setMessage("An error has occurred.", MessageService.messageTypes.error);
                            deffered.reject();
                        }
                    });
                }
                else {
                    //DB Data present so Update
                    ifDataInDBExistsDoFormalities();

                    //End Constraint Check for previous DB Data
                    $scope.EmployeeOnboardingElectronicConsent.employeeId = $scope.employeeId;
                    DsOnboardingApi.addOnboardingElectronicConsent($scope.EmployeeOnboardingElectronicConsent).then(function (resData) {
                        if (resData.hasNoError) {
                            $scope.savedChanges = true;
                            isWorkflowTaskComplete = true;
                            myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);

                            MessageService.sending(false);
                            deffered.resolve();

                        } else {
                            MessageService.sending(false);
                            MessageService.setMessage("An error has occurred.", MessageService.messageTypes.error);
                            deffered.reject();
                        }
                    });
                }


                return deffered.promise;

            }

            $scope.savePageData = savePageData;
            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                if ($scope.frmElectronicConsents.$dirty && $scope.savedChanges !== true) {
                    savePageData().then(function () {
                    })
                    .catch(function () {
                        event.preventDefault();  //This does
                    });
                }
                //else {
                //    MessageService.sending(false);
                //    event.preventDefault();
                //}
                //}
            });
        }

        accountService.getUserInfo().then(user => {
            workflowService.getUserWorkflowPromise().then(workflow => {
                servicesProvider(user, workflow);
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
