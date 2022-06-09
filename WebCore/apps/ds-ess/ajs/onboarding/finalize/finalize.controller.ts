import { STATES } from '../../shared/state-router-info';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { DsApiCommonService } from '@ajs/core/api/ds-api-common.provider';
import { DsOnboardingFormsApiService } from '@ajs/onboarding/forms/ds-onboarding-forms-api.service';
import { DsOnboardingEmployeeApiService } from '@ajs/onboarding/shared/employee-api.service';
import { DsOnboardingAdminApiService } from "@ajs/onboarding/shared/ds-admin-api.service";
import { DsNavigationService } from '../../ui/nav/ds-navigation.service';
import { DsResourceApi } from '@ajs/core/ds-resource/ds-resource-api.service';
import { AccountService } from '@ajs/core/account/account.service';
import * as angular from 'angular';
import { isUndefinedOrNullOrEmptyString } from '@util/ds-common';
import { DataRowOutlet } from '@angular/cdk/table';
import { coerceNumberProperty } from '@angular/cdk/coercion';
import { ClientEssOptions } from 'apps/ds-company/ajs/models/client-ess-options.model';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingFinalizeController {
    static readonly $inject = [
        "$scope",
        "$window",
        DsMsgService.SERVICE_NAME,
        DsApiCommonService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        '$q',
        DsOnboardingFormsApiService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        DsOnboardingAdminApiService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        DsResourceApi.SERVICE_NAME,
        AccountService.SERVICE_NAME,
    ];
    constructor(
        protected $scope: ng.IScope,
        $window,
        msgs,
        api,
        protected workflowService: WorkflowService,
        $q,
        formsApi,
        DsOnboardingApi,
	DsOnboardingAdminApi, 
        DsNavigationService,
        DsResourceApiSevice,
        accountSvc: AccountService
    ) {
        function init(userAccount, myWorkflow) {
            $scope.nav = DsNavigationService;
            let formsToSave = [];
            let isPageDirty = false;
            let currState = myWorkflow.getCurrState();
            // Used to pass workflow object into workflow footer via directive. [my-workflow]
            // The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;
            // $scope.circleNumber = myWorkflow.getCircleNumber();
            $scope.EmployeeOnboardingCompletionStatus = myWorkflow.getUserWorkflow();
            $scope.userData = {};
            $scope.hasError = false;
            $scope.signature = {};
            $scope.savePageData = savePageData;
            $scope.previewForm = previewForm;
            $scope.previewDocument = previewDocument;
            $scope.redirectToDominionSourceHome = redirectToDominionSourceHome;
            $scope.save = save;
            $scope.downloadForm = downloadForm;
            $scope.hasUnsignedForm = hasUnsignedForm;
            $scope.agree = false;
            $scope.btnUrl = '';
            $scope.signature.signeeFirstName = userAccount.firstName;
            $scope.signature.signeeMiddle = userAccount.middleInitial;
            $scope.signature.signeeLastName = userAccount.lastName;
            $scope.signature.signeeInitials = $scope.signature.signeeFirstName.charAt(0).toUpperCase() + $scope.signature.signeeLastName.charAt(0).toUpperCase();
            $scope.isOnboardingFinalizeCompleteBool = false;
            $scope.isListAShow = true;
            $scope.IsW4StateRequired = true;
            $scope.IsW4FederalRequired = true;
            $scope.IsI9Required = true;
            $scope.isAllWorkFlowCompleted = myWorkflow.isAllWorkFlowCompleted();
            $scope.documents = [];
            $scope.isSubmitted = false;
            $scope.legacyHomepageLink = { url: '/' };
            $scope.finalDisclaimerMessage = "By selecting the checkbox and clicking the 'Complete Onboarding' button, you agree that you have reviewed the documents under Preview Employment Forms and confirm that the information within them is accurate.";
            $scope.finalDisclaimerAgreementText = "Under penalties of perjury, I declare that I have answered all previous questions to the best of my knowledge and belief, and they are true, correct, and complete.";

            // CONROLLER INIT
            $scope.isLoading = true;
            msgs.loading(true);
            DsOnboardingApi.getEmployeeW4ByTaxCategory(userAccount.employeeId, 2).then(function (dataState) {
                if (dataState == 'null') {
                    $scope.IsW4StateRequired = false;
                }

                return DsOnboardingApi.getEmployeeW4ByTaxCategory(userAccount.employeeId, 1);
            }).then(function (dataFederal) {
                if (dataFederal == 'null') {
                    $scope.IsW4FederalRequired = false;
                }
                return DsOnboardingApi.getEmployeeI9(userAccount.employeeId);
            }).then(function (i9Data) {
                if (i9Data.i9EligibilityStatusId == 0) {
                    $scope.IsI9Required = false;
                }
                return formsApi.getEmployeeFormStatus(userAccount.employeeId);  // old way of finding isOnboardingComplete based on forms
            }).then(function (forms) {
                $scope.forms = forms;
                return DsOnboardingApi.getEmployeeFinalizeStatus(userAccount.employeeId);
            }).then(function (finalizeStatus) {
                $scope.isOnboardingFinalizeCompleteBool = finalizeStatus.data.isFinalizeComplete;
                return formsApi.getUserUploadedDocuments(userAccount.clientId, userAccount.employeeId);
            }).then(function (documents) {
                $scope.documents = documents;
                $scope.isLoading = false;
                msgs.loading(false);
            });

            // DsOnboardingApi.getEmployeeOnboardingCompletionStatus(userAccount.employeeId).then(function (completionStatusData) {
            //    $scope.EmployeeOnboardingCompletionStatus = completionStatusData.data;
            // });

            // get links to the legacy system to render in the header
            accountSvc.getAccessibleLegacyLinks().then(linksLoaded)['catch'](msgs.showWebApiException);

            DsOnboardingApi.getEmployeeHireDate(userAccount.employeeId).then(function (emp) {
                $scope.employeeHireDate = !isUndefinedOrNullOrEmptyString(emp.data.rehireDate) ? emp.data.rehireDate : emp.data.hireDate;
            });

            DsOnboardingApi.getI9DocumentList().then(function (data) {
                // GEt I9 List A B C
                const items = $scope.i9DocumentsList = data;
                items.forEach(item => {
                    const index = item.name.split('.')[0];
                    item.index = coerceNumberProperty(index);
                });
            });

            DsOnboardingApi.getEmployeeOnboardingField(userAccount.employeeId, 1)
                .then(function (employeeOnboardingField) {
                    $scope.prefStatusId = employeeOnboardingField.decimalValue;
                });

            DsOnboardingApi.getEmployeeHireDate(userAccount.employeeId).then(function (emp) {
                $scope.employeeHireDate = !isUndefinedOrNullOrEmptyString(emp.data.rehireDate) ? emp.data.rehireDate : emp.data.hireDate;
            });

            DsOnboardingAdminApi.getClientEssOptions(userAccount.clientId).then(function (essOptions) {
                if (essOptions) {
                    if (essOptions.finalDisclaimerMessage && essOptions.finalDisclaimerMessage.trim() != '') 
                        $scope.finalDisclaimerMessage = essOptions.finalDisclaimerMessage;

                    if (essOptions.finalDisclaimerAgreementText && essOptions.finalDisclaimerAgreementText.trim() != '') 
                        $scope.finalDisclaimerAgreementText = essOptions.finalDisclaimerAgreementText;
                }              
            });



            /////////////////////////////////////////////////////////
            // $scope METHODS
            /////////////////////////////////////////////////////////
            $scope.goToMenu = function (menu) {
                myWorkflow.goWorkFlowByMenu(menu);
            };

            $scope.addBorderBottomForLast = function ($last) {
                if ($last === true) {
                    return { 'border-bottom': '1px solid lightgrey' };
                }
            };

            $scope.employeeEndsOnboarding = function () {
                DsOnboardingApi.employeeEndsOnboarding(userAccount.employeeId).then(function (data) {
                });
            };

            function savePageData() {
                // noop
            }

            /**
             * Returns true if at least one form has not been saved/signed yet.
             * @returns {boolean} - True, if at least one form has not been saved/signed yet.
             */
            function hasUnsignedForm() {
                let hasUnsigned = false;
                angular.forEach($scope.forms, function (f) {
                    if (!f.formId) {
                        hasUnsigned = true;
                    } else {
                        $scope.isOnboardingComplete = true;
                    }
                });
                return hasUnsigned;
            }

            /**
             * Downloads a preview version of the specified form.
             * @param {Object} form - Form object to get a preview of.
             */
            function previewForm(form) {

                msgs.clearMessage();

                formsApi
                    .downloadFormPreview(form)
                    .catch(msgs.showWebApiException);
            }

            function previewDocument(document) {

                msgs.clearMessage();

                DsResourceApiSevice.downloadResource(document.resourceId, true, true);
            }

            /**
             * Downloads the saved version of a form.
             * @param {Object} form - The form to download.
             */
            function downloadForm(form) {
                formsApi
                    .downloadForm(form.formId)
                    .catch(msgs.showWebApiException);
            }

            function addAdminTask() {
                let newTaskDto = <any>{};

                newTaskDto.employeeId = userAccount.employeeId;
                newTaskDto.description = 'Assign ' + userAccount.firstName + ' ' + userAccount.lastName + ' a Paycard';
                newTaskDto.sequence = 1;
                DsOnboardingApi.addAdminTask(newTaskDto).then(function (result) {

                });

            }

            function redirectToDominionSourceHome() {
                window.location.href = $scope.legacyHomepageLink.url;
            }

            /**
             * Loads the legacy links onto the scope to be displayed in the header.
             *
             * @function
             * @private
             */
            function linksLoaded(links) {
                angular.forEach(links, function (link) {
                    // grab the homepage link
                    if (link.isHomepage)
                        $scope.legacyHomepageLink = link;
                });
            }

            /**
             * Applies the current employee signature to each unsigned form and then saves the newly signed forms.
             */
            function save() {
                $scope.isSubmitted = true;
                msgs.loading();

                // sign any unsigned forms
                angular.forEach($scope.forms, function (f) {
                    if (!f.formId) {
                        signForm(f);
                        formsToSave.push(f);
                    }
                });

                // save the newly signed forms via the API
                formsApi
                    .saveForms(userAccount.employeeId, formsToSave)
                    .then(function (updatedForms) {

                        for (let i = 0; i < $scope.forms.length; i++) {
                            for (let j = 0; j < updatedForms.length; j++) {
                                if ($scope.forms[i].formDefinitionId === updatedForms[j].formDefinitionId)
                                    $scope.forms[i] = updatedForms[j];
                            }
                        }

                        DsOnboardingApi.employeeEndsOnboarding(userAccount.employeeId).then(function (data) {
                            DsOnboardingApi.getEmployeeFinalizeStatus(userAccount.employeeId).then( function (finalizeStatus) {
                                $scope.isOnboardingFinalizeCompleteBool = finalizeStatus.data.isFinalizeComplete;
                                myWorkflow.updateWorkflowTask(currState, $scope.isOnboardingFinalizeCompleteBool);
                                msgs.setTemporarySuccessMessage('Your onboarding forms were signed successfully.');
                            });
                        }).catch((err) => {
                            $scope.isSubmitted = false;
                            msgs.showWebApiException (err);
                        });

                        if (!isUndefinedOrNullOrEmptyString($scope.prefStatusId) && $scope.prefStatusId == 3) {
                            addAdminTask();
                        }
                    })
                    .catch((err) => {
                        $scope.isSubmitted = false;
                        msgs.showWebApiException (err);
                    });

                // clear the forms to save
                formsToSave = [];


            }
            $scope.fnListAShow = function () {
                $scope.isListAShow = true;
                // console.log('Show');

            };

            $scope.fnListAHide = function () {
                $scope.isListAShow = false;
                // console.log('Show');
            };
            $scope.toggleListA = function () {

                $scope.isListAShow = true;
            };
            $scope.toggleListBAndC = function () {

                $scope.isListAShow = false;
            };

            $scope.filterInComplete = function (item) {
                return item.isCompleted == null || item.isCompleted == false;
            };

            // for each given document, if there is a form where the docs resourceId match the forms formId, hide the document,
            // as the document should not be associated with the users onboarding if they have filled out an onboarding form for that workflow item.
            $scope.hideDocumentsWithMatchingFormId = function (doc) {
                let hideDoc = false;
                $scope.forms.forEach(form => {
                    if (form.formId == doc.resourceId) {
                        hideDoc = true;
                    }
                });
                return hideDoc; 
            };

            // works with the above function to make sure a form can be previewed, even if it has a formId.
            // This case occurs when there is a matching form and document, because the workflow and documents system are decoupled, the document
            // will not attach to the workflow object and a form will still need to be submitted, and this is the correct way to preview said form.
            $scope.hidePreviewFormButtonForFormsWithoutMatchingDocument = function (form) {
                let hidePreviewButton = true;
                $scope.documents.forEach(doc => {
                    if ( doc.resourceId == form.formId ) {
                        hidePreviewButton = false;
                    }
                });
                return hidePreviewButton;
            };

            $scope.replaceWildCards = function(message:string){
                var currDate = new Date();
                var startDate = new Date();
                startDate.setDate(startDate.getDate() + 7);

                message = message.replace(/\{\*EmployeeName\}/g, $scope.signature.signeeFirstName +  ' ' + $scope.signature.signeeLastName);
                message = message.replace(/\{\*Date\}/g, currDate.toLocaleDateString());
                message = message.replace(/\{\*CompanyName\}/g, userAccount.clientName || userAccount.lastClientName );
                message = message.replace(/\{\*StartDate\}/g, startDate.toLocaleDateString());
                return message.replace(/\n/g, '<br/>');
            }
    
            ///////////////////////////////////////////////////////////////////
            // PRIVATE METHODS
            ///////////////////////////////////////////////////////////////////

            /**
             * Applies the current employee signature to a form's 'employee' signature.
             * @param {Object} form - The form to sign.
             */
            function signForm(form) {

                // add the current timestamp as the signature date
                $scope.signature.signatureDate = api.toApiDateTimeString(Date.now());

                // set the signature's full name text from the individual name fields
                $scope.signature.signatureName = $scope.signature.signeeFirstName;
                if (!!$scope.signature.signeeMiddle)
                    $scope.signature.signatureName += ' ' + $scope.signature.signeeMiddle;
                $scope.signature.signatureName += ' ' + $scope.signature.signeeLastName;

                // apply the signature to the 'employee' role
                angular.forEach(form.signatures, function (s) {
                    if (s.roleIdentifier === 'employee') {
                        angular.extend(s, $scope.signature);
                    }
                });
            }
        }

        accountSvc.getUserInfo().then(user => {
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
