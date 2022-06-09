import * as angular from "angular";
import { GroupScheduleShiftModalModes } from "./shift-modal.modes";

declare var _:any;

export class GroupScheduleShiftModalController {
    static readonly $inject = [
        '$scope',
        '$modalInstance',
        'scheduleGroup',
        'selectedSubGroup',
        'week',
        'mode'
    ];

    constructor($scope: any, $modalInstance, scheduleGroup, selectedSubGroup, week, mode) {
        $scope.allShifts = [];
        $scope.shiftsActive = [];

        $scope.scheduleGroup = angular.copy(scheduleGroup);
        $scope.week = week;

        (function init() {
            var shiftIndex;

            updateAllShifts();

            // sub group
            $scope.subgroup = _.first(_.filter($scope.scheduleGroup.subgroups, function (sg) {
                return angular.equals(sg, selectedSubGroup);
            }));

            if (mode && mode.type) {
                switch(mode.type) {
                    case GroupScheduleShiftModalModes.ADD_SHIFT:
                        addShift(mode.value);
                        break;
                    case GroupScheduleShiftModalModes.EDIT_SHIFT: {
                        shiftIndex = _.indexOf(selectedSubGroup.shifts, mode.value);
                        setActiveShift($scope.subgroup.shifts[shiftIndex]);
                        break;
                    }
                }
            } else if ($scope.subgroup.shifts && $scope.subgroup.shifts.length) {
                setActiveShift($scope.subgroup.shifts[0]);
            }                
        })();

        $scope.addShift = addShift;

        function addShift(dow) {
            var shift = $scope.subgroup.addShiftOnDow(dow);
            setActiveShift(shift);
        }

        $scope.duplicateShift = duplicateShift;
        function duplicateShift(baseShift) {
            var shift = $scope.subgroup.addShiftOnDow(baseShift.dayOfWeek);
            shift.startTime = baseShift.startTime;
            shift.endTime = baseShift.endTime;
            shift.numberOfEmployees = baseShift.numberOfEmployees;
            shift.momentize();
            setActiveShift(shift);
        }

        function setActiveShift(shift) {
            angular.forEach($scope.subgroup.shifts, function (s) {
                s.$active = false; 
            });

            shift.$active = true;
        }

        function updateAllShifts() {
            $scope.allShifts = [];

            // all shifts
            angular.forEach($scope.scheduleGroup.subgroups, function (sg) {
                angular.forEach(sg.shifts, function (shift) {
                    shift.$selectedSubgroup = shift.$subgroup;
                    $scope.allShifts.push(shift);
                });
            });
        }

        $scope.activeShifts = function () {

            if($scope.subgroup) {
                $scope.shiftsActive = $scope.subgroup.shifts;
            } else {
                updateAllShifts();
                $scope.shiftsActive = $scope.allShifts;
            }
            return $scope.shiftsActive;
        };

        $scope.switchShiftSubgroup = function (shift) {
            shift.$subgroup.removeShift(shift);
            shift.$selectedSubgroup.addShift(shift);
            updateAllShifts();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss();
        };

        $scope.save = function () {
            if($scope.subgroup)
                $scope.subgroup.orderShifts();
            $modalInstance.close($scope.scheduleGroup);
        };
    }
}