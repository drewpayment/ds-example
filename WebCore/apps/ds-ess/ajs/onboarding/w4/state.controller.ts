import { isUndefinedOrNullOrEmptyString } from '@util/ds-common';
import { DsOnboardingFormsApiService } from '@ajs/onboarding/forms/ds-onboarding-forms-api.service';
import { DsOnboardingEmployeeApiService } from '@ajs/onboarding/shared/employee-api.service';
import * as $ from 'jquery';
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingW4StateController {

    static readonly $inject = [
        '$scope',
        AccountService.SERVICE_NAME,
        'DsMsg',
        'DsBubbleMessage',
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        '$q',
         DsOnboardingFormsApiService.SERVICE_NAME,
         'CountryStateService'
    ];

    constructor (
        protected $scope,
        accountService: AccountService,
        MessageService,
        BubbleMessageService,
        DsOnboardingApi: DsOnboardingEmployeeApiService,
        protected workflowService: WorkflowService,
        $q,
        DsFormsApi: DsOnboardingFormsApiService,
        locations
    ) {

        function init(userAccount, myWorkflow) {
            let isPageDirty = false;
            let pageDirtyWatchCnt = 0;

            // Used to pass workflow object into workflow footer via directive. [my-workflow]
            // The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;

            $scope.circleNumber = myWorkflow.getCircleNumber();
            let currState = myWorkflow.getCurrState();
            $scope.hasError = false;
            $scope.isLoading = true;
            $scope.savedChanges = false;

            $scope.downloadOriginalForm = downloadOriginalForm;
            $scope.form = null;

            $scope.dirty = false;
            $scope.cnt = 0;
            $scope.isSoftWarned = false;
            // $scope.alreadySaved = false;
            $scope.employeeId = userAccount.employeeId;
            // $scope.StateId = 0;
            $scope.employeeW4StateData = {};
            $scope.employeeW4StateData.isTaxExempt = false;
            $scope.employeeW4StateData.TaxCategory = 2;
            $scope.employeeW4StateData.createDt = null;
            $scope.employeeW4StateData.totalExemptions = 0;
            $scope.stateId = 0;
            $scope.showRenaissanceZone = false;
            $scope.showTaxExemptReason = false;
            $scope.showSchoolDistricts = false;
            $scope.showFilingStatus = true;
            $scope.showWithheldTaxAtLowerRate = false;
            $scope.useAllowancesOrExemptions = 'exemption';

            $scope.employeeW4StateData.isFederalSubtractions = false;
            $scope.employeeW4StateData.federalSubtractions = 0;
            $scope.employeeW4StateData.isPersonalExemption = false;
            $scope.showFederalSubtractions = false;
            $scope.showCountyOfResidence = false;
            $scope.showCountyOfEmployment = false;
            $scope.showAdditionalExemptions = false;
            $scope.showAdditionalCountyWithholding = false;
            $scope.isEmploymentCountyDisabled = false;
            $scope.isResidentCountyDisabled = false;
            $scope.showAllowableDeductionsToFedAdjGrossIncome = false;
            $scope.showIncomeNotSubjectToWithholding = false;
            $scope.showEstimatedItemizedDeductions = false;
            $scope.showEstimatedDeductionAllowances = false;

            $scope.showTotalNumberOfDependentsField = false;
            $scope.showBlindAndAgeFields = false;
            $scope.showSpouseWorksField = false;
            $scope.showIsClaimedAsDependent = false;
            $scope.showIsClaimingSpouseExemption  = false;
            $scope.showAgeAndBlindnessQuestions = false;
            $scope.showIsMarriedFilingSeparately = false;
            $scope.showW4Download = false;
            $scope.showPersonalExemtion = false;
            $scope.showTaxableWagesPercentage = false;
	    $scope.showUseTheChartFromInstructionA = false;
            $scope.showIsNYCResident = false;
            $scope.showIsYonkersResident = false;

            $scope.showStateAdditionalWithholding = true;

            $scope.countyFromContactInfo = '';
            $scope.taxExemptReason = [];
            $scope.reciprocalStates = [];

            $scope.filingStatuses = [{ id: 1, desc: 'Married' }
                                        , { id: 2, desc: 'Single' }
                                        , { id: 3, desc: 'Head of Household' }
            ];
            $scope.taxableWagesPercentages = [{ id: .8, desc: '0.8%' }
                                        , { id: 1.3, desc: '1.3%' }
                                        , { id: 1.8, desc: '1.8%' }
                                        , { id: 2.7, desc: '2.7%' }
                                        , { id: 3.6, desc: '3.6%' }
                                        , { id: 4.2, desc: '4.2%' }
                                        , { id: 5.1, desc: '5.1%' }
            ];

            $scope.instructionAOptionsFromChart = [{ id: 4, desc: 'A' }
                                        , { id: 5, desc: 'B' }
                                        , { id: 6, desc: 'C' }
                                        , { id: 7, desc: 'D' }
                                        , { id: 8, desc: 'E' }
            ];


            let def = myWorkflow.getFormDefinitionId();
            if(def)
            {
                $scope.showW4Download = true;
            }
            else{
                $scope.showW4Download = false;
            }
            let defTaxExmtReason = '';
            $scope.counties = {};

            let isCompleted = isUndefinedOrNullOrEmptyString(myWorkflow.isStateCompleted(currState));
            $scope.isCompleted = isUndefinedOrNullOrEmptyString(isCompleted) || isCompleted === true;
            // console.log($scope.employeeId);

            // load the status of the current employee's onboarding forms
            DsFormsApi.getEmployeeFormStatus(userAccount.employeeId).then(function (forms) {

                if (forms) {
                    for (let i = 0; i < forms.length; i++) {
                        if (forms[i].systemFormType == '1') { // W4
                            $scope.form = forms[i];
                        }
                    }
                }

                $scope.isLoading = false;
            });

            function getOtherData(formType){
                DsOnboardingApi.getEmployeeW4ByTaxCategory(userAccount.employeeId, 2)
                .then(function (data) {
                    if (data !== 'null' && data !== undefined) {
                        getCountiesByState();
                        $scope.employeeW4StateData = data;
                        // console.log(userAccount.employeeId);
                    } else {
                        $scope.employeeW4StateData.stateId = $scope.stateId;
                        $scope.employeeW4StateData.schoolDistrictId = null;
                        $scope.employeeW4StateData.recommendedAllowanceCount = 0;
                        // Set county dropdowns to a value only if indiana state - Indiana state has the field CountyOfResidence
                        if ($scope.showCountyOfResidence) {
                            getCountiesByState();
                            getCountyFromContactInfo();
                        }
                    }

                    if(formType){
                        DsOnboardingApi.getSchoolDistrictsByState(formType.localityId).then(function (schoolDistricts) {
                            if (schoolDistricts != null) {
                                $scope.employeeW4StateData.schoolDistricts = schoolDistricts;
                                let emptySchoolDistrict = { schoolDistrictId: null, name: 'Select a school district', code: null };
                                $scope.employeeW4StateData.schoolDistricts.unshift(emptySchoolDistrict);
                            }
                        });

                        DsOnboardingApi.getReciprocalStatesByStateId($scope.stateId).then(function (reciprocalStates) {
                            if (reciprocalStates != null) {
                                $scope.reciprocalStates = reciprocalStates;
                            }
                        });
                    }

                    DsFormsApi.getFormDefinitionsByNameAndVersion("NY IT-2104", "IT-2104 (2021)").then(function (formDefinitions) {
                       $scope.formDefinitionsNY = formDefinitions;
                    });

                    DsFormsApi.getFormDefinitionsByNameAndVersion("NY IT-2104-E", "IT-2104-E (2021)").then(function (formDefinitions) {
                        $scope.formDefinitionsNYE = formDefinitions;
                     });
 
                    $scope.$watchCollection('employeeW4StateData', function () {
                        if ($scope.cnt > 0) $scope.dirty = true;
                        $scope.cnt++;
                    }, true);
                    DsOnboardingApi.getEmployeeW4TotalExemptions(userAccount.employeeId).then(function (data) {
                        if (!isUndefinedOrNullOrEmptyString(data)) {
                            $scope.employeeW4StateData.totalExemptions = data.w4TotalExemptions;
                            if (!$scope.employeeW4StateData.filingStatus && data.filingStatus) { // If not available on EmployeeOnboardingW4 yet, assign the one from EmployeeOnboardingW4Assist
                                $scope.employeeW4StateData.filingStatus = data.filingStatus;
                            }
                        }
                        $scope.isLoading = false;
                        $scope.hasError = false;
                    });

                });
            }

            if(def){
                DsFormsApi.getFormTypeInfo(def).then(function (formType) {
                    defTaxExmtReason = 'A ' + formType.stateName + ' income tax liability is not expected this year';
                    $scope.taxExemptReason = [{ id: 1, desc: defTaxExmtReason }];

                    $scope.formType = formType;
                    $scope.stateId = formType.localityId;

                    if ($.inArray(15, formType.fieldIds) != -1)
                        $scope.showAllowableDeductionsToFedAdjGrossIncome = true;

                    if ($.inArray(16, formType.fieldIds) != -1)
                        $scope.showIncomeNotSubjectToWithholding = true;

                    if ($.inArray(17, formType.fieldIds) != -1)
                        $scope.showEstimatedItemizedDeductions = true;

                    if ($.inArray(18, formType.fieldIds) != -1)
                        $scope.showEstimatedDeductionAllowances = true;

                    if ($.inArray(23, formType.fieldIds) != -1)
                        $scope.showWithheldTaxAtLowerRate = true;

                    if ($.inArray(11, formType.fieldIds) != -1)
                            $scope.taxExemptReason.push({ id: 2, desc: 'My wages are exempt from withholding' });
                    if ($.inArray(12, formType.fieldIds) != -1)
                            $scope.taxExemptReason.push({ id: 3, desc: 'My permanent home is located in a Renaissance  Zone' });
                    if ($.inArray(13, formType.fieldIds) != -1)
                            $scope.taxExemptReason.push({ id: 4, desc: 'I meet the conditions set forth under the Servicemember Civil Relief Act' });
                    if ($.inArray(14, formType.fieldIds) != -1)
                            $scope.taxExemptReason.push({ id: 5, desc: 'Income is earned as an active duty member of the Armed Forces of the United States' });
                    if ($.inArray(19, formType.fieldIds) != -1)
                        $scope.taxExemptReason.push({ id: 6, desc: 'I qualify for the Fort Campbell Exemption Certificate' });
                    if ($.inArray(20, formType.fieldIds) != -1)
                        $scope.taxExemptReason.push({ id: 7, desc: 'I qualify for the nonresident military spouse exemption' });
                    if ($.inArray(21, formType.fieldIds) != -1)
                        $scope.taxExemptReason.push({ id: 8, desc: 'I work in this state but reside in a reciprocal state' });
                    if ($.inArray(34, formType.fieldIds) != -1)
                        $scope.taxExemptReason.push({ id: 9, desc: 'I am a resident military servicemember who is stationed outside of the state on active duty military orders' });
                    if ($.inArray(35, formType.fieldIds) != -1)
                        $scope.taxExemptReason.push({ id: 10, desc: 'I am a nonresident military servicemember who is stationed in this state due to military orders' });
                    if ($.inArray(38, formType.fieldIds) != -1)
                        $scope.taxExemptReason.push({ id: 11, desc: 'I am a full-time student' });

                        
                    if ($.inArray(1, formType.fieldIds) != -1)
                        $scope.showRenaissanceZone = true;

                    if ($.inArray(2, formType.fieldIds) != -1)
                        $scope.showTaxExemptReason = true;

                    if ($.inArray(3, formType.fieldIds) != -1)
                        $scope.showCountyOfResidence = true;

                    if ($.inArray(4, formType.fieldIds) != -1)
                        $scope.showCountyOfEmployment = true;

                    if ($.inArray(5, formType.fieldIds) != -1)
                        $scope.showAdditionalExemptions = true;

                    if ($.inArray(6, formType.fieldIds) != -1)
                        $scope.showSchoolDistricts = true;

                    if ($.inArray(7, formType.fieldIds) != -1)
                        $scope.useAllowancesOrExemptions = 'Allowance';

                    if ($.inArray(8, formType.fieldIds) != -1)
                        $scope.showAdditionalCountyWithholding = true;

                    if ($.inArray(9, formType.fieldIds) != -1)
                        $scope.showFederalSubtractions = true;

                    if ($.inArray(10, formType.fieldIds) != -1)
                        $scope.showFilingStatus = false;

                    if ($.inArray(24, formType.fieldIds) != -1)
                        $scope.showBlindAndAgeFields = true;

                    if ($.inArray(25, formType.fieldIds) != -1)
                        $scope.showTotalNumberOfDependentsField  = true;

                    if ($.inArray(26, formType.fieldIds) != -1)
                        $scope.showSpouseWorksField  = true;

                    if ($.inArray(27, formType.fieldIds) != -1)
                        $scope.showIsClaimedAsDependent  = true;

                    if ($.inArray(28, formType.fieldIds) != -1)
                        $scope.showIsClaimingSpouseExemption  = true;

                    if ($.inArray(29, formType.fieldIds) != -1)
                        $scope.showAgeAndBlindnessQuestions  = true;

                    if ($.inArray(30, formType.fieldIds) != -1)
                        $scope.showIsMarriedFilingSeparately  = true;

	            if ($.inArray(31, formType.fieldIds) != -1)
	                $scope.showPersonalExemtion  = true;
			
                    if ($.inArray(32, formType.fieldIds) != -1)
                    	$scope.showUseTheChartFromInstructionA  = true;
                
                    if ($.inArray(33, formType.fieldIds) != -1)
                    	$scope.showTaxableWagesPercentage  = true;

	            if ($.inArray(36, formType.fieldIds) != -1)
	                $scope.showIsNYCResident  = true;                

	            if ($.inArray(37, formType.fieldIds) != -1)
	                $scope.showIsYonkersResident  = true;                

                if($scope.stateId==3)
                {
                    $scope.filingStatuses.push({ id: 23, desc: 'Married Filing Separate' });
                }

                
                DsFormsApi.getStateTaxByStateId($scope.stateId).then(function (stateTax) {
                    if (stateTax.blockOverrides && stateTax.blockOverrides == true) {
                        $scope.showStateAdditionalWithholding = false;                        
                    }
                    getOtherData(formType);
                });
            }).catch(getFailed);

        }else{
            DsFormsApi.getFormType(myWorkflow.getFormTypeId()).then(function (formType) {
                $scope.formType = formType;
                $scope.stateId = formType.localityId;

                DsFormsApi.getStateTaxByStateId($scope.stateId).then(function (stateTax) {
                    if (stateTax.blockOverrides && stateTax.blockOverrides == true) {
                        $scope.showStateAdditionalWithholding = false;                        
                    }
                    getOtherData(null);
                });
            });
        }



            function getFailed() {
                alert('An error has occurred.');

            }

            $scope.toggleEmploymentCountyStatus = function () {
                if ($scope.employeeW4StateData.isEmployedInIndiana == true) $scope.employeeW4StateData.employmentCountyId = null;
            };
            $scope.toggleResidentCountyStatus = function () {
                if ($scope.employeeW4StateData.isIndianaResident == true) $scope.employeeW4StateData.countyId = null;

            };

            $scope.clearValuesBasedOnExempt = function () {
                if ($scope.employeeW4StateData.isTaxExempt) {
                    $scope.employeeW4StateData.allowances = null;
                    $scope.employeeW4StateData.isAdditionalAmountWithheld = null;
                    $scope.employeeW4StateData.additionalWithholdingAmt = null;
                    $scope.employeeW4StateData.taxableWagesPercentage = null;

                    $scope.employeeW4StateData.isResidentOfNewYorkCity = false;
                    $scope.employeeW4StateData.isAdditionalNYCAmountWithheld = null;
                    $scope.employeeW4StateData.nycAdditionalWithholdingAmt = null;
                    $scope.employeeW4StateData.nycAllowances = null;

                    $scope.employeeW4StateData.isResidentOfYonkers = false;
                    $scope.employeeW4StateData.isAdditionalYonkersAmountWithheld = null;
                    $scope.employeeW4StateData.yonkersAdditionalWithholdingAmt = null;
                    this.calculateTotal();
                } else {
                    $scope.employeeW4StateData.taxExemptReasonId = null;
                    $scope.employeeW4StateData.taxExemptReason = null;
                    $scope.employeeW4StateData.renaissanceZone = null;
                    $scope.employeeW4StateData.estimatedDeductionAllowances = null;
                    $scope.employeeW4StateData.reciprocalStateId = null;
                    this.calculateTotal();
                }
            };

            $scope.clearValuesBasedOnFilingStatus = function(){
                if($scope.employeeW4StateData.filingStatus != 1 ) {//not married
                    $scope.employeeW4StateData.isSpouseBlind = false;
                    $scope.employeeW4StateData.isSpouseOver65 = false;
                    $scope.employeeW4StateData.isSpouseEmployed = false;
                    $scope.employeeW4StateData.isClaimingSpouseExemption = false;
                    $scope.employeeW4StateData.isMarriedFilingSeparately = false;
                }
            }

            $scope.clearReciprocalStateWhereNeeded = function () {
                if ($scope.employeeW4StateData.taxExemptReasonId != 8)
                    $scope.employeeW4StateData.reciprocalStateId = null;
            };

            $scope.clearValuesBasedOnSubtractions = function () {
                if (!$scope.employeeW4StateData.isFederalSubtractions) {
                    $scope.employeeW4StateData.federalSubtractions = 0;
                }
            };

            $scope.clearAppropriateValuesForNYC = function () {
                if (!$scope.employeeW4StateData.isResidentOfNewYorkCity) {
                    $scope.employeeW4StateData.isAdditionalNYCAmountWithheld = null;
                    $scope.employeeW4StateData.nycAdditionalWithholdingAmt = null;
                    $scope.employeeW4StateData.nycAllowances = null;
                }
                else {
                    $scope.employeeW4StateData.isResidentOfYonkers = false;
                    $scope.employeeW4StateData.isAdditionalYonkersAmountWithheld = null;
                    $scope.employeeW4StateData.yonkersAdditionalWithholdingAmt = null;
                }
            };

            $scope.clearAppropriateValuesForYonkers = function () {
                if (!$scope.employeeW4StateData.isResidentOfYonkers) {
                    $scope.employeeW4StateData.isAdditionalYonkersAmountWithheld = null;
                    $scope.employeeW4StateData.yonkersAdditionalWithholdingAmt = null;
                }
                else {
                    $scope.employeeW4StateData.isResidentOfNewYorkCity = false;
                    $scope.employeeW4StateData.isAdditionalNYCAmountWithheld = null;
                    $scope.employeeW4StateData.nycAdditionalWithholdingAmt = null;
                    $scope.employeeW4StateData.nycAllowances = null;
                }
            };

            $scope.updateRecommendedAdditionalAllownaces = function () {
                let count = 0;
                if ($scope.employeeW4StateData.federalSubtractions && $scope.employeeW4StateData.federalSubtractions > 0) {
                    count = Math.round($scope.employeeW4StateData.federalSubtractions / 1000);
                }

                if ($scope.employeeW4StateData.age && $scope.employeeW4StateData.age > 65) {
                    count++;
                }

                if ($scope.employeeW4StateData.isSpouseOver65) {
                    count++;
                }

                if ($scope.employeeW4StateData.isEmployeeBlind) {
                    count++;
                }

                if ($scope.employeeW4StateData.isSpouseBlind) {
                    count++;
                }

                $scope.employeeW4StateData.recommendedAllowanceCount = count;
            };

            function validRequiredFields() {

                return true;
            }

            function getCountiesByState() {
                locations.getCountiesByState($scope.stateId).then(countiesSuccess).catch(errorCallBack);
            }



            function getCountyFromContactInfo() {
                // Stub
                DsOnboardingApi.getCountyFromContactInfo(userAccount.employeeId).then(function (data) {
                    if (data !== 'null') {
                        $scope.countyFromContactInfo = data.countyName;
                    }

                    for (let i = 0; i < $scope.counties.length; i++) {
                        if ($scope.counties[i].name == $scope.countyFromContactInfo) {
                            $scope.employeeW4StateData.countyId = $scope.counties[i].countyId;

                        }
                    }




                });
            }

            function countiesSuccess(data) {
                $scope.countyDisabled = false;
                $scope.counties = data;
                if ($scope.counties.length === 0) $scope.countyDisabled = true;
            }

            $scope.calculateTotal = function () {
                if ($scope.stateId === 51) {
                    let isClaimedAsDependent: number = 0;
                    let isClaimingSpouseExemption: number = 0;
                    let numberOfDependents: number = 0;
                    
                    if (!$scope.employeeW4StateData.isClaimedAsDependent) isClaimedAsDependent = 1
                    if ($scope.employeeW4StateData.isClaimingSpouseExemption) isClaimingSpouseExemption = 1
                    numberOfDependents = Number($scope.employeeW4StateData.totalNumberOfDependents);
    
                    $scope.employeeW4StateData.allowances = isClaimedAsDependent + isClaimingSpouseExemption + numberOfDependents;
                }
            }



            function savePageData(withoutSoftWarn) {
                let deffered = $q.defer();
                BubbleMessageService.hideBubbleMessage();

                // Validate required fields.
                if (validRequiredFields()) {
                    let isWorkflowTaskComplete = !$scope.frm.$error.required;
                    
                    if (isWorkflowTaskComplete || $scope.isSoftWarned || withoutSoftWarn) {
                        $scope.isSoftWarned = false;

                        MessageService.sending(true);
                        if ($scope.employeeW4StateData.countyId == 0) $scope.employeeW4StateData.countyId = null;
                        if ($scope.employeeW4StateData.employmentCountyId == 0) $scope.employeeW4StateData.employmentCountyId = null;
                        if(!$scope.employeeW4StateData.isAdditionalAmountWithheld) $scope.employeeW4StateData.additionalWithholdingAmt = null;
                        DsOnboardingApi.putEmployeeW4($scope.employeeW4StateData)
                            .then(function (data) {

                                MessageService.sending(false);
                                $scope.frm.$setPristine();

                                // Check for errors.
                                if (data.hasNoError) {
                                    $scope.savedChanges = true;
                                    myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
                                    deffered.resolve(isWorkflowTaskComplete);

                                } else {
                                    MessageService.setMessage('An error has occurred.', MessageService.messageTypes.error);
                                    deffered.reject();
                                }
                            });
                    } else {
                        $scope.isSoftWarned = true;
                        BubbleMessageService.showBubbleMessage('Are you sure you want to continue? You\'ll need to add this info later before you can finalize your self-onboarding.', BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                        deffered.reject();
                    }

                } else {

                    $scope.isSoftWarned = true;
                    MessageService.setMessageWithIcon('Missing required information. Please fill the required fields.', MessageService.messageTypes.error, MessageService.iconTypes.error);
                    deffered.reject();

                }
                return deffered.promise;

            }

            $scope.savePageData = savePageData;

            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
                // Check if page data is dirty.
                if ($scope.frm.$dirty && $scope.savedChanges !== true) {
                    if (validRequiredFields()) {
                        savePageData(true).then(function () {
                        })
                        .catch(function () {
                            event.preventDefault();  // This does nothing but it will catch the rejection.
                        }
                    );

                    } else {
                        MessageService.sending(false);
                        event.preventDefault();
                    }
                }

            });

            function errorCallBack() {
                MessageService.setMessage('An error has occurred.', MessageService.messageTypes.error);
                $scope.hasError = true;
            }

            function clearErrors() {
                $scope.hasError = false;
            }

            /**
             * Downloads the saved version of a form.
             * @param {Object} form - The form to download.
             */
            function downloadOriginalForm(form) {
                //Special handling for NYW4 as it requires a different form when EXEMPT is chosen
                if (!isUndefinedOrNullOrEmptyString($scope.formDefinitionsNY) && !isUndefinedOrNullOrEmptyString($scope.formDefinitionsNYE)) {
                    if (form.formDefinitionId == $scope.formDefinitionsNYE.formDefinitionId && form.formTypeId == $scope.formDefinitionsNYE.formTypeId && !$scope.employeeW4StateData.isTaxExempt) {
                        form.formDefinitionId = $scope.formDefinitionsNY.formDefinitionId;
                        form.formTypeId = $scope.formDefinitionsNY.formTypeId;
                    }

                    if (form.formDefinitionId == $scope.formDefinitionsNY.formDefinitionId && form.formTypeId == $scope.formDefinitionsNY.formTypeId && $scope.employeeW4StateData.isTaxExempt) {
                        form.formDefinitionId = $scope.formDefinitionsNYE.formDefinitionId;
                        form.formTypeId = $scope.formDefinitionsNYE.formTypeId;
                    }
                }

                DsFormsApi
                    .downloadOriginalForm(form);
                // .catch(msgs.showWebApiException);
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
