import * as common from "../../../../../Scripts/util/ds-common";
import { STATES } from "../../shared/state-router-info";
import { WorkflowService } from '../workflow/workflow.service';
export class CompletionController {
    static readonly $inject = [
        '$rootScope',
        '$scope',
        '$injector',
        WorkflowService.SERVICE_NAME,
    ];

    constructor(private $rootScope, private $scope, private $injector, private workflowService: WorkflowService) {
        function init(workflow) {
            $scope.showCompletion = false;
            $scope.completionPct = null;

            $scope.myWorkflow = workflow;
            $scope.completionPct = $scope.myWorkflow.determineCompletionPct();

            $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
                var setMenu = !fromState.data ||
                    toState.data.menuName != fromState.data.menuName;

                if (setMenu) {
                    if (toState.data.menuName == STATES.ds.ess.onboarding.MENU_NAME) {
                        $scope.showCompletion = true;
                    } else {
                        $scope.showCompletion = false;
                    }
                }

            });


            $rootScope.$on('$itemCompletion', function (event, pct) {
                $scope.completionPct = pct;
            });
        }

        workflowService.getUserWorkflowPromise().then(workflow => init(workflow));
    }
}
