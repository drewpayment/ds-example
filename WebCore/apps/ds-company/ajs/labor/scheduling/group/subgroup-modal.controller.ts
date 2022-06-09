import * as angular from "angular";
import { PlannerScheduleGroup } from "@ajs/labor/group-scheduler/planner/schedule-group.model";
export class GroupScheduleSubGroupModalController {
    static readonly $inject = [
        '$scope',
        '$timeout',
        '$modalInstance',
        'scheduleGroup',
        'availableSubGroups'
    ];

    constructor ($scope, $timeout, $modalInstance, group, subgroups) {
        //the schedule group object
        $scope.group = angular.copy(group);
        //the scheduleGroupShiftName objects
        $scope.subgroups = angular.copy(subgroups);
        $scope.newSubgroup = {};
        $scope.modes = { add: true };

        (function init() {
            angular.forEach($scope.subgroups, function (sg) {
                sg.$isSelected = $scope.group.hasSubgroup(sg);
            });
        })();

        $scope.addSubgroup = function () {
            var addedSubgroup = (<PlannerScheduleGroup>$scope.group).addNewSubgroup($scope.newSubgroup.name);
            addedSubgroup.$isSelected = true;
            $scope.subgroups.push(addedSubgroup);
            $scope.newSubgroup.name = null;
            $scope.newSubgroupForm.$setPristine();
            $scope.modes.add = false;
            $timeout(function () {
                $scope.modes.add = true; 
            });
        };

        $scope.byNonDefaultSubGroup = function (subgroup) {
            return !subgroup.$isDefault;
        }

        $scope.selectSubgroup = function (subgroup) {
            subgroup.$isSelected = !subgroup.$isSelected;  
            if (subgroup.$isSelected) {
                $scope.group.addExistingSubgroup(subgroup); 
            } else {
                $scope.group.removeSubgroup(subgroup);
            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss();
        };

        $scope.save = function () {
            $modalInstance.close($scope.group);
        };
    }
}