import { TimeOffManagerInstance } from "./services/manager.service";
import { EssLeaveTimeoffState } from "./header.state";

export class TimeOffSummaryController {
    static readonly COMPONENT_NAME = "timeoffSummary";
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        template: require('./summary.html'),
        controller: TimeOffSummaryController,
        bindings: {
            timeOff: "="
        }
    }

    timeOff: TimeOffManagerInstance;
    // unitsAvailable: number;

    constructor() {
    }

    $onInit() {
        // if only one policy then immediately display its activity view
        // todo: debugging
        // ?: this duplicates the sidebar menu for some reason with ui-router
        // if(this.timeOff.policies.length == 1) {
        //     this.timeOff.policies[0].viewActivity();
        // }
    }
}
