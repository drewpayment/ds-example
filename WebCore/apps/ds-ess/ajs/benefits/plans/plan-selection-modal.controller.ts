import * as angular from 'angular';
import * as benefitSvc from '@ajs/benefits/benefits.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { DsInlineValidatedInputService } from '../../ui/form-validation/ds-inline-validated-input.service';
import { DsUtilityServiceProvider } from '@ajs/core/ds-utility/ds-utility.service';
import { IUserInfo } from '@ajs/user';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { AccountService } from '@ajs/core/account/account.service';

declare var _: any;
export class PlanSelectionModalController {
    static readonly $inject = [
        '$scope',
        '$modalInstance',
        'modalData',
        benefitSvc.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        AccountService.SERVICE_NAME,
        DsInlineValidatedInputService.SERVICE_NAME,
        'states',
        'waiverReasons',
        DsUtilityServiceProvider.SERVICE_NAME
    ];

    constructor (
        $scope: any,
        $modalInstance,
        modalData,
        BenefitsService,
        MessageService: DsMsgService,
        accountService: AccountService,
        DsInlineValidatedInputService: DsInlineValidatedInputService,
        states,
        waiverReasons,
        util: DsUtilityServiceProvider) {

        let context = BenefitsService.getBenefitContext();

        $scope.coverageType                      = modalData.coverageType;
        $scope.waivedCoverage                    = modalData.waivedCoverage;
        $scope.waiverInfo                        = _.find(modalData.coverageType.selections, {waivedCoverage: true, planId: null}) || {};
        $scope.states                            = states;
        $scope.employeePcp                       = context.employeePcp;
        $scope.plan                              = modalData.plan;
        $scope.employeeSelectionDependents       = [];
        $scope.planWaiveReasons                  = [];
        $scope.showPcpForm                       = false;
        $scope.selectedPcp                       = {};
        $scope.allPcpsState                      = { allPcpsSupplied : true };
        $scope.moneyValueIsValid                 = true;
        $scope.planWaiveReasonIsValid            = true;
        $scope.checkForMissingPcp                = checkForMissingPcp;
        $scope.cancelPcp                         = cancelPcp;


        let origPcp;
        let selectedDependent;
        // ------------------------------------------------------------------------------------------
        // CONTROLLER & MODAL STATE INITIALIZATION
        // ------------------------------------------------------------------------------------------
        function initModalState(userAccount) {

            MessageService.loading(true);

            $scope.currentUser = userAccount;

            // If the user is waiving coverage, we dont need to get this information.
            if ($scope.waivedCoverage) {

                // Grab the list of possible waive reasons
                $scope.planWaiveReasons = waiverReasons;

                if (!$scope.waiverInfo.planWaiveReasonId) {
                    $scope.waiverInfo.planWaiveReasonId = null;
                }

            } else {

                if ($scope.plan) {
                    $scope.checkForMissingPcp();
                }
            }

            MessageService.loading(false);
        }

        accountService.getUserInfo().then(user => initModalState(user));

        $scope.hasSelectedDependents = function(option) {
            return _.some(option.eligibleDependents, {isSelected: true});
        };

        // --------------------------------------------------------------------------------
        // This function is called when the 'Select' (submit) button is clicked.
        // Saves the selected option and opens up the modal dialog window
        // This will give confirmation to what is selected as well as select dependents if needed.
        // --------------------------------------------------------------------------------
        $scope.saveEmployeeOpenEnrollmentSelection = function () {

            MessageService.sending(true);

            let defaultDto = {
                openEnrollmentId:         context.selectedOpenEnrollment.openEnrollmentId,
                employeeOpenEnrollmentId: context.selectedOpenEnrollment.employeeOpenEnrollment.employeeOpenEnrollmentId || 0,
                coverageTypeId:           $scope.coverageType.coverageTypeId,
                employeeDependents:       [],
                employeeDependentDtosSelections: []
            };

            let dtos = [];

            if ($scope.waivedCoverage) {
                // WAIVER
                // construct and save the waiver selection record
                let waiverDto = angular.extend({}, defaultDto, util.stripNonValueProperties($scope.waiverInfo));
                waiverDto.waivedCoverage = true;

                dtos.push(waiverDto);

                // delete all other selections for this coverage type
                angular.forEach($scope.coverageType.selections, function (s) {
                    let raw = angular.extend({}, s);

                    raw.coverageTypes         = null;
                    raw.selectedOption        = util.stripNonValueProperties(raw.selectedOption);
                    raw.selectedBenefitAmount = util.stripNonValueProperties(raw.selectedBenefitAmount);
                    raw.selectedPlan          = util.stripNonValueProperties(raw.selectedPlan);

                    if (raw.employeeOpenEnrollmentSelectionId !== waiverDto.employeeOpenEnrollmentSelectionId) {
                        raw.dtoState = 2; // DELETED
                        dtos.push(raw);
                    }
                });

            } else {

                // PLAN SELECTIONS
                angular.forEach($scope.plan.planOptionList, function(o) {

                    let raw;

                    if (o.isSelected) {

                        // setup current selection to be saved
                        raw = angular.extend({}, defaultDto, o.selection);

                        raw.coverageTypes         = null;
                        raw.selectedOption        = util.stripNonValueProperties(o);
                        raw.selectedPlan          = util.stripNonValueProperties($scope.plan);
                        raw.selectedBenefitAmount = util.stripNonValueProperties(raw.selectedBenefitAmount);
                        raw.planOptionId          = o.planOptionId;
                        raw.planId                = $scope.plan.planId;
                        raw.waivedCoverage        = false;

                        if (raw.selectedBenefitAmount)
                            raw.benefitAmountId = raw.selectedBenefitAmount.benefitAmountId;

                        raw.employeeDependents = _.map(o.eligibleDependents, function(d) {
                            let dCopy = <any>util.stripNonValueProperties(d.dependent);
                            dCopy.isSelected = d.isSelected;
                            return dCopy;
                        });

                        dtos.push(raw);

                        // check if other plan's option selections need to be removed
                        if ($scope.coverageType.maxEmployeeSelectionsAllowed === 1) {
                            angular.forEach($scope.coverageType.selections, function(s) {
                                let rawOther;
                                if (s.planId && s.planId !== $scope.plan.planId && o.hasOverlappingCoverageWith(s.selectedOption)) {
                                    rawOther = angular.extend({}, s);

                                    rawOther.coverageTypes         = null;
                                    rawOther.selectedOption        = util.stripNonValueProperties(rawOther.selectedOption);
                                    rawOther.selectedBenefitAmount = util.stripNonValueProperties(rawOther.selectedBenefitAmount);
                                    rawOther.selectedPlan          = util.stripNonValueProperties(rawOther.selectedPlan);

                                    rawOther.dtoState = 2; // DELETED

                                    dtos.push(rawOther);
                                }
                            });
                        }

                    } else {

                        // check if we need to delete the option...delete if previously saved
                        if (o.selection && o.selection.employeeOpenEnrollmentSelectionId) {
                            raw = angular.extend({}, o.selection);

                            raw.coverageTypes         = null;
                            raw.selectedOption        = util.stripNonValueProperties(raw.selectedOption);
                            raw.selectedBenefitAmount = util.stripNonValueProperties(raw.selectedBenefitAmount);
                            raw.selectedPlan          = util.stripNonValueProperties(raw.selectedPlan);

                            raw.dtoState = 2; // DELETED
                            dtos.push(raw);
                        }
                    }

                });

                // delete previous waiver record if present
                let rawWaiver;
                if ($scope.waiverInfo && $scope.waiverInfo.employeeOpenEnrollmentSelectionId) {
                    rawWaiver = util.stripNonValueProperties($scope.waiverInfo);
                    rawWaiver.dtoState = 2; // DELETED
                    dtos.push(rawWaiver);
                }
            }

            // Add/update the selection and process any dependents.
            BenefitsService.putEmployeeOpenEnrollmentSelection(dtos).then(
                    // success
                    function (data) {
                        $modalInstance.close();
                    },
                    // error
                    errorCallback
            );

            return;
        };

        // ------------------------------------------------------------------------------------------
        // MODAL NAVIGATION
        // ------------------------------------------------------------------------------------------
        $scope.close = function () {
            $modalInstance.dismiss($scope.employeeDependents);
        };

        // --------------------------------------------------------
        // used for all async errors
        // --------------------------------------------------------
        function errorCallback(data, status, headers, config) {
            MessageService.setMessage(
                        window.MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                        MessageTypes.error);

            // MessageService.xhrMessageHandler(data);
        }


        $scope.savePcp = function savePcp(pcp, sourceId, sourceType) {
            let sourceTypes = context.pcpSourceTypes;

            DsInlineValidatedInputService.validateAll('addPcpForm')
                    .then(
                        function () {
                        BenefitsService.postPcp(pcp).then(
                            function(dto) {

                                $scope.showPcpForm = false;

                                // update scope & context w/ PCP changes
                                if (sourceType === sourceTypes.employee) {
                                    $scope.employeePcp = dto;
                                    context.setEmployeePcp(dto);
                                } else if (sourceType === sourceTypes.dependent) {
                                    context.setEmployeeDependentPcp(dto);
                                }

                                $scope.checkForMissingPcp();
                            },
                            errorCallback);

                    },
                    function() {
                        MessageService.setTemporaryMessage(window.MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                            MessageTypes.error,
                            2500);
                    });
        };

        $scope.loadPcp = function loadPcp(pcp, sourceId, sourceType) {

            let sourceTypes = context.pcpSourceTypes,
                isNewPcp = !pcp
                    || (sourceType === sourceTypes.employee && !pcp.employeeId)
                    || (sourceType === sourceTypes.dependent && !pcp.employeeDependentId);

            let pcpIds = { sourceId: sourceId, sourceType: sourceType, isNew: isNewPcp };

            // create a dto to bind to in the PCP modal by copying info from the saved PCP & source info
            let dto = angular.extend({}, (pcp || {}), pcpIds);

            // save the state so we know what to do when the modal is saved, then show the modal
            $scope.selectedPcp = dto;
            $scope.showPcpForm = true;
        };

        function checkForMissingPcp() {
            $scope.allPcpsState.allPcpsSupplied = true;
            if ($scope.plan.isPcpRequired) {

                angular.forEach($scope.plan.planOptionList, function(po) {

                    if (po.isSelected) {

                        // pcp is required. check each dependent for a missing PCP.
                        angular.forEach(po.eligibleDependents, function(ed) {
                            if (!ed.dependent.hasPcp && ed.isSelected) {
                                // selected dependent does not have a pcp mark as missing.
                                $scope.allPcpsState.allPcpsSupplied = false;
                            }
                        });
                    }
                });

                if ($scope.allPcpsState.allPcpsSupplied && !$scope.employeePcp) {
                    // all dependents have a pcp check if employee does.
                    $scope.allPcpsState.allPcpsSupplied = false;
                }
            }
        }

        function cancelPcp() {
            $scope.showPcpForm = false;
        }
    }
}
