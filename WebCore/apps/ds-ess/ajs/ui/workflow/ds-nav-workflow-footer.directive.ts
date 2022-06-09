import * as angular from 'angular';
import { DsNavWorkflowFooterController } from './ds-nav-workflow-footer.controller';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { DsBubbleMessageService } from '@ajs/core/msg/ds-bubbleMsg.service';
import { WorkflowService } from './workflow.service';

/**
 * Directive used to construct a navigational menu within the ESS application.  The menu monitors state changes and
 * will update the menu's active menu-item based on the current state (see states.js).
 *
 * @ngdirective
 */
export class DsNavWorkflowFooterComponent {
    static readonly COMPONENT_NAME = 'dsNavWorkflowFooter';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        bindings: {
            workflowHideNext: '=',
            workflowHidePrev: '=',  // Ess.Services~Menu() object (see Menu.js) containing the menu-items & configuration to display
            onSave: '&',
        },
        controller: DsNavWorkflowFooterComponent,
        template: require('./ds-nav-workflow-footer.html'),
    };
    static readonly $inject = [
        DsMsgService.SERVICE_NAME,
        DsBubbleMessageService.SERVICE_NAME,
        WorkflowService.SERVICE_NAME,
    ];

    onSave;
    workflowHideNext: boolean;
    workflowHidePrev: boolean;

    constructor(
        private msg: DsMsgService,
        private bubbleMsg: DsBubbleMessageService,
        private myWorkflow: WorkflowService,
    ) {
        // angular.extend(this.controller, {
        //     savePageData: this.scope.savePageData,
        // });
    }

    // static $instance(): ng.IDirectiveFactory {
    //     const dir = (msg: DsMsgService, bubbleMsg: DsBubbleMessageService, workflow: WorkflowService) =>
    //         new DsNavWorkflowFooterDirective(msg, bubbleMsg, workflow);
    //     dir.$inject = [DsMsgService.SERVICE_NAME, DsBubbleMessageService.SERVICE_NAME, WorkflowService.SERVICE_NAME];
    //     return dir;
    // }

    // link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {

    // }

    nextPage(direction: number) {
        this.msg.clearMessage();
        this.bubbleMsg.hideBubbleMessage();

        this.onSave({ $direction: direction });

        // .then((isWorkflowTaskComplete) => {
        //     const currState = this.myWorkflow.getCurrState();

        //     if (isWorkflowTaskComplete) {
        //         this.myWorkflow.updateWorkflowTask(currState, isWorkflowTaskComplete);
        //     }

        //     this.myWorkflow.getNextPrevPage(currState, direction);
        // });
    }

}
