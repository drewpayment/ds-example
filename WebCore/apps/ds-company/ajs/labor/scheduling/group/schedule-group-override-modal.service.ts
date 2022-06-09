import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { ScheduleGroupOverrideModalController } from "./schedule-group-override-modal.controller";

export class ScheduleGroupOverrideModalService {
    static readonly SERVICE_NAME = 'ScheduleGroupOverrideModalService';
    static readonly $inject = [
        DsModalService.SERVICE_NAME
    ];

    constructor (private modal: DsModalService) {
    }

    open(employee, shift, scheduleGroups, selectedGroup) {
        return this.modal.open({
            template: require('./schedule-group-override-modal.html'),
            controller: ScheduleGroupOverrideModalController,
            
            resolve: {
                //passed in as parameters to the controller function RON
                employee: function() {
                    //this is the employee object
                    return employee;
                },
                shift: function() {
                    return shift;
                },
                scheduleGroups: function() {
                    return scheduleGroups;
                },
                selectedGroup: function() {
                    return selectedGroup;
                }
            }
        });
    }
}