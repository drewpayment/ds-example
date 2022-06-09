import * as angular from 'angular';
import { STATES } from '../../shared/state-router-info';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { DsBubbleMessageService } from '@ajs/core/msg/ds-bubbleMsg.service';
import { DsOnboardingEmployeeApiService } from '@ajs/onboarding/shared/employee-api.service';
import { DsStateService } from '@ajs/core/ds-state/ds-state.service';
import { isUndefinedOrNullOrEmptyString } from '@util/ds-common';
import { DsResourceApi } from '@ajs/core/ds-resource/ds-resource-api.service';
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';
import { IUserInfo } from '@ajs/user';

export class OnboardingDocumentController {
    static readonly $inject = [
        '$scope',
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        DsResourceApi.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        DsStateService.SERVICE_NAME,
        '$q',
        'Upload'
    ];

    urlChangeListener: any;
    isCompleted: boolean;

    // workflowservice.currState
    currState: any;
    userAccount: IUserInfo;
    direction: number;

    constructor (
        private $scope: ng.IScope,
        private accountService: AccountService,
        private MessageService: DsMsgService,
        private BubbleMessageService: DsBubbleMessageService,
        private DsOnboardingApi,
        private DsResourceApi,
        private myWorkflow: WorkflowService,
        private DsState,
        private $q: ng.IQService,
        private upload
    ) {
        this.addUrlChangeListener();
        this.preInit();
    }

    preInit() {
        this.accountService.getUserInfo().then(user => {
            this.userAccount = user;
            this.myWorkflow.getUserWorkflowPromise().then(_ => {
                this.afterPreInit();
            });
        });
    }

    afterPreInit() {
        // Used to pass workflow object into workflow footer via directive. [my-workflow]
        // The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
        this.$scope.myWorkflow = this.myWorkflow;

        this.$scope.circleNumber = this.myWorkflow.getCircleNumber();
        this.$scope.currWorkflowTitle = this.myWorkflow.getWorkFlowTitle();

        // this.$scope.isLoaded = false;

        this.$scope.selectedResources = [];
        this.$scope.previewResource = '';
        this.$scope.signatureDescription = '';
        this.$scope.considerSignatureDescription = true;
        this.$scope.userMustUpload = false;
        this.$scope.uploadDescription = '';
        this.$scope.existingResourceId = 0;
        this.$scope.description = this.myWorkflow.getWorkFlowDescription();
        if (isUndefinedOrNullOrEmptyString(this.$scope.description))
            this.$scope.description = 'Please review the document(s) below to continue.';

        this.currState = this.myWorkflow.getCurrState();
        this.isCompleted = isUndefinedOrNullOrEmptyString(this.myWorkflow.isStateCompleted(this.currState))
            ? false : this.myWorkflow.isStateCompleted(this.currState) === true;

        this.$scope.document = {
            isCompleted: this.isCompleted,
            isUploaded: this.isCompleted,
            isResourceExists: false,
            currentAttachment: { name: '' },
            showFileName: false
        };

        this.$scope.isSaving = false;
        this.$scope.isSoftWarned = false;
        this.$scope.isLoaded = false;
        this.$scope.fileAccept = '.PDF,.DOC,.DOCX,.XLS,.XLSX,.TXT,.RTF';

        if(this.DsState.params.workFlowTaskId)
        this.DsOnboardingApi.getOnboardingWorkflowTaskByTaskId(this.DsState.params.workFlowTaskId)
            .then((data) => {
                this.$scope.selectedResources = data.resources;
                this.$scope.userMustUpload = data.userMustUpload;
                this.$scope.uploadDescription = data.uploadDescription;

                if (this.$scope.userMustUpload && data.userMustUploadResource) {
                    this.$scope.document.isUploaded = true;
                    this.$scope.document.showFileName = true;
                    this.$scope.document.isResourceExists = true;
                    this.$scope.existingResourceId = data.userMustUploadResource.resourceId;
                    this.$scope.document.currentAttachment.name = data.userMustUploadResource.name;
                }
                if (!$.isEmptyObject(this.$scope.selectedResources)) {
                    this.$scope.previewResource = 'api/resources/' + this.$scope.selectedResources[0].resourceId + '?isDownload=false&fromESS=true';
                    this.$scope.selectedResources[0].previewResourceCssClass = true;
                    this.$scope.signatureDescription = data.signatureDescription;
                    this.$scope.considerSignatureDescription = isUndefinedOrNullOrEmptyString(this.$scope.signatureDescription) ? false : true;
                }
                this.$scope.isLoaded = true;
            });

        this.$scope.download = (resource) => {
            this.DsResourceApi.downloadResource(resource.resourceId);
        };

        this.$scope.setPreviewResource = (selectedResource) => {
            if (!isUndefinedOrNullOrEmptyString(selectedResource.resourceId)) {
                this.$scope.previewResource = 'api/resources/' + selectedResource.resourceId + '?isDownload=false&fromESS=true';
                this.setActiveResource(selectedResource);
            }
        };

        this.$scope.fileAccept = '.PDF,.DOC,.DOCX,.XLS,.XLSX,.TXT,.RTF';

        this.$scope.fileChanges = (file) => {
            // this.$scope.hasFiles = file && file.length > 0;
            // if (this.$scope.hasFiles) {
            if (file && file.length > 0) {
                let extension = file[0].name.substr((file[0].name.lastIndexOf('.') + 1)).toLowerCase();
                let allowedFiles = ['pdf', 'doc', 'docx', 'xlsx', 'xls', 'txt', 'rtf'];

                if (!!extension && _.some(allowedFiles, f => f === extension)) {
                    this.$scope.document.isUploaded = true;

                    this.$scope.document.showFileName = true;
                    if (!this.$scope.document.isResourceExists) {
                        this.$scope.existingResourceId = 0;
                    }
                    this.$scope.document.isResourceExists = false;
                    this.$scope.document.currentAttachment.name = this.$scope.document.currentAttachment.data.name;
                } else {
                    this.$scope.document.isUploaded = false;
                    this.$scope.document.showFileName = false;
                    file = null;
                    this.$scope.document.currentAttachment.name = '';

                    return;
                }
            }
        };

        this.$scope.savePageData = this.savePageData;

        // Menu Change Started
        this.$scope.$on('$stateChangeStart', (event, toState, toParams, fromState, fromParams) => {

            // Check if the page is dirty and not currently being saved.
            if (this.$scope.frmDocument.$dirty && !this.$scope.isSaving) {
                event.preventDefault();

                this.savePageData(true).then(() => {
                    this.DsState.router.go(toState, toParams);

                })
                .catch((isError) => {
                    // Messages will be display from savePageData function.

                }
                );
            }
        });

        this.$scope.changeSignatureState = ($event) => {
            this.$scope.frmDocument.$setDirty();
        };
    }

    setActiveResource(activeResource) {
        angular.forEach(this.$scope.selectedResources, (currResource, index) => {
            if (currResource.resourceId == activeResource.resourceId) {
                currResource.previewResourceCssClass = true;
            } else {
                currResource.previewResourceCssClass = false;
            }
        });
    }

    addUrlChangeListener() {
        window.addEventListener('locationchange', (event) => {
            this.preInit();
        });
    }

    clickSavePageData(direction: number) {
        this.direction = direction;
        this.savePageData(false).then(isWorkflowTaskComplete => {
            const currState = this.myWorkflow.getCurrState();

            if (isWorkflowTaskComplete) {
                this.myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
            }

            this.myWorkflow.getNextPrevPage(currState, this.direction);
        });
    }

    savePageData(withoutSoftWarning: boolean): ng.IPromise<any> {
        const deffered = this.$q.defer<any>();

        // Check if data is currently in the process of being saved.
        if (this.$scope.isSaving) {
            deffered.reject();
            return deffered.promise;
        }

        if (!this.$scope.considerSignatureDescription && !this.$scope.document.isCompleted) {
            this.$scope.document.isCompleted = true;
            this.$scope.frmDocument.$dirty = true;
        }

        if (!this.$scope.userMustUpload && !this.$scope.document.isUploaded) {
            this.$scope.document.isUploaded = true;
            this.$scope.frmDocument.$dirty = true;
        }

        if (!withoutSoftWarning) {
            // Check if soft warn has been already been displayed.  Otherwise, we need to display.
            if (!this.$scope.isSoftWarned &&
                (!this.$scope.document.isCompleted ||
                    (this.$scope.userMustUpload && !this.$scope.document.isUploaded))
            ) {
                this.$scope.isSoftWarned = true;
                this.BubbleMessageService
                    .showBubbleMessage(
                        `Are you sure you want to continue? You\'ll need to add this info
                         later before you can finalize your self-onboarding.`,
                        this.BubbleMessageService.messageTypes.warning,
                        this.BubbleMessageService.iconTypes.info,
                        'input[value="Continue"]'
                        );
                deffered.reject();
                return deffered.promise;
            }
        }

        const isWorkflowTaskComplete = this.$scope.document.isCompleted &&
            (!this.$scope.userMustUpload || this.$scope.document.isUploaded);

        // Check if we need to save the page.
        if (!this.$scope.frmDocument.$dirty) {
            deffered.resolve(this.isCompleted);
            return deffered.promise;
        }

        this.$scope.isSaving = true;
        this.$scope.isSoftWarned = false;
        this.$scope.frmDocument.$setPristine();

        // Save data.
        this.MessageService.sending(true);

        if (this.$scope.userMustUpload && this.$scope.document.isUploaded && !this.$scope.document.isResourceExists) {
            this.upload.upload({
                url: 'api/employeeAttachment/uploadUserOnboardingWorkFlowTaskDocument',
                data: {
                    file: this.$scope.document.currentAttachment.data,
                    'employeeId': this.userAccount.employeeId,
                    'clientId': this.userAccount.clientId,
                    'resourceId': this.$scope.existingResourceId,
                    'workflowTaskId': this.DsState.params.workFlowTaskId,
                    'uploadFilename': this.$scope.currWorkflowTitle + ' upload'
                }
            }).then(() => {
                this.updateWorkFlowTask(isWorkflowTaskComplete, () => deffered.resolve());
            });
        } else {
            this.updateWorkFlowTask(isWorkflowTaskComplete, () => deffered.resolve());
        }

        return deffered.promise;
    }

    updateWorkFlowTask(isWorkflowTaskComplete: boolean, callback: () => void) {
        this.myWorkflow.updateWorkflowTask(this.currState, isWorkflowTaskComplete);
        this.$scope.isSaving = false;
        this.MessageService.sending(false);
        callback();
    }

}
