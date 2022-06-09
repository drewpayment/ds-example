import { STATES } from "../../shared/state-router-info";
import { WorkflowService } from '../../ui/workflow/workflow.service';

export class WorkflowSidebarController {
    static readonly CONTROLLER_NAME = 'WorkflowSidebarController';
    static readonly CONFIG = {
        template: require('./workflow-sidebar.html')
    };
    static readonly $inject = [
        '$rootScope',
        '$scope',
        WorkflowService.SERVICE_NAME
    ];

    constructor($rootScope: ng.IRootScopeService, $scope: any, workflowService: WorkflowService) {
        function init(workflow) {
            $scope.items = workflow.getUserWorkflow();

            //Lock menu if Onboarding is finalized.
            $scope.isFinalized = workflow.checkFinalizedStatus();

            $rootScope.$on('workflowUpdate', function(event, menuId, menuSubId) {
                $scope.items = workflow.getUserWorkflow();
            });

            $scope.workflowItemSelect = function(item) {
                //need to start using Item being passed in
                item.toggleState = !item.toggleState;
            }

            $scope.workflowMenuItemClicked = function(item) {
                $rootScope.$emit('$workflowMenuItemClicked');
            }

            //----------------------------------------------
            //fires when ever there is a workflow change.
            //----------------------------------------------
            $rootScope.$on('$workflowUpdate', function () {
                $scope.isFinalized = workflow.checkFinalizedStatus();
            });
        }

        workflowService.getUserWorkflowPromise().then(w => init(w));
    }
}

export class WorkflowSidebarComponent {
    static readonly COMPONENT_NAME = 'ajxEssOnboardingWorkflowSidebar';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: WorkflowSidebarController,
        template: require('./workflow-sidebar.html'),
    };
}
