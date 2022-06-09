import { GroupScheduleConfigModalController } from "./schedule-config-modal.controller";
import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { DsPlannerGroupService } from "@ajs/labor/group-scheduler/planner/schedule-group.service";

export class GroupScheduleConfigModalService {
    static readonly SERVICE_NAME = 'GroupScheduleConfigModalService';
    static readonly $inject = [
        DsModalService.SERVICE_NAME,
        DsPlannerGroupService.SERVICE_NAME
    ];

    constructor (private modal: DsModalService, private groupSvc: DsPlannerGroupService) {
    }
    open(schedule, clientId) {
        return this.modal.open({
            template: require('./schedule-config-modal.html'),
            controller: GroupScheduleConfigModalController,
            resolve: {  
                schedule: function () {
                    return schedule;
                },
                clientId: function () {
                    return clientId;  
                },
                scheduleGroups: () => {
                    return this.groupSvc.getList();
                }
            },
            size: 'lg'
        });
    }
}