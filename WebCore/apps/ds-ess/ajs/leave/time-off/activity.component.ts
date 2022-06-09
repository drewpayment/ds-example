import { EssLeaveTimeoffState } from "./header.state";
import { TimeOffManagerInstance } from "./services/manager.service";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { TimeOffPolicy } from "./shared/time-off-policy.model";

export class TimeoffActivityController {
    static COMPONENT_NAME = "timeoffActivity";
    static COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: TimeoffActivityController,
        template: require('./activity.html'),
        bindings: {
            timeOff: "="
        }
    }

    static readonly $inject = [
        'DsState'
    ];

    timeOff: TimeOffManagerInstance;
    policy: TimeOffPolicy;

    constructor(private dsState: DsStateService) {
    }

    $onInit(){
        this.policy = this.timeOff.getActivePolicy();
    }
    
    editEvent(event) {
        this.timeOff.editTimeOffEvent({ event: event }).then(() => {
            this.dsState.reload();
        });
    }
}
