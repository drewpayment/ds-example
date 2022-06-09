import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsEmployeeHRInfoService } from "@ajs/employee/employee-hr-info.service";
import { DsBubbleMessageService } from "@ajs/core/msg/ds-bubbleMsg.service";
import { valdateUsingRegExp } from "@util/ds-common";
import { AccountService } from '@ajs/core/account/account.service';
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class OnboardingOtherInfoController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
        "$location",
        DsMsgService.SERVICE_NAME,
        DsInlineValidatedInputService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        '$q',
        DsEmployeeHRInfoService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME
    ];

    constructor (protected $scope: ng.IScope,
        accountService: AccountService, 
        protected workflowService: WorkflowService, 
        $location, 
        MessageService, 
        DsInlineValidatedInputService, 
        DsOnboardingApi, 
        $q, 
        EmployeeHRInfoService, 
        BubbleMessageService) {
            function init(userAccount, myWorkflow) {
                $scope.hasError = false;
                $scope.isLoading = true;
                $scope.isLoaded = false;
                $scope.isAdding = false;
                $scope.isSoftWarned = false;
                $scope.isHardStop = false;
                //Used to pass workflow object into workflow footer via directive. [my-workflow]
                //The workflow footer directive also requires name of function used to save the data form the page. [save-Page-Data]
                $scope.myWorkflow = myWorkflow;
    
                $scope.circleNumber = myWorkflow.getCircleNumber();
                var currState = myWorkflow.getCurrState();
    
                $scope.isCompletedStatus = myWorkflow.isStateCompleted(currState);
                var isCompleted = !myWorkflow.isStateCompleted(currState);
                $scope.isCompleted = isCompleted || isCompleted === true;
                $scope.userData = {};
    
                function initUserData() {
                    $scope.userData = userAccount;
                    $scope.selected = {};
    
                    EmployeeHRInfoService.getEmployeeHRInfos(userAccount.employeeId, userAccount.lastClientId || userAccount.clientId, true).then(function (employeeHRInfoData) {
                        _.forEach(employeeHRInfoData, function(employeeHRInfo) {
                          if (employeeHRInfo.employeeHRInfos.length == 0) {
                              var newEmployeeHRInfo = {
                                  employeeHRInfoId: 0,
                                  clientHRInfoId: employeeHRInfo.clientHRInfoId,
                                  employeeId: userAccount.employeeId,
                                  value: (employeeHRInfo.fieldTypeId == 1) ? 'false' : '',
                                  clientId: userAccount.lastClientId || userAccount.clientId,
                              }
                              employeeHRInfo.employeeHRInfos.push(newEmployeeHRInfo);
                          }
                          else {
                              if (employeeHRInfo.fieldTypeId == 1) {
                                  employeeHRInfo.employeeHRInfos[0].value = employeeHRInfo.employeeHRInfos[0].value.toLowerCase();
                              }
                          }
                        });
                        $scope.employeeHRInfoList = employeeHRInfoData;                  
                   });
          
                    $scope.isLoading = false;
                    $scope.isLoaded = true;
    
                }
                initUserData();
    
                function savePageData(withoutSoftWarn) {
                    var deffered = $q.defer();
                    //deffered.resolve();
                    MessageService.clearMessage();
                    BubbleMessageService.hideBubbleMessage();
    
                    EmployeeHRInfoService.saveEmployeeHRInfos($scope.employeeHRInfoList, userAccount.employeeId, userAccount.lastClientId || userAccount.clientId)
                    .then(function (data) {
                        //Check for errors.  This should be in a catch.
                        if (data.hasNoError) {
                            $scope.isSaving = false;
                            $scope.savedChanges = true;
                            myWorkflow.updateWorkflowTask(currState, true);
        
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