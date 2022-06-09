import * as angular from "angular";

export class ScheduleGroupOverrideModalController {
    static readonly $inject = [
        '$scope',
        '$modalInstance',
        'employee',
        'shift',
        'scheduleGroups',
        'selectedGroup'
    ];

    //RON GET RID OF CRAP AND REDO STUFF HERE
    constructor ($scope, $modalInstance, employee, shift, scheduleGroups, selectedGroup) {
        $scope.employee = employee;
        $scope.shift = shift;
        $scope.scheduleGroups = scheduleGroups;
        $scope.selectedGroup = selectedGroup;
        
        if (shift.override_ScheduleGroupId != null) {
            angular.forEach(scheduleGroups, function (group) {
                if (group.scheduleGroupId == shift.override_ScheduleGroupId)
                    $scope.selectedGroup = group;
            });
        }
        $scope.cancel = function () {
            $modalInstance.dismiss();
        };

        $scope.save = function () {
            // $scope.shift.override_ScheduleGroupId = $scope.selectedGroup;
            $modalInstance.close($scope.selectedGroup);
        };
    }
}