import * as angular from "angular";
import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { DsPlannerGroupService } from "@ajs/labor/group-scheduler/planner/schedule-group.service";
import { GroupScheduleSubGroupModalController } from "./subgroup-modal.controller";

declare var _:any;

export class GroupScheduleSubGroupModalService {
    static readonly SERVICE_NAME = 'GroupScheduleSubGroupModalService';
    static readonly $inject = [
        DsModalService.SERVICE_NAME,
        DsPlannerGroupService.SERVICE_NAME
    ];

    constructor (private modal: DsModalService, private groupSvc: DsPlannerGroupService) {
    }

    open(group) {
        return this.modal.open({
            template: require('./subgroup-modal.html'),
            controller: GroupScheduleSubGroupModalController,
            resolve: {  
                scheduleGroup: function () {
                    //this is the scheduleGroup object
                    return group;
                },
                availableSubGroups: () => {
                    //passing in the scheduleGroups.scheduleGroupShiftNames (sub-groups)
                    return this.groupSvc
                        .getDetail(group.scheduleGroupId)
                        .then(function (apiScheduleGroup) {

                            angular.forEach(group.subgroups, function (sg) {
                                var apiSubgroup;

                                if(!sg.$isDefault) {
                                    if(sg.$isNew) {
                                        // add back in newly created subgroups
                                        apiScheduleGroup.subgroups.push(sg); 
                                    } else {
                                        apiSubgroup = _.find(apiScheduleGroup.subgroups, { scheduleGroupShiftNameId: sg.scheduleGroupShiftNameId });
                                        apiSubgroup.name = sg.name;
                                    }
                                }
                            });

                            return apiScheduleGroup.subgroups;
                        });
                }
            }
        });
    }
}