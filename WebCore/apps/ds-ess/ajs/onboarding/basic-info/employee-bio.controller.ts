import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { isUndefinedOrNullOrEmptyString } from "@util/ds-common";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';
import { DsResourceApi } from '@ajs/core/ds-resource/ds-resource-api.service';
import { IUserInfo } from '@ajs/user/user-info';

export class OnboardingEmployeeBioController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        "$q",
        DsBubbleMessageService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        DsResourceApi.SERVICE_NAME,
    ];


    constructor (
        protected $scope: ng.IScope,
        accountService: AccountService,
        MessageService: DsMsgService,
        protected workflowService: WorkflowService,
        $q: ng.IQService,
        BubbleMessageService,
        dsOnboardingApi: DsOnboardingEmployeeApiService,
        resourceApi: DsResourceApi,
    ) {
        function init(userAccount: IUserInfo, myWorkflow, employeeImage) {
            $scope.hasError = false;
            $scope.isLoading = false;
            $scope.isSoftWarned = false;
            $scope.myWorkflow = myWorkflow;
            $scope.userData = userAccount;
            $scope.circleNumber = myWorkflow.getCircleNumber();

            var currState = myWorkflow.getCurrState();
            var isCompleted = myWorkflow.isStateCompleted(currState);
            $scope.isCompleted = isCompleted;
            $scope.employeeDetail = null;
            $scope.employeeImage = employeeImage;
            $scope.allowImageUpload = false;

            dsOnboardingApi.getClientEssOptions(userAccount.clientId).then(function (essOptions) {
                $scope.allowImageUpload = essOptions.allowImageUpload;
            });

            dsOnboardingApi.getEmployeePersonalInfo(userAccount.employeeId).then(function (data) {
                if (!isUndefinedOrNullOrEmptyString(data)) {
                    $scope.employeePersonalInfo = data;
                }
                else {
                    $scope.employeePersonalInfo = {
                        employeeId: userAccount.employeeId,
                        bio: null
                    };
                }


            });

            function saveEmployeeBioData(withoutSoftWarning) {

                BubbleMessageService.hideBubbleMessage();
                var deffered = $q.defer();

                if (!withoutSoftWarning) {

                    if (!$scope.isSoftWarned && ($scope.frmEmployeeBio.$invalid )) {
                        $scope.isSoftWarned = true;
                        BubbleMessageService
                            .showBubbleMessage("Are you sure you want to continue? You'll need to add this info later before you can finalize your self-onboarding.", BubbleMessageService.messageTypes.warning, BubbleMessageService.iconTypes.info, 'input[value="Continue"]');
                        deffered.reject();
                        return deffered.promise;
                    }
                }

                if (!$scope.frmEmployeeBio.$dirty) {
                    deffered.resolve($scope.frmEmployeeBio.$valid);
                    return deffered.promise;
                }

                var isWorkflowTaskComplete = false;
                if (!$scope.frmEmployeeBio.$invalid)
                    isWorkflowTaskComplete = true;

                MessageService.sending(true);

                dsOnboardingApi.putEmployeePersonalInfo($scope.employeePersonalInfo)
                    .then(function (result) {

                        //Check for errors.  This should be in a catch.
                        if (result.hasNoError) {
                            myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
                            MessageService.sending(false);
                            $scope.employeePersonalInfo = result.data;
                            deffered.resolve();

                        } else {
                            MessageService.sending(false);
                            MessageService.setMessage("An error has occurred.", MessageService.messageTypes.error);
                            deffered.reject();
                        }
                    });

                return deffered.promise;

            }

            $scope.saveEmployeeBioData = saveEmployeeBioData;

            $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

                //Check if page data is dirty.
                if ($scope.frmEmployeeBio.$dirty) {
                    saveEmployeeBioData(true).then(function () {
                    }).catch(function () {
                        event.preventDefault();  //This does
                    });
                }
            });
        }

        accountService.getUserInfo().then(user => {
            workflowService.getUserWorkflowPromise().then(workflow => {
                resourceApi.getEmployeeProfileImages(user.selectedClientId(), user.employeeId)
                    .then(imageData => {
                        const employeeImage = {
                            employeeId: imageData.employeeId,
                            clientId: imageData.clientId,
                            profileImage: imageData,
                        };

                        init(user, workflow, employeeImage);
                    });
            });
        });
    }

    clickSavePageData(direction: number) {
        this.$scope.saveEmployeeBioData(false).then(isWorkflowTaskComplete => {
            const currState = this.workflowService.getCurrState();

            if (isWorkflowTaskComplete) {
                this.workflowService.updateWorkflowTask(currState, isWorkflowTaskComplete);
            }

            this.workflowService.getNextPrevPage(currState, direction);
        });
    }
}
