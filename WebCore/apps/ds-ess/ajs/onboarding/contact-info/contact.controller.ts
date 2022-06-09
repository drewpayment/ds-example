import { isUndefinedOrNullOrEmptyString, isNotUndefinedOrNull, valdateUsingRegExp } from "@util/ds-common";
import { STATES } from "../../shared/state-router-info";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';
import { truncate } from "fs";

export class OnboardingContactInfoAddController {
    static readonly $inject = [
        "$rootScope",
        "$scope",
        CountryStateService.SERVICE_NAME,
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        DsStateService.SERVICE_NAME,
        '$q'
    ];
    constructor (
        $rootScope,
        protected $scope: ng.IScope,
        locations: CountryStateService,
        accountService: AccountService,
        MessageService,
        BubbleMessageService,
        DsOnboardingApi: DsOnboardingEmployeeApiService,
        protected workflowService: WorkflowService,
        DsState,
        $q
        ) {

        function init(userAccount, myWorkflow) {
            //Used to pass workflow object into workflow footer via directive. [my-workflow]
            //The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;

            //Re-route if finalized.
            //myWorkflow.checkFinalizeStatusAndNavigate();

            $scope.circleNumber = myWorkflow.getCircleNumber();

            var currState = myWorkflow.getCurrState();
            var isCompleted = isUndefinedOrNullOrEmptyString(myWorkflow.isStateCompleted(currState));
            $scope.isCompleted = isUndefinedOrNullOrEmptyString(isCompleted) || isCompleted === true;

            $scope.SSNExp = "^(?!\\b(\\d)1+-(\\d)1+-(\\d)1+\\b)(?!123-45-6789|219-09-9999|078-05-1120)(?!666|000|9\\d{2})\\d{3}-(?!00)\\d{2}-(?!0{4})\\d{4}$";

            $scope.isSaving = false;
            $scope.isValid = true;

            //$scope.driversLicense = $rootScope.hasLicense;
            $scope.locationUS = true;

            $scope.requiredToContinue = false;

            $scope.countyDisabled = false;
            $scope.stateDisabled = false;

            $scope.validSSN = true;
            $scope.isSoftWarned = false;

            $scope.showInValidSSNMsg = true;
            $scope.requiredSSN = false;

            $scope.requiredZipUS = false;
            $scope.validZipUS = true;
            $scope.showInValidZipUSMsg = true;
            $scope.zipUSExp = "^(\\d{5}(-\\d{4})?|[A-Z]\\d[A-Z] *\\d[A-Z]\\d)$";

            $scope.requiredZipCanada = false;
            $scope.validZipCanada = true;
            $scope.showInValidZipCanadaMsg = true;
            $scope.zipCanadaExp = "^[A-Za-z]\\d[A-Za-z][ -]?\\d[A-Za-z]\\d$";            

            $scope.SSNFieldType = "Password";
            $scope.isConfirmSSNValid=true;
            $scope.hasError = false;
            $scope.isLoaded = false;
            $scope.gender = [{ value: null, desc: "" }, { value: "N", desc: "Prefer not to answer" }, { value: "M", desc: "Male" }, { value: "F", desc: "Female" } ];

            $scope.firstPass = true;
            $scope.detailedDescription = "";
            var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var monthDays = [];
            var monthDaysStatic = [];
            var monthDayCount = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            var years = [];

            var d = new Date();
            var n = d.getFullYear();

            var beginYear = n - 90;
            var endYear = n - 10;


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
            var dummyMonthName = { value: null, desc: "Month" };
            $scope.monthNames.unshift(dummyMonthName);

            $scope.monthDays = monthDays;
            var dummyMonthDay = { value: null, desc: "Day" };
            $scope.monthDays.unshift(dummyMonthDay);
            $scope.years = years;
            var dummyYears = { value: null, desc: "Year" };
            $scope.years.unshift(dummyYears);

            DsOnboardingApi.getEmployeeInfo(userAccount.employeeId).then(function (data) {
                $scope.employeeContactInformationData = data;
                $scope.employeeContactInformationData.confirmSocialSecurityNumber=$scope.employeeContactInformationData.socialSecurityNumber;
                $scope.employeeContactInformationData.maritalStatusId = $scope.employeeContactInformationData.maritalStatusId ? 
                    $scope.employeeContactInformationData.maritalStatusId : 0;
                DsOnboardingApi.employeeStartsOnboarding(userAccount.employeeId).then(function (finData) {

                });

                unPackDOB();
                $scope.changeMonthDays();
                $scope.locationUS = false;

                if (isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.countryId)) {
                    $scope.employeeContactInformationData.countryId = 1;
                    $scope.locationUS = true;
                } else {
                    if ($scope.employeeContactInformationData.countryId === 1) {
                        $scope.locationUS = true;
                    }
                }

                if (!isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.driversLicenseExpirationDate)) {
                    var licDate = $scope.employeeContactInformationData.driversLicenseExpirationDate;
                    $scope.licDate = licDate.substr(5, 2) + "/" + licDate.substr(8, 2) + "/" + licDate.substr(0, 4);
                }

                LoadCountriesState();

                //Previously, we had  " " as value for Gender - Prefer Not to answer. We have now made it as "N" and added another item with value as null. So, all the existing " " will be considered as not selected anything.
                if (isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.gender)) $scope.employeeContactInformationData.gender = null;

                DsOnboardingApi.getEeocRaceEthnicCategoryList().then(function (raceData) {
                    $scope.EeocRaceEthnicCategoryList = raceData;
                    var dummyRace = { raceId: null, description: "", detailedDescription: "" };
                    var prefereNotToAnswer = _.find($scope.EeocRaceEthnicCategoryList, function(o) { return o.raceId == 0; });

                    $scope.EeocRaceEthnicCategoryList = _.filter($scope.EeocRaceEthnicCategoryList, function(o) { return o.raceId != 0; });

                    $scope.EeocRaceEthnicCategoryList = _.sortBy($scope.EeocRaceEthnicCategoryList, ['description']);

                    $scope.EeocRaceEthnicCategoryList.unshift(prefereNotToAnswer);
                    $scope.EeocRaceEthnicCategoryList.unshift(dummyRace);
                    $scope.changeRace();
                });

                $scope.isLoaded = true;
                $scope.hasError = false;

            });

            var countries = {};
            var countryStates = [];

            function LoadCountriesState() {
                locations.getCountryList().then(countriesSuccess, errorCallBack);

            }

            function countriesSuccess(data) {
                countries = data;
                $scope.countries = countries;

                if (isNotUndefinedOrNull($scope.employeeContactInformationData.countryId)) {
                    const promise = locations.getStatesByCountry($scope.employeeContactInformationData.countryId);
                    promise.then(countryStateSuccess, errorCallBack);
                }
            }

            function countryStateSuccess(data) {
                countryStates = data;
                $scope.states = countryStates;
                $scope.stateDisabled = false;
                $scope.countyDisabled = true;
                if ($scope.states.length === 0) $scope.stateDisabled = true;
                if (countryStates.length === 1) $scope.employeeContactInformationData.stateId = countryStates[0].stateId;
                if (isNotUndefinedOrNull($scope.employeeContactInformationData.stateId)) {
                    const promise = locations.getCountiesByState($scope.employeeContactInformationData.stateId);
                    promise.then(countiesSuccess, errorCallBack);
                }
            }

            function countiesSuccess(data) {
                $scope.countyDisabled = false;
                $scope.counties = data;
                if ($scope.counties.length === 0) $scope.countyDisabled = true;
            }

            $scope.changeCountryStates = function changeCountryStates() {
                $scope.employeeContactInformationData.stateId = null;
                $scope.employeeContactInformationData.countyId = null;
                $scope.stateDisabled = true;
                $scope.countyDisabled = true;
                $scope.employeeContactInformationData.stateId = null;
                $scope.employeeContactInformationData.countyId = null;
                $scope.employeeContactInformationData.postalCode = null;
                $scope.counties = {};

                if ($scope.employeeContactInformationData.countryId === 1) $scope.locationUS = true;
                locations.getStatesByCountry($scope.employeeContactInformationData.countryId).then(countryStateSuccess, errorCallBack);
            }

            $scope.changeStateCounties = function changeStateCounties() {
                $scope.countyDisabled = true;
                $scope.employeeContactInformationData.countyId = null;
                $scope.counties = {};
                locations.getCountiesByState($scope.employeeContactInformationData.stateId).then(countiesSuccess, errorCallBack);
            }

            $scope.changeRace = function changeRace() {
                var raceId = $scope.employeeContactInformationData.eeocRaceId;
                $scope.detailedDescription = "";
                if ($scope.EeocRaceEthnicCategoryList) {
                    for (var i = 1; i < $scope.EeocRaceEthnicCategoryList.length; i++) {
                        if ($scope.EeocRaceEthnicCategoryList[i].raceId === raceId) {
                            $scope.detailedDescription = $scope.EeocRaceEthnicCategoryList[i].detailedDescription;
                        }
                    }

                }
            }

            function IsLeapYear(year) {
                if (parseInt(year) % 4 === 0 && (parseInt(year) % 100 !== 0 || parseInt(year) % 400 !== 0)) {
                    return true;
                }

                return false;
            }

            $scope.toggleDriversLicense = function toggleDriversLicense() {
                $scope.frmContact.$setDirty();
                //$scope.driversLicense = !$scope.driversLicense;
                //$rootScope.hasLicense = $scope.driversLicense;

                //if (!$scope.driversLicense) {
                //    clearDriveresLicense();
                //}
                if ($scope.employeeContactInformationData.noDriversLicense) {
                        clearDriveresLicense();
                    }
            }

            function clearDriveresLicense() {
                $scope.employeeContactInformationData.driversLicenseNumber = null;
                $scope.employeeContactInformationData.driversLicenseIssuingStateId = 0;
                $scope.licDate = null;
            }

            $scope.toggleLocationUS = function toggleLocationUS() {

                $scope.frmContact.$setDirty();
                $scope.employeeContactInformationData.postalCode = "";
                $scope.locationUS = !$scope.locationUS;

                if ($scope.locationUS) {
                    locations.getStatesByCountry(locations.defaults.countries.usa).then(countryStateSuccess, errorCallBack);
                    $scope.employeeContactInformationData.countryId = 1;
                } else {

                    $scope.employeeContactInformationData.stateId = null;
                    $scope.employeeContactInformationData.countyId = null;

                    $scope.employeeContactInformationData.noDriversLicense = false;
                    clearDriveresLicense();
                    locations.getStatesByCountry($scope.employeeContactInformationData.countryId).then(countryStateSuccess, errorCallBack);
                }

            }

            $scope.validateSSN = function validateSSN() {

                $scope.validSSN = valdateUsingRegExp($scope.SSNExp, $scope.employeeContactInformationData.socialSecurityNumber);

            }

            $scope.validateZipUS = function validateZipUS() {
                $scope.validZipUS = valdateUsingRegExp($scope.zipUSExp, $scope.employeeContactInformationData.postalCode);
            }

            $scope.validateZipCanada = function validateZipCanada() {
                $scope.validZipCanada = valdateUsingRegExp($scope.zipCanadaExp, $scope.employeeContactInformationData.postalCode);
            }

            $scope.toggleSSNFieldType = function toggleSSNFieldType() {
                if ($scope.SSNFieldType === "Password") {
                    $scope.SSNFieldType = "Text";
                } else {
                    $scope.SSNFieldType = "Password";
                };
            }
            $scope.changeMonthDays = function () {
                var month;
                if ($scope.DOBmonth != 0) {
                    month = $scope.DOBmonth;
                }
                var endDay = monthDayCount[month - 1];

                if (month === 2) {
                    if (IsLeapYear($scope.dobYear)) {
                        endDay = 29;
                    }
                }

                $scope.monthDays = monthDaysStatic.slice(0, endDay);
                var dummyMonthDay = { value: null, desc: "Day" };
                $scope.monthDays.unshift(dummyMonthDay);
            }
            function unPackDOB() {
                let dateOfBirth = $scope.employeeContactInformationData.birthDate;
                if (isNotUndefinedOrNull(dateOfBirth)) {
                    $scope.DOBday = Number(dateOfBirth.substring(8, 10));
                    $scope.DOBmonth = Number(dateOfBirth.substring(5, 7));
                    $scope.DOByear = Number(dateOfBirth.substring(0, 4));
                } else {
                    $scope.DOBday = null;
                    $scope.DOBmonth = null;
                    $scope.DOByear = null;
                }
            }

            function packDOB() {
                var dobM;
                var dobD;
                if ($scope.DOBmonth.toString().length == 1) {
                    dobM = "0" + $scope.DOBmonth.toString();
                }
                else {
                    dobM = $scope.DOBmonth.toString();
                }
                if ($scope.DOBday.toString().length == 1) {
                    dobD = "0" + $scope.DOBday.toString();
                }
                else {
                    dobD = $scope.DOBday.toString();
                }
                var dobString = $scope.DOByear.toString() + "-" + dobM + "-" + dobD;

                return parseISO8601(dobString);
                //if (isNaN(Date.parse(dob))) {

                //    return null;
                //}

                //return dob;

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

            function validateRequiredFields() {

                var areFieldsValid = false;
                var isValidLicense = true;
                
                $scope.employeeContactInformationData.driversLicenseExpirationDate = $scope.licDate;

                //Setting the drivers license to null will remove the drivers license information from the database
                // or will prevent the drivers license information from being created in the database.
                if ($scope.employeeContactInformationData.noDriversLicense) {
                    $scope.employeeContactInformationData.driversLicenseNumber = null;
                    $scope.employeeContactInformationData.driversLicenseIssuingStateId = 0;

                }
                else if (!isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.driversLicenseNumber) && $scope.employeeContactInformationData.driversLicenseIssuingStateId === 0 && $scope.locationUS) {
                    isValidLicense = false;

                }

                if (!isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.socialSecurityNumber) && $scope.employeeContactInformationData.socialSecurityNumber != $scope.employeeContactInformationData.confirmSocialSecurityNumber){
                    $scope.isConfirmSSNValid = false;
                }
                else{
                    $scope.isConfirmSSNValid = true;
                }

                areFieldsValid = $scope.frmContact.FirstName.$valid && $scope.frmContact.LastName.$valid && $scope.frmContact.primaryPhone.$valid && isValidLicense && $scope.isConfirmSSNValid;
                

                if (!areFieldsValid) {
                    MessageService.setMessageWithIcon("Missing required information. Please fill the required fields.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                }

                if ($scope.employeeContactInformationData.countryId == 1 && $scope.employeeContactInformationData.postalCode) {
                    $scope.validateZipUS();
                    $scope.showInValidZipUSMsg=true;
                    if (!$scope.validZipUS && $scope.showInValidZipUSMsg && $scope.employeeContactInformationData.postalCode.length > 0) {
                        //MessageService.setMessageWithIcon("Zip code must be either 5 (49045) or 9 (49505-6528) digits.", MessageService.messageTypes.error, MessageService.iconTypes.error);
                        areFieldsValid = false;
                    }
                }

                if ($scope.employeeContactInformationData.countryId == 7 && $scope.employeeContactInformationData.postalCode) {
                    $scope.validateZipCanada();
                    $scope.showInValidZipCanadaMsg=true;
                    if (!$scope.validZipCanada && $scope.showInValidZipCanadaMsg && $scope.employeeContactInformationData.postalCode.length > 0 ) {
                        //MessageService.setMessageWithIcon("Zip code must be in the format 'A1A 0A0'.", MessageService.messageTypes.error, MessageService.iconTypes.error); 
                        areFieldsValid = false;
                    }
                }

                return areFieldsValid;
            }

            function validateSoftData() {

                var isValid = true;

                if (isUndefinedOrNullOrEmptyString($scope.DOBmonth) || $scope.DOBmonth === 0) isValid = false;
                if (isUndefinedOrNullOrEmptyString($scope.DOBday) || $scope.DOBday === 0) isValid = false;
                if (isUndefinedOrNullOrEmptyString($scope.DOByear) || $scope.DOByear === 0) isValid = false;

                if (!$scope.employeeContactInformationData.noDriversLicense && $scope.locationUS) {
                    if (isNotUndefinedOrNull($scope.employeeContactInformationData.driversLicenseNumber)) {
                        if ($scope.employeeContactInformationData.driversLicenseNumber.length === 0) isValid = false;
                    } else {
                        isValid = false;
                    }
                }

                if (!$scope.employeeContactInformationData.noDriversLicense && $scope.locationUS) {
                    if (isNotUndefinedOrNull($scope.employeeContactInformationData.licDate)) {
                        if ($scope.employeeContactInformationData.licDate.length === 0) isValid = false;
                    } else {
                        isValid = false;
                    }
                }

                return isValid;

            }

            function setDefaults() {
                //Set default values for fields that can't be null.
                if (isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.addressLine1)) $scope.employeeContactInformationData.addressLine1 = "";
                if (isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.city)) $scope.employeeContactInformationData.city = "";
                if (isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.stateId)) $scope.employeeContactInformationData.stateId = 1;
                if (isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.postalCode)) $scope.employeeContactInformationData.postalCode = "";
                //if (isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.homePhoneNumber)) $scope.employeeContactInformationData.homePhoneNumber = "";

                if (isUndefinedOrNullOrEmptyString($scope.employeeContactInformationData.socialSecurityNumber)) $scope.employeeContactInformationData.socialSecurityNumber = "";

                if (isUndefinedOrNullOrEmptyString($scope.DOBmonth) || isUndefinedOrNullOrEmptyString($scope.DOBday) || isUndefinedOrNullOrEmptyString($scope.DOByear)) {
                    $scope.employeeContactInformationData.birthDate = null;

                } else {
                    $scope.employeeContactInformationData.birthDate = packDOB();
                }

            }

            function savePageData(withoutSoftWarning) {

                BubbleMessageService.hideBubbleMessage();

                var deffered = $q.defer();

                //Check if data is currently in the process of being saved.
                if ($scope.isSaving) {
                    deffered.reject();
                    return deffered.promise;
                }

                //Checck if any required (must have) fields have not been entered.
                if (!validateRequiredFields()) {
                    $scope.isValid=false;
                    deffered.reject();
                    return deffered.promise;
                }

                //Bypass checks if we don't want a soft warning.
                if (!withoutSoftWarning) {
                    //Check if soft warn has been already been displayed.  Otherwise, we need to display.
                    if (!$scope.isSoftWarned && $scope.frmContact.$invalid) {
                        $scope.isSoftWarned = true;
                        BubbleMessageService
                            .showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                        deffered.reject();
                        return deffered.promise;
                    }
                }

                //Check if we need to save the page.
                if (!$scope.frmContact.$dirty) {
                    deffered.resolve($scope.frmContact.$valid);
                    return deffered.promise;
                }

                $scope.isSaving = true;
                $scope.isSoftWarned = false;
                $scope.frmContact.$setPristine();

                //Save data.
                var isWorkflowTaskComplete = $scope.frmContact.$valid;

                MessageService.sending(true);

                //Temp:  We may be able to remove this at a future date.
                setDefaults(); //Set default values for fields required in the table.

                DsOnboardingApi.putEmployeeInfo($scope.employeeContactInformationData)
                    .then(function (data) {

                        //Check for errors.  This should be in a catch.
                        if (data.hasNoError) {
                            $scope.isSaving = false;
                            myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);

                            MessageService.sending(false);
                            deffered.resolve();

                        } else {
                            MessageService.sending(false);
                            MessageService.setMessage("An error has occurred.", MessageService.messageTypes.error);
                            deffered.reject();
                        }
                    });

                return deffered.promise;

            }

            $scope.savePageData = savePageData;

            //Menu Change Started
            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                //Check if the page is dirty and not currently being saved.
                if ($scope.frmContact.$dirty && !$scope.isSaving) {
                    event.preventDefault();

                    savePageData(true).then(function () {
                        DsState.router.go(toState);

                        })
                    .catch(function (isError) {
                            //Messages will be display from savePageData function.

                        }
                    );
                }
            });

            function errorCallBack(error) {
                //MessageService.setMessage("An error has occurred.", MessageService.messageTypes.error);
                $scope.hasError = true;
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
