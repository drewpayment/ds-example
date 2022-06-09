import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { DsBubbleMessageService } from '@ajs/core/msg/ds-bubbleMsg.service';

export class DsNavWorkflowFooterController {
    static readonly $inject = ['$scope', DsMsgService.SERVICE_NAME, DsBubbleMessageService.SERVICE_NAME];
    savePageData: (isWorkflowTaskCompleted?: boolean) => ng.IPromise<boolean>;

    constructor(private $scope: ng.IScope, private msg: DsMsgService, private bubbleMsg: DsBubbleMessageService) {
        // $scope.nextPage = (direction) => {
        //     this.msg.clearMessage();
        //     this.bubbleMsg.hideBubbleMessage();

        //     const dir = direction;

        //     // myWorkflow is passed in from the page.

        //     $scope.savePageData()
        //         .then((isWorkflowTaskComplete) => {

        //             // if (isUndefinedOrNull(isWorkflowTaskComplete)) isWorkflowTaskComplete = false;

        //             const currState = $scope.myWorkflow.getCurrState();

        //             if (isWorkflowTaskComplete) {
        //                 $scope.myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
        //             }

        //             $scope.myWorkflow.getNextPrevPage(currState, dir);
        //         });
        // };
    }

    // nextPage(direction: number) {
    //     this.msg.clearMessage();
    //     this.bubbleMsg.hideBubbleMessage();

    //     this.savePageData().then((isWorkflowTaskComplete) => {
    //         const currState = this.$scope.myWorkflow.getCurrState();

    //         if (isWorkflowTaskComplete) {
    //             this.$scope.myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
    //         }

    //         this.$scope.myWorkflow.getNextPrevPage(currState, direction);
    //     });
    // }

}


