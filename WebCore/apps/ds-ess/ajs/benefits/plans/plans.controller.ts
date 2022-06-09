import * as angular from 'angular';
import { PlanSelectionModalService } from './plan-selection-modal.service';
import { DsStateService } from '@ajs/core/ds-state/ds-state.service';
import { DsInlineValidatedInputService } from '../../ui/form-validation/ds-inline-validated-input.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { STATES } from '../../shared/state-router-info';
import * as benefitSvc from '@ajs/benefits/benefits.service';
import { AccountService } from '@ajs/core/account/account.service';

declare var _: any;
export class BenefitPlansController {

    static readonly $inject = [
        '$scope',
        AccountService.SERVICE_NAME,
        benefitSvc.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsInlineValidatedInputService.SERVICE_NAME,
        DsStateService.SERVICE_NAME,
        PlanSelectionModalService.SERVICE_NAME
    ];

    constructor($scope, accountService: AccountService, benefitSvc, msg, DsInlineValidatedInputService, state, planSelectionModal) {

        let context;

        $scope.openEnrollment                    = {};
        $scope.plans                             = [];
        $scope.hasError                          = false;
        $scope.isLoading                         = true;
        $scope.currentUser                       = {};
        $scope.selectedPlanOption                = {};
        $scope.currentPlanDetail                 = {};
        $scope.employeeOpenEnrollment            = {};
        $scope.waivedCoverage                    = false;
        $scope.validateResource                  = validateResource;
        $scope.getBenefitAmountLabel             = getBenefitAmountLabel;

        function initController() {

            context = benefitSvc.getBenefitContext();

            if (!context.hasSelectedCoverageType()) {

                if (context.hasSelectedEnrollment()) {
                    //state.router.go('ess.benefits.enrollment');
                    benefitSvc.redirect("enrollment");
                } else {
                    //state.router.go('ess.benefits.home');
                    benefitSvc.redirect("home");
                }

                return;
            } else {
                $scope.coverageType                   = context.selectedCoverageType;
                $scope.openEnrollment                 = context.selectedOpenEnrollment;
                $scope.plans                          = context.selectedCoverageType.plans;
                $scope.employeeOpenEnrollment         = context.selectedOpenEnrollment.employeeOpenEnrollment;

                // these are used in the header and/or selection summary pane
                $scope.selectedOpenEnrollment         = context.selectedOpenEnrollment;
                $scope.selectedEmployeeOpenEnrollment = context.selectedOpenEnrollment.employeeOpenEnrollment;

                angular.forEach($scope.plans, function (p) {

                    initPlanResources(p.resourceList); // todo: should be moved to service method

                    // init view-mode
                    p.showInfo = true;
                    p.showCoverage = true;

                    if (p.planOptionList.length > 1) {
                        p.showInfo = false;
                        p.showCoverage = true;
                    }

                });

                if ($scope.employeeOpenEnrollment) {

                    $scope.isSigned   = $scope.employeeOpenEnrollment.isSigned;
                    $scope.dateSigned = $scope.employeeOpenEnrollment.dateSigned;
                }

                let inactiveCutoffDate;
                if ($scope.openEnrollment.openEnrollmentTypeId === benefitSvc.enrollmentTypes.LifeEvent) {
                    inactiveCutoffDate = new Date($scope.openEnrollment.effectiveDate ||
                        $scope.openEnrollment.eventDate);
                } else {
                    inactiveCutoffDate = new Date(Math.min.apply(null, $scope.plans.map(function(p) { return new Date(p.startDate || '1900-01-01'); })));
                }

                let selectedDepIds = $scope.employeeOpenEnrollment.employeeSelections ? $scope.employeeOpenEnrollment.employeeSelections.reduce(function(accum, sel) {
                    let depIds =
                        sel.employeeDependentDtosSelections.map(function (dep) { return dep.employeeDependentId; });
                    return _.uniq(accum.concat(depIds));
                }, []) : [];

                $scope.dependentFilter = function (item) {
                    let inactiveDate = new Date(item.dependent.inactiveDate);
                    return _.contains(selectedDepIds, item.dependent.employeeDependentId) || !item.dependent.isInactive || inactiveDate >= inactiveCutoffDate;
                };
                $scope.isLoading = false;
            }
        }

        accountService.getUserInfo().then(user => {
            $scope.user = user;
            initController();
        });

        $scope.isNotEligibleForAnyPlans = function () {
            return !_.some($scope.plans, $scope.isPlanViewable);
        };
        $scope.isPlanViewable = function (p) {
            return p.isSelectable || p.isSelected;
        };
        $scope.isOptionViewable = function (o) {
            return o.isSelectable || o.isSelected;
        };

        $scope.isPlanValid = function (p) {

            let isValid = true,
                hasSavedSelections = false;

            p.isSelected = false;

            angular.forEach(p.planOptionList, function(po) {

                if (po.isSelected && isValid) {
                    p.isSelected = true;

                    if (po.isEmployeeElected && !po.selection.cost) {
                        isValid = false;
                    }

                    // validate dependent selections
                    if (!po.isDependentLimitSatisfied()) {
                        isValid = false;
                    }
                }

                if (po.selection.employeeOpenEnrollmentSelectionId) {
                    hasSavedSelections = true;
                }

            });

            return isValid && (p.isSelected || hasSavedSelections);
        };

        $scope.selectOption = function (option) {
            if (option.hasBenefitAmounts) {
                option.isSelected = !!option.selection.selectedBenefitAmount;
            } else {
                option.isSelected = !option.isSelected;
            }

            // if spouse ... auto add todo:clean this up
            if (option.eligibleDependents.length && !option.hasElectableDependents) {
                angular.forEach(option.eligibleDependents, function(ed) {
                    ed.isSelected = option.isSelected;
                });
            }

            if (option.isSelected) {
                // see if any selections in this plan conflict with the currently selected option
                angular.forEach(option.plan.planOptionList, function(po) {
                    if (po.planOptionId !== option.planOptionId && po.isSelected && po.hasOverlappingCoverageWith(option)) {
                        po.isSelected = false;
                    }
                });
            }

            if (option.dependentOptions) {

                if (option.isSelected) {

                    // TODO: Recalc sub-options
                    let selections = [];
                    // see if any selections in this plan conflict with the currently selected option
                    angular.forEach(option.plan.planOptionList, function(po) {
                        if (po.isSelected) {
                            selections.push({
                                planId:                      option.planId,
                                planOptionId:                option.planOptionId,
                                employeeId:                  context.employeeInfo.employeeId,
                                benefitAmountId:             option.selection.selectedBenefitAmount.benefitAmountId,
                                benefitAmountValue:          option.selection.selectedBenefitAmount.benefitAmount,
                                alternateBenefitAmountValue: option.selection.selectedBenefitAmount.calculatedBenefitAmount
                            });
                        }

                        po.isLoading = true;
                    });

                    msg.loading(true);

                    benefitSvc.getUpdatedPlanOptionCosts(
                        context.employeeInfo.employeeId,
                        context.selectedOpenEnrollment.openEnrollmentId,
                        option.plan.raw(),
                        selections
                    ).then(function(updatedPlan) {

                        // update benefit amounts with new costs/options
                        angular.forEach(updatedPlan.planOptionList, function(po) {
                            let currentPo = _.find(option.plan.planOptionList, {planOptionId: po.planOptionId});
                            let changed = false;

                            if (currentPo) {
                                angular.forEach(po.benefitAmounts, function(ba) {
                                    let currentBa = _.find(currentPo.benefitAmounts, {benefitAmountId: ba.benefitAmountId});

                                    if (currentBa) {
                                        if (currentBa.cost !== ba.cost) {
                                            changed = true;
                                        }

                                        angular.extend(currentBa, ba);
                                    }
                                });

                                // something changed so clear previous selection and force EE to reselect if desired
                                if (changed && currentPo.isSelected) {
                                    currentPo.isSelected = false;
                                    currentPo.selection.selectedBenefitAmount = null;
                                }

                                currentPo.isLoading = false;
                            }
                        });

                        updateDependentOptionSelectability();
                        msg.loading(false);
                    });

                } else {
                    updateDependentOptionSelectability();
                }
            }

            function updateDependentOptionSelectability() {
                angular.forEach(option.dependentOptions, function(linked) {
                    linked.updateSelectable();

                    if ((!option.isSelected && !linked.isSelectable) ||
                        (linked.selection.selectedBenefitAmount && !linked.selection.selectedBenefitAmount.isSelectable)) {
                        linked.isSelected = false;
                        linked.selection.selectedBenefitAmount = null;
                    }
                });
            }
        };

        $scope.selectPlan = function (plan) {

            let isWaived = !plan;

            let modalInstance = planSelectionModal.open($scope.coverageType, plan, $scope.user, isWaived);

            modalInstance.result.then(
                function(data) {
                    // This gets called when they successfully submit in the modal.
                    return $scope.selectedOpenEnrollment.refreshEmployeeSelections().then(function() {

                        let isComplete = true;

                        // if this coverage category allows more than one selection...check if all plans were
                        // selected or not
                        if ($scope.coverageType.maxEmployeeSelectionsAllowed > 1) {

                            angular.forEach($scope.coverageType.plans, function(p) {
                                if (!p.isSelected)
                                    isComplete = false;
                            });
                        }

                        if (!isComplete) {
                            msg.setTemporarySuccessMessage('Your benefits selection has been updated.');
                        } else {
                            msg.setOnRouteChangeSuccessMessage('Your benefits selection has been updated.');
                            //state.router.go('ess.benefits.enrollment');
                            benefitSvc.redirect("enrollment");
                        }

                    });

                }, function (data) {
                    $scope.waivedCoverage = false;
                    $scope.hasExistingSignedRecord = false;
                    $scope.hasSuccess = false;
                    msg.clearMessage();
                    DsInlineValidatedInputService.clearAllRegistrations();
            });
        };

        $scope.returnToEnrollment = function () {
            msg.loading(true);
            context.selectedOpenEnrollment.refreshEmployeeSelections().then(function() {
                //state.router.go('ess.benefits.enrollment');
                benefitSvc.redirect("enrollment");
            });
        };

        $scope.orderPlanOptions = function (option) {
            return option.costType === 1 ? option.cost : 0;
        };

        function initPlanResources(resources) {
            angular.forEach(resources, function (rec) {

                rec.isAvailable = false;

                if (rec.isFile) {
                    rec.url = benefitSvc.getFileResourceURL(rec.resourceId);
                    benefitSvc
                        .doesFileResourceExist(rec.resourceId)
                        .then(function(response) {
                            rec.isAvailable = response.doesFileExist;
                        });

                } else {
                    rec.isAvailable = true;
                }

            });
        }

        function validateResource(resource, event) {
            if (!resource.isAvailable) {
                alert('File is not available');
                event.preventDefault();
            }
        }

        function getBenefitAmountLabel(amount, option) {
            let label = amount.getFriendlyName(option);

            if (amount.isEoiRequired) {
                label = label + '*';
            }

            return label;
        }

    }
}
