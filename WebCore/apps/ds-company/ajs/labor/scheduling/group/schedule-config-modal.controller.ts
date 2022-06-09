import * as angular from "angular";
import { DsPlannerGroupScheduleService } from "@ajs/labor/group-scheduler/planner/group-schedule.service";

export class GroupScheduleConfigModalController {
    static readonly $inject = [
        '$scope',
        '$modalInstance',
        DsPlannerGroupScheduleService.SERVICE_NAME,
        'schedule',
        'scheduleGroups', //all the schedule groups WITH the sub groups for each
        'clientId'
    ];

    constructor ($scope, $modalInstance, scheduleSvc: DsPlannerGroupScheduleService, schedule, scheduleGroups, clientId) {
        //groupSchedule
        if (typeof schedule === 'undefined' || schedule === null) {
            $scope.schedule = scheduleSvc.create({ clientId: clientId });
            $scope.schedule.isActive = true;
        } else {
            $scope.schedule = angular.copy(schedule);
        }

        //scheduleGroups (with sub groups)
        $scope.groups = angular.copy(scheduleGroups);
        $scope.groupFilter = {};

        (function initGroups() {
            //foreach scheduleGroup
            angular.forEach($scope.groups, function (group) {
                group.isScheduled = $scope.schedule ? $scope.schedule.hasGroup(group) : false;
            });
        })();

        //adding/removing a scheduleGroup to the list of selected schedule groups
        $scope.selectGroup = function (group) {
            group.isScheduled = !group.isScheduled;
            if (group.isScheduled) {
                $scope.schedule.addGroup(group);
            } else {
                $scope.schedule.removeGroup(group);
            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss();
        };

        $scope.save = function () {
            $modalInstance.close($scope.schedule);
        };
    }
}
