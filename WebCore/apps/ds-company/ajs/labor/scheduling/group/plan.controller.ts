import * as angular from "angular";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsWeekService } from "@ajs/core/ds-week/ds-week.service";
import { GroupScheduleConfigModalService } from "./schedule-config-modal.service";
import { IUserInfo } from "@ajs/user";
import { GroupScheduleShiftModalService } from "./shift-modal.service";
import { GroupScheduleSubGroupModalService } from "./subgroup-modal.service";
import { PlannerGroupSchedule } from '@ajs/labor/group-scheduler/planner/group-schedule.model';

declare var _:any;

export class GroupSchedulePlanningController {
    static readonly $inject = [
        '$scope',
        'schedules',
        'moment',
        'userAccount',
        DsMsgService.SERVICE_NAME,
        DsWeekService.SERVICE_NAME,
        GroupScheduleConfigModalService.SERVICE_NAME,
        GroupScheduleShiftModalService.SERVICE_NAME,
        GroupScheduleSubGroupModalService.SERVICE_NAME
    ];

    constructor ($scope, schedules: PlannerGroupSchedule[], moment, userAccount: IUserInfo, msg: DsMsgService, dsWeek: DsWeekService, scheduleModal: GroupScheduleConfigModalService, shiftModal: GroupScheduleShiftModalService, subGroupModal: GroupScheduleSubGroupModalService) {
        $scope.noop = angular.noop;
        $scope.schedules = schedules;
        $scope.schedule = null;

        $scope.week = dsWeek.getWeek();

        $scope.addSchedule = function () {
            editSchedule();
        };

        $scope.editSchedule = editSchedule;

        $scope.toggleActions = function toggleActions($event) {
            $event.stopPropagation();
            $event.preventDefault();

            $scope.schedule.$isActionOpen = !$scope.schedule.$isActionOpen;
        };

        $scope.copySchedule = function () {
            editSchedule($scope.schedule.copy());
        }

        $scope.scheduleSelected = function (schedule) {
            if(schedule && schedule.groupScheduleId && !schedule.$isLoaded) {
                schedule.$isSaved = true;
                msg.loading(true);
                schedule.load().then(function () {
                    msg.loading(false);
                    schedule.$isSaved = true;
                });
            }
        }

        function editSchedule(schedule?) {
            //opeing the modal for creating/editing a group schedule and allowing the user to name
            //and select the schedule groups for the group schedule
            scheduleModal
                .open(schedule, userAccount.lastClientId || userAccount.clientId)
                .result
                .then(function(updatedSchedule) {
                    var origIndex = _.indexOf($scope.schedules, schedule);

                    if(origIndex < 0) {
                        $scope.schedules.push(updatedSchedule);
                    } else {
                        $scope.schedules[origIndex] = updatedSchedule;
                    }

                    //set the current groupSchedule object
                    $scope.schedule = updatedSchedule;
                    $scope.schedule.$isSaved = false;
                });
        }

        $scope.editShift = function (subgroup, shift, $event) {
            if($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }

            editShifts(subgroup, { type: shiftModal.modes.EDIT_SHIFT, value: shift });
        };

        $scope.addShift = function (subgroup, dow) {
            editShifts(subgroup, { type: shiftModal.modes.ADD_SHIFT, value: dow });
        };

        //this is where the 'Add Subgroup' modal is opened; passing in the 'scheduleGroup' object
        $scope.addSubgroup = function (group) {
            subGroupModal
                .open(group)
                .result
                .then(function (updatedGroup) {
                    $scope.schedule.replaceGroup(group, updatedGroup);
                    $scope.schedule.$isSaved = false;
                });
        };

        $scope.byNonDefaultSubGroup = function (subgroup) {
            return !subgroup.$isDefault;
        }

        $scope.saveSchedule = function (unpublish) {
            msg.sending(true);
            $scope.schedule.isActive = !unpublish;
            $scope.schedule.save().then(function() {
                msg.sending(false);
                msg.setTemporarySuccessMessage("Schedule changes saved successfully");
                $scope.schedule.$isSaved = true;

                // If marking the current schedule as inactive, remove it from the schedules array.
                if (!$scope.schedule.isActive) {
                    const index = $scope.schedules.indexOf($scope.schedule as PlannerGroupSchedule);
                    if (index > -1) {
                        $scope.schedules.splice(index, 1);
                        $scope.schedule = null;
                    }
                }
            })
            ['catch'](msg.showWebApiException);
        }

        function editShifts(subgroup, mode) {
            shiftModal
                .open(subgroup.$scheduleGroup, subgroup, $scope.week, mode)
                .result
                .then(function (updatedGroup) {
                    $scope.schedule.replaceGroup(subgroup.$scheduleGroup, updatedGroup);
                    $scope.schedule.$isSaved = false;
                });
        }

        $scope.editSubgroup = function (subgroup) {
            subgroup.$isEditing = true;
            subgroup.$editName = subgroup.name;
        }

        $scope.updateSubgroup = function (subgroup) {
            if(subgroup.name !== subgroup.$editName) {
                subgroup.name = subgroup.$editName;
                $scope.schedule.$isSaved = false;
            }

            subgroup.$isEditing = false;
        }
    }
}
