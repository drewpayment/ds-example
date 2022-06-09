import * as angular from "angular";
import { DsWeekService, DayOfWeek } from "@ajs/core/ds-week/ds-week.service";
import { DsSchedulerContextService } from "@ajs/labor/group-scheduler/scheduler/context.service";
import { DsSchedulerGroupScheduleService } from "@ajs/labor/group-scheduler/scheduler/group-schedule.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { GroupSchedulePrintDialogService } from "./print-dialog.service";
import { ScheduleGroupOverrideModalService } from "./schedule-group-override-modal.service";

export class GroupScheduleSchedulingController {
    static readonly $inject = [
        '$scope',
        '$q',
        '$window',
        '$timeout',
        'moment',
        DsWeekService.SERVICE_NAME,
        DsSchedulerContextService.SERVICE_NAME,
        DsSchedulerGroupScheduleService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        'scheduleGroups',
        GroupSchedulePrintDialogService.SERVICE_NAME,
        ScheduleGroupOverrideModalService.SERVICE_NAME,
        'rateAccess',
        'budgetAccess'
    ];

    constructor ($scope, $q, $window, $timeout, moment, dsWeek: DsWeekService, contextSvc, groupScheduleSvc, msgSvc, scheduleGroups, printDialog, overrideDialog, rateAccess, budgetAccess) {

        var weekof         = moment(), // initially show this week's schedule
            firstDayOfWeek = DayOfWeek.SUNDAY,
            getWeek        = dsWeek.getWeek,
            activeEmployee = null;

        $scope.scheduleGroups = scheduleGroups;
        $scope.context        = {};
        $scope.mode           = {
            $isRecurring: false
        };

        $scope.activeGroup    = null;
        $scope.activeSchedule = null;
        $scope.moment         = moment;

        $scope.week = [];

        $scope.goToNextWeek           = goToNextWeek;
        $scope.goToPrevWeek           = goToPrevWeek;
        $scope.selectGroup            = selectGroup;
        $scope.selectSchedule         = selectSchedule;
        $scope.clearAndSelectSchedule = clearAndSelectSchedule;
        $scope.selectEmployee         = selectEmployee;
        $scope.assignShiftsToEmployee = assignShiftsToEmployee;
        $scope.selectShift            = selectShift;
        $scope.saveSchedule           = saveSchedule;
        $scope.cancelChanges          = cancelChanges;
        $scope.nonDefaultSubgroup     = nonDefaultSubgroup;
        $scope.toggleActions          = toggleActions;
        $scope.getStatusText          = getStatusText;
        $scope.order = {
            byEmployeeStatus : byEmployeeStatus,
            byEmployeeTitle  : byEmployeeTitle,
            byEmployeeName   : byEmployeeName
        };
        $scope.removeEmployeeShift    = removeEmployeeShift;

        $scope.isPrinting = false;
        $scope.printIncludeJobTitles        = true;
        $scope.printIncludeOtherGroupShifts = false;
        $scope.printIncludeOpenShifts       = false;
        $scope.printIncludeSubgroups        = false;
        $scope.printIncludeBenefits         = true;
        $scope.printInScheduleGroup         = printInScheduleGroup;
        $scope.printNotInScheduleGroup      = printNotInScheduleGroup;
        $scope.printBenefits                = printBenefits;
        $scope.printSchedule                = printSchedule;
        $scope.scheduleGroupOverridePopup   = scheduleGroupOverridePopup;

        // recurring shifts
        $scope.setRecurringShifts           = setRecurringShifts;
        $scope.removeEmployeeRecurringShift = removeEmployeeRecurringShift;
        $scope.toggleRecurringShift         = toggleRecurringShift;

        // view rate
        $scope.rateAccess                   = rateAccess;

        // view budget
        $scope.budgetAccess                 = budgetAccess;

        (function init() {
            updateWeek();
        })();


        function goToNextWeek() {
            weekof.add({days: 7});
            updateWeek();
        }

        function goToPrevWeek() {
            weekof.add({days: -7});
            updateWeek();
        }

        function updateWeek() {
            $scope.week = getWeek(weekof, firstDayOfWeek);
            loadContext();
        }

        function selectGroup(group) {
            var schedulePromise;

            $scope.activeGroup = group;
            $scope.activeSchedule = null;

            if(group && group.scheduleGroupId) {
                schedulePromise = groupScheduleSvc.getList(group).then(function (schedules) {
                    $scope.groupSchedules = schedules; 
                });

                loadContext(schedulePromise);
            } else {

                if(group && !group.scheduleGroupId) {
                    msgSvc.setTemporaryMessage("Schedules are not available for " + group.description, msgSvc.messageTypes.info);
                }

                $scope.context = null;
            }
        }

        function loadContext(pendingPromise?) {
            var promises = [],
                contextPromise;

            // setup loading state
            if($scope.context)
                $scope.context.$isLoading = true;

            msgSvc.loading(true);
            activeEmployee = null;
            
            // load employee schedules within the given date range
            contextPromise = contextSvc
                .getContext($scope.week, $scope.activeGroup)
                .then(function (context) {
                    $scope.context = context;
                    $scope.context.$isSaved = true;
                });
            promises.push(contextPromise);

            // if schedules are being loaded too, wait for them to load as well
            if(pendingPromise) 
                promises.push(pendingPromise);

            return $q.all(promises).then(function () {
                
                // set active group schedule
                var schedule = $scope.context.getGroupScheduleFromShifts($scope.groupSchedules);
                if (schedule) {
                    selectSchedule(schedule);
                } else if(!$scope.context.hasShifts()) {

                    if($scope.activeSchedule) {
                        selectSchedule($scope.activeSchedule);
                    } else if ($scope.groupSchedules && $scope.groupSchedules.length) {
                        selectSchedule($scope.groupSchedules[0]);
                    } else {
                        selectSchedule();  
                    }

                } else {
                    selectSchedule();
                }

                msgSvc.loading(false);
                $scope.context.$isLoading = false;
            })['catch'](msgSvc.showWebApiException);
        }

        function clearAndSelectSchedule(schedule) {
            if($scope.context) {
                $scope.context.clearScheduledShifts();
            }

            selectSchedule(schedule);
        }

        function selectSchedule(schedule?) {
            $scope.activeSchedule = schedule;
            
            if($scope.context) {
                $scope.context.setGroupSchedule($scope.activeSchedule);
                $scope.context.assignRecurringShifts();
            }
        }

        function saveSchedule(isPreview?) {
            if($scope.context) {
                msgSvc.loading(true);
                $scope.context.$isLoading = true;
                $scope.context.save(isPreview).then(function () {
                    $scope.context.$isSaved = true; 
                    msgSvc.setTemporarySuccessMessage("Schedule saved successfully");
                    $scope.context.$isLoading = false;
                    $scope.mode.$isRecurring = false;
                    activeEmployee = null;
                    deselectAllEmployees();
                    deselectShifts();
                })['catch'](msgSvc.showWebApiException);
            }
        }

        function cancelChanges() {
            if($scope.context) {
                $scope.context.cancelChanges();
                $scope.mode.$isRecurring = false;
            }
        }

        function nonDefaultSubgroup(subgroup) {
            return subgroup.$isSubgroup;
        }

        // ORDERBY : status
        function byEmployeeStatus(employee) {
            return employee.isTerminated || employee.isNotAssignedToSource ? 1 : 0;
        }

        // ORDERBY : name
        function byEmployeeTitle(employee) {
            
            return (!$scope.isPrinting || $scope.printIncludeJobTitles) ? (employee.jobTitle || "zzz") : 0;
        }

        // ORDERBY : name
        function byEmployeeName(employee) {
            return employee.lastName + employee.firstName;
        }

        // FILTER: 
        function printInScheduleGroup(shift) {
            return shift.isScheduledForGroup($scope.context.$scheduleGroup) && !shift.isOverridden;
        }

        // FILTER: 
        function printNotInScheduleGroup(shift) {
            return $scope.printIncludeOtherGroupShifts 
                && (!shift.isScheduledForGroup($scope.context.$scheduleGroup) || shift.isOverridden)
                && ($scope.context.$isPreview || !shift.isPreview);
        }

        // FILTER: 
        function printBenefits(benefit) {
            return $scope.printIncludeBenefits;
        }

        function printSchedule() {
            var origSettings = {
                includeOtherShifts: $scope.printIncludeOtherGroupShifts,
                includeJobTitles:   $scope.printIncludeJobTitles,
                includeOpenShifts:  $scope.printIncludeOpenShifts,
                includeSubgroups:   $scope.printIncludeSubgroups,
                includeBenefits:    $scope.printIncludeBenefits,
                includeHours:       $scope.printIncludeHours,
                includeRates:       $scope.printIncludeRates,
                rateAccess:         $scope.rateAccess,
                budgetAccess:       $scope.budgetAccess
            };

            printDialog.open(origSettings).result.then(function (settings) {
                $scope.isPrinting = true;
                $scope.printIncludeOtherGroupShifts = settings.includeOtherShifts;
                $scope.printIncludeJobTitles        = settings.includeJobTitles;
                $scope.printIncludeOpenShifts       = settings.includeOpenShifts;
                $scope.printIncludeSubgroups        = settings.includeSubgroups;
                $scope.printIncludeBenefits         = settings.includeBenefits;
                $scope.printIncludeHours            = settings.includeHours;
                $scope.printIncludeRates            = settings.includeRates;
                $timeout($window.print).then(function () {
                    $scope.isPrinting = false;
                });
            });
        }
        function scheduleGroupOverridePopup(employee, shift) {
        
            overrideDialog.open(employee, shift, $scope.scheduleGroups, $scope.activeGroup).
                result.then(function(selectedGroupOverride) {
                    //check if the shift is overridden
                    if (shift.override_ScheduleGroupId != selectedGroupOverride.scheduleGroupId) {
                        $scope.context.$isSaved = false;
                    }
                    shift.override_ScheduleGroupId = selectedGroupOverride.scheduleGroupId;
                    shift.override_ScheduleGroupSourceId = selectedGroupOverride.sourceId;
                    shift.isOverridden = shift.scheduleGroupId != shift.override_ScheduleGroupId;
                    shift.override_Description = selectedGroupOverride.description;
                    
            } );
                
            
        }
        function getStatusText(employee) {
            var status = '',
                firstLast = employee.firstName + ' ' + employee.lastName;

            if(employee.isTerminated) {
                status += firstLast + ' is no longer an active employee';
            }

            if(employee.isNotAssignedToSource) {
                if(status) {
                    status += ' or assigned to the ' + $scope.activeGroup.description + ' group';
                } else {
                    status += firstLast + ' is no longer assigned to the ' + $scope.activeGroup.description + ' group';
                }
            }

            if(status)
                status += '.';

            return status;
        }

        function toggleActions($event) {
            $event.stopPropagation();
            $event.preventDefault();

            $scope.context.$isActionOpen = !$scope.context.$isActionOpen;
        }

        ///////////// EMPLOYEE SHIFT SELECTION /////////////////

        function selectEmployee(employee, $event) {
            var wasSelected = employee.$selected;

            if($event) {
                $event.stopPropagation();
                $event.preventDefault();
            }

            deselectAllEmployees();

            employee.$selected = !wasSelected;
            activeEmployee = employee.$selected ? employee : null;

            if(activeEmployee && $scope.activeSchedule) {
                if(assignShiftsToEmployee(activeEmployee)) {
                    activeEmployee.$selected = false;
                    activeEmployee = null;
                }
            }
        }

        function assignShiftsToEmployee(employee) {
            var wasScheduled = false;

            if(employee && $scope.activeSchedule) {
                angular.forEach($scope.activeSchedule.subgroups, function(subgroup) {
                    angular.forEach(subgroup.shifts, function (shift) {
                        if(shift.$selected) {
                            employee.assignGroupShift(shift);
                            shift.$selected = false;
                            wasScheduled = true;
                        }
                    });
                });
            }

            return wasScheduled;
        }

        function deselectAllEmployees() {
            angular.forEach($scope.context.$employeeSchedules, function (schedule) {
                schedule.$selected = false;
            });
        }

        function selectShift(shift) {
            var wasSelected = shift.$selected;

            // if already have an employee selected immediately assign it
            if(activeEmployee) {
                activeEmployee.assignGroupShift(shift);
            } else {
                deselectShifts(shift.dayOfWeek);
                shift.$selected = !wasSelected;
            }
        }

        function deselectShifts(dow?:DayOfWeek) {
            if($scope.activeSchedule) {
                angular.forEach($scope.activeSchedule.subgroups, function(subgroup) {
                    angular.forEach(subgroup.shifts, function (shift) {
                        if(!angular.isDefined(dow) || (dow && shift.dayOfWeek === dow)) {
                            shift.$selected = false;
                        } 
                    });
                });
            }
        }

        function removeEmployeeShift(employee, shift, $event) {
            $event.stopPropagation();
            $event.preventDefault();

            employee.removeShift(shift);
        }

        ///////////// RECURRING SHIFTS /////////////////

        function removeEmployeeRecurringShift(employee, shift, $event) {
            if($event) {
                $event.stopPropagation();
                $event.preventDefault();
            }

            employee.removeRecurringShift(shift);
        }

        function setRecurringShifts() {
            $scope.mode.$isRecurring = true;
        }

        function toggleRecurringShift(shift, $event) {
            if($event) {
                $event.stopPropagation();
                $event.preventDefault();
            }

            shift.toggleRecurring();
        }
    }
}