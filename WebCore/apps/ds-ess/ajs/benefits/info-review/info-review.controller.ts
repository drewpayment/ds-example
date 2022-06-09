import * as benefitSvc from "@ajs/benefits/benefits.service";
import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsUtilityServiceProvider } from "@ajs/core/ds-utility/ds-utility.service";
import { DsEmployeeContactInfoService } from "@ajs/employee/contact-info.service";
import { DsEmployeeDependentService } from "@ajs/employee/dependent.service";
import { IUserInfo } from "@ajs/user";
import { BenefitDependentEditModalService } from "../dependents/edit-modal.service";
import { AccountService } from '@ajs/core/account/account.service';

declare var _:any;
export class BenefitInfoReviewController {
    static readonly $inject = [
        "$scope",
        "$q",
        AccountService.SERVICE_NAME,
        benefitSvc.SERVICE_NAME,
        DsStateService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsUtilityServiceProvider.SERVICE_NAME,
        DsEmployeeContactInfoService.SERVICE_NAME,
        DsEmployeeDependentService.SERVICE_NAME,
        BenefitDependentEditModalService.SERVICE_NAME
    ];

    constructor($scope: any,
        $q: ng.IQService,
        accountService: AccountService,
        benefitSvc: any,
        state: DsStateService,
        msg: DsMsgService,
        util: DsUtilityServiceProvider,
        employeeSvc: DsEmployeeContactInfoService,
        dependentSvc: DsEmployeeDependentService,
        dependentModal: BenefitDependentEditModalService
    ) {

        var context = benefitSvc.getBenefitContext();

        $scope.addDependent = addDependent;
        $scope.editDependent = editDependent;
        $scope.continueEnrollment = continueEnrollment;
        $scope.canContinue = canContinue;
        $scope.dependentIsGood = dependentIsGood;
        $scope.dependentsHaveRelationships = dependentsHaveRelationships;
        $scope.birthDate = null;
        $scope.employeeHasBirthDate = employeeHasBirthDate;
        $scope.isEditEmployee = false;
        $scope.user = {};

        // datepicker stuff
        $scope.datePickerIsOpen = false;

        function init() {

            $scope.loading = true;
            msg.loading(true);

            if (!context.hasSelectedEnrollment()) {
                //state.router.go("ess.benefits.home");
                benefitSvc.redirect("home");
                return;
            }

            $scope.employee = context.employeeInfo.raw();
            $scope.dependents = context.dependents;

            /**
            * If the employee already have a birth date set, set $scope.birthDate to that date.
            */
            if ($scope.employee.birthDate) {
                var birthDate = util.parseDateInput($scope.employee.birthDate, "MM/DD/YYYY");
                $scope.birthDate = birthDate;
            }

            var benefitCtx = benefitSvc.getBenefitContext();
            var isLifeEvent = benefitCtx.selectedOpenEnrollment.openEnrollmentTypeId === benefitSvc.enrollmentTypes.LifeEvent;
            var inactiveCutoff = new Date(benefitCtx.selectedOpenEnrollment.effectiveDate ||
                benefitCtx.selectedOpenEnrollment.eventDate);
            var minPlanStart =
                benefitCtx.selectedOpenEnrollment.planList.map(function (p) { return new Date(p.startDate); });
            minPlanStart = Math.min.apply(null, minPlanStart);
            $scope.isActiveDependent = function(item) {
                var inactiveDate = new Date(item.inactiveDate);
                if (isLifeEvent) {
                    return !(item.isInactive && inactiveDate < inactiveCutoff);
                } else {
                    return !(item.isInactive && inactiveDate < minPlanStart);
                }
            }

            $scope.loading = false;
            msg.loading(false);
        }
        accountService.getUserInfo().then(user => {
            $scope.user = user;
            init();
        });

        /**
         * Opens a modal to add a new dependent. Upon success, the modal will be closed and the new
         * dependent will be returned.
         * @returns {}
         */
        function addDependent() {
            editDependent();
        }

        /**
         * Opens a model to edit an existing or new dependent.
         * @param {} dependent
         * @returns {}
         */
        function editDependent(dependent?) {
            var isNew = !dependent;

            var modal = dependentModal.open(dependent || { employeeId: $scope.user.employeeId });

            //update depedents upon modal success
            modal.result.then(function(updatedDep) {
                if(isNew) {
                    context.addDependent(updatedDep);
                } else {
                    angular.extend(dependent, updatedDep);
                    dependent.setMomentBirthDate();
                }

                $scope.dependents.refreshDependentState();
                context.selectedOpenEnrollment.setPlanOptionEligibility();
            });
        }

        /**
         * Checks if changes have been made to the employee's benefit settings. If so, saves them. Then redirects
         * to the enrollment view.
         * @returns {}
         */
        function continueEnrollment() {

             if(!$scope.birthDate){
                 msg.setTemporaryMessage("Please supply your date of birth before continuing", msg.messageTypes.error);
                 return;
             }
            var promises = [];

            msg.loading(true);

            if ($scope.employeeSettingsForm.$dirty) {

                $scope.employee.birthDate = $scope.birthDate;

                promises.push(benefitSvc
                    .saveEmployeeBenefitSettings($scope.employee)
                    .then(function(updated) {
                        employeeSvc.resetEmployeeContactCache();
                        angular.extend(context.employeeInfo, updated);
                    }));
            }

            $q  .all(promises)
                .then(function() {
                    return context.refreshSelectedOpenEnrollment();
                })
                .then(function() {
                    msg.setOnRouteChangeSuccessMessage("Your benefit information was successfully updated.");
                    //state.router.go("ess.benefits.enrollment");
                    benefitSvc.redirect("enrollment");
                });
        }

        function dependentIsGood(dependent) {
            var isGood = dependent.employeeDependentsRelationshipId !== null;

            return isGood;
        }

        /**
         * Checks if employee dependent relationship ID has been set for all the EE's dependents
         * and the employee has a birth date set.
         *
         * This must be satisfied before the user can click the Continue button
         * @returns boolean
         */
        function canContinue() {
            return dependentsHaveRelationships() && employeeHasBirthDate();
        }

        /**
         * Checks to make sure each dependent has a relationship ID set.
         * @returns boolean
         */
        function dependentsHaveRelationships() {
            return _.every(_.filter($scope.dependents, $scope.isActiveDependent), dependentIsGood);
        }

        /**
         * Checks to make sure the employee already had a birth date set ($scope.employee.birthDate)
         * or that a birth date has been added on the info-review ($scope.birthDate)
         * @returns boolean
         */
        function employeeHasBirthDate() {
            var hasBirthDate;

            if ($scope.employee != null
                && $scope.employee.birthDate
                || $scope.birthDate
            ) {
                hasBirthDate = true;
            } else {
                hasBirthDate = false;
            }
            return hasBirthDate;
        }
    }
}
