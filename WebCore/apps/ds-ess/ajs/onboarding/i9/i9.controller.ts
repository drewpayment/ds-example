import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { isUndefinedOrNullOrEmptyString } from "@util/ds-common";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingI9Controller {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        CountryStateService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        '$q'
    ];

    constructor (
        protected $scope: ng.IScope,
        accountService: AccountService,
        MessageService,
        BubbleMessageService,
        DsOnboardingApi,
        locations,
        protected workflowService: WorkflowService,
        $q
    ) {

        function init(userAccount, myWorkflow) {
            $scope.hasError = false;
            $scope.isLoading = true;
            $scope.isLoaded = false;
            $scope.userData = {};
            $scope.isSoftWarned = false;
            var isPageDirty = false;

            //Used to pass workflow object into workflow footer via directive. [my-workflow]
            //The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;

            $scope.circleNumber = myWorkflow.getCircleNumber();
            var currState = myWorkflow.getCurrState();
            var isCompleted = isUndefinedOrNullOrEmptyString(myWorkflow.isStateCompleted(currState));
            $scope.stateDisabled = false;
            $scope.isCompleted = isUndefinedOrNullOrEmptyString(isCompleted) || isCompleted === true;
            $scope.YesNoPairs = [{ txt: 'Yes', val: true }, { txt: 'No', val: false }];
            $scope.alienAdmissionNumberTypes = [{ id: 1, desc: "Alien Registration Number/USCIS Number" }
                                        , { id: 2, desc: "I-94 Admission Number" }
                                        , { id: 3, desc: "Foreign Passport Number" }];
            $scope.translatorNonUs = false;
            $scope.translatorStates = {};
            $scope.I9DocumentList = {};
            $scope.showListA = false;
            $scope.showListB = false;
            $scope.showListC = false;
            $scope.lblOtherNameNA = "";
            $scope.employeeI9 = {};

            function initUserData() {
                $scope.userData = userAccount;


                LoadCountriesState();


                DsOnboardingApi.getI9DocumentList().then(function (docs) {
                    $scope.I9DocumentList = docs;
                });
                DsOnboardingApi.getEmployeeI9(userAccount.employeeId).then(function (i9Data) {
                    $scope.employeeI9 = i9Data;

                    if (!i9Data.employeeId)
                        $scope.employeeI9.i9EligibilityStatusId = 1;
                    else if (!i9Data.otherName)
                        $scope.lblOtherNameNA = "N/A";

                    if ($scope.employeeI9.i9EligibilityStatusId == 0) {
                        $scope.isSoftWarned = false;

                    }
                    if ($scope.employeeI9.translatedDate != null) {
                        $scope.employeeI9.translatedDate = $scope.employeeI9.translatedDate.substr(5, 2) + "/" + $scope.employeeI9.translatedDate.substr(8, 2) + "/" + $scope.employeeI9.translatedDate.substr(0, 4);
                    }
                    if ($scope.employeeI9.authorizedWorkDate != null) {
                        $scope.employeeI9.authorizedWorkDate = $scope.employeeI9.authorizedWorkDate.substr(5, 2) + "/" + $scope.employeeI9.authorizedWorkDate.substr(8, 2) + "/" + $scope.employeeI9.authorizedWorkDate.substr(0, 4);
                    }
                    if ($scope.employeeI9.translatorAddress == null) {
                        $scope.loadTranslatorStates();
                    } else {
                        $scope.loadTranslatorStates();
                        $scope.translatorNonUs = $scope.employeeI9.translatorAddress.countryId > 1;
                    }

                    $scope.isLoading = false;
                    $scope.isLoaded = true;
                });

            }
            initUserData();

            $scope.loadTranslatorStates = function () {
                var country;
                if ($scope.employeeI9.translatorAddress == null) {
                    country = locations.defaults.countries.usa;
                } else {
                    country = $scope.employeeI9.translatorAddress.countryId;
                }
                locations.getStatesByCountry(country).then(translatorStateSuccess).catch(errorCallback);
            }


            $scope.toggleTranslatorAddress = function () {
                // Clear only the zip code for now
                // DO not clear all other values in transalator address

                    $scope.employeeI9.translatorAddress.zipCode = "";


            }

            function translatorStateSuccess(data) {
                $scope.translatorStates = data;
                if ($scope.translatorStates.length === 0){
                    $scope.stateDisabled = true;

                } else {
                    $scope.stateDisabled = false;
                }
            }

            function LoadCountriesState() {
                locations.getCountryList().then(countriesSuccess).catch(errorCallback);

            }

            function countriesSuccess(data) {
                //  countries = data;
                $scope.countries = data;

            }
            function errorCallback(data) {
                $scope.hasError = true;
                $scope.isLoading = false;
                $scope.isLoaded = true;

                if (data.errors) {

                    angular.forEach(data.errors, function (error) {

                        // check if user does not have permission to view Taxes
                        if (error.messageType === MessageService.validationMessageTypes.actionNotAllowed) {
                            MessageService.setMessage("You do not have permission to view onboarding.", MessageService.messageTypes.error);

                        }

                    });
                }
            }

            $scope.setEligStatus = function setEligStatus(status) {
                $scope.employeeI9.i9EligibilityStatusId = status;
            }

            function validRequiredFields() {

                //if (isUndefinedOrNullOrEmptyString($scope.employeeI9.i9EligibilityStatusId))
                    //return false;
                return true;
            }

            function setDefaults() {
                if($scope.employeeI9.alienAdmissionNumberType==3)
                {
                    $scope.employeeI9.alienAdmissionNumber = null;
                }
                else
                {
                    $scope.employeeI9.foreignPassportNumber = null;
                    $scope.employeeI9.passportCountryId = null;
                }
            }

            function savePageData(withoutSoftWarn) {
                var deffered = $q.defer();
                BubbleMessageService.hideBubbleMessage();
                if (validRequiredFields() == false) {
                    MessageService.setMessageWithIcon("Missing required information. Please fill the required fields.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                    deffered.reject();
                }
                else {
                    var isWorkflowTaskComplete;
                    if ($scope.employeeI9.i9EligibilityStatusId == 0) {
                        // $scope.isSoftWarned = false;
                        isWorkflowTaskComplete = false;
                    }
                    else {
                        isWorkflowTaskComplete = !$scope.frmi9.$error.required;
                    }


                    if (isWorkflowTaskComplete || $scope.isSoftWarned || withoutSoftWarn) {
                        $scope.isSoftWarned = false;

                        $scope.savingChanges = true;
                        MessageService.sending(true);
                        setDefaults();
                        DsOnboardingApi.putEmployeeI9($scope.employeeI9)
                            .then(function (data) {
                                MessageService.sending(false);
                                $scope.savingChanges = false;
                                $scope.frmi9.$setPristine();
                                //Check for errors.
                                if (data.hasNoError) {
                                    myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
                                    isPageDirty = false;

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

                }

                return deffered.promise;
            }

            $scope.savePageData = savePageData;

            //Menu Change
            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                //Check if page data is dirty.
                if ($scope.frmi9.$dirty && $scope.savedChanges !== true) {
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
                        MessageService.setMessageWithIcon("Missing required information. Please fill the required fields.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                        event.preventDefault();
                    }
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
