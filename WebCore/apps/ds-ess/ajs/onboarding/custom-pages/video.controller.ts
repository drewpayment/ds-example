﻿import * as angular from "angular";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { STATES } from "../../shared/state-router-info";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { isUndefinedOrNullOrEmptyString } from "@util/ds-common";
import { DsResourceApi } from "@ajs/core/ds-resource/ds-resource-api.service";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingVideoController {
    static readonly $inject = [
        "$scope",
        "$sce",
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        DsResourceApi.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        DsStateService.SERVICE_NAME,
        '$q'
    ];
    constructor (
        protected $scope: ng.IScope,
        $sce,
        accountService: AccountService,
        MessageService,
        BubbleMessageService,
        DsOnboardingApi,
        DsResourceApi,
        protected workflowService: WorkflowService,
        DsState,
        $q
    ) {
        function init(myWorkflow) {
            //Used to pass workflow object into workflow footer via directive. [my-workflow]
            //The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
            $scope.myWorkflow = myWorkflow;

            $scope.circleNumber = myWorkflow.getCircleNumber();
            $scope.currWorkflowTitle = myWorkflow.getWorkFlowTitle();

            //$scope.isLoaded = false;

            $scope.selectedResources = [];
            $scope.previewResource = "";
            $scope.signatureDescription = "";
            $scope.considerSignatureDescription = true;
            $scope.description = myWorkflow.getWorkFlowDescription();
            if (isUndefinedOrNullOrEmptyString($scope.description))
                $scope.description = "Please review the video(s) below to continue.";

            var currState = myWorkflow.getCurrState();
            var isCompleted = function () {
                var workFlowValue = myWorkflow.isStateCompleted(currState);
                return isUndefinedOrNullOrEmptyString(workFlowValue) ? false : workFlowValue === true;
            }();

            $scope.document = { isCompleted: isCompleted };

            $scope.isSaving = false;
            $scope.isSoftWarned = false;
            $scope.isLoaded = false;

            if(DsState.params.workFlowTaskId)
            DsOnboardingApi.getOnboardingWorkflowTaskByTaskId(DsState.params.workFlowTaskId)
                .then(function (data) {
                    $scope.selectedResources = data.resources;
                    if (!$.isEmptyObject($scope.selectedResources)) {
                        $scope.previewResource = $sce.trustAsResourceUrl($scope.selectedResources[0].source);
                        $scope.selectedResources[0].previewResourceCssClass = true;
                        $scope.signatureDescription = data.signatureDescription;
                        $scope.considerSignatureDescription = isUndefinedOrNullOrEmptyString($scope.signatureDescription) ? false : true;
                    }
                    $scope.isLoaded = true;
                });

            $scope.setPreviewResource = function setPreviewResource(selectedResource) {
                if (!isUndefinedOrNullOrEmptyString(selectedResource.resourceId)) {
                    $scope.previewResource = $sce.trustAsResourceUrl(selectedResource.source);
                    setActiveResource(selectedResource);
                }
            }

            function setActiveResource(activeResource) {
                angular.forEach($scope.selectedResources, function (currResource, index) {
                    if (currResource.resourceId == activeResource.resourceId) {
                        currResource.previewResourceCssClass = true;
                    }
                    else {
                        currResource.previewResourceCssClass = false;
                    }
                });
            }

            function savePageData(withoutSoftWarning) {
                MessageService.clearMessage();
                BubbleMessageService.hideBubbleMessage();

                var deffered = $q.defer();

                //Check if data is currently in the process of being saved.
                if ($scope.isSaving) {
                    deffered.reject();
                    return deffered.promise;
                }

                if (!$scope.considerSignatureDescription && !$scope.document.isCompleted) {
                    $scope.document.isCompleted = true;
                    $scope.frmVideo.$dirty = true;
                }

                if (!withoutSoftWarning) {
                    //Check if soft warn has been already been displayed.  Otherwise, we need to display.
                    if (!$scope.isSoftWarned && !$scope.document.isCompleted) {
                        $scope.isSoftWarned = true;
                        BubbleMessageService
                            .showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                        deffered.reject();
                        return deffered.promise;
                    }
                }

                var isWorkflowTaskComplete = $scope.document.isCompleted;
                //Check if we need to save the page.
                if (!$scope.frmVideo.$dirty) {
                    deffered.resolve(isCompleted);
                    return deffered.promise;
                }

                $scope.isSaving = true;
                $scope.isSoftWarned = false;
                $scope.frmVideo.$setPristine();

                //Save data.
                MessageService.sending(true);
                myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
                $scope.isSaving = false;
                MessageService.sending(false);
                deffered.resolve();
                return deffered.promise;
            }

            $scope.savePageData = savePageData;

            //Menu Change Started
            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                //Check if the page is dirty and not currently being saved.
                if ($scope.frmVideo.$dirty && !$scope.isSaving) {
                    event.preventDefault();

                    savePageData(true).then(function () {
                        DsState.router.go(toState, toParams);

                    })
                    .catch(function (isError) {
                        //Messages will be display from savePageData function.

                    }
                    );
                }
            });

            function errorCallBack() {
                MessageService.setMessage("An error has occurred.", MessageService.messageTypes.error);
            }

            $scope.changeSignatureState = function changeSignatureState($event) {
                $scope.frmVideo.$setDirty();
            }
        }

        workflowService.getUserWorkflowPromise().then(workflow => {
            init(workflow);
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