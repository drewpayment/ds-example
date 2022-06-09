import * as angular from "angular";
import { GroupScheduleShiftModalModes } from "./shift-modal.modes";
import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { GroupScheduleShiftModalController } from "./shift-modal.controller";

declare var _:any;

export class GroupScheduleShiftModalService {
    static readonly SERVICE_NAME = 'GroupScheduleShiftModalService';
    static readonly $inject = [
        DsModalService.SERVICE_NAME
    ];
    modes = GroupScheduleShiftModalModes;

    constructor (private modal: DsModalService) {
    }
    open(group, subgroup, week, mode?) {
        return this.modal.open({
            template: require('./shift-modal.html'),
            controller: GroupScheduleShiftModalController,
            resolve: {  
                scheduleGroup: function () {
                    return group;
                },
                selectedSubGroup: function () {
                    return subgroup;
                },
                week: function () {
                    return week;
                },
                mode: function () {
                    return angular.extend({ type: GroupScheduleShiftModalModes.DEFAULT, value: null}, mode);
                }
            },
            size: 'lg'
        });
    }
}