import { STATES } from "../../shared/state-router-info";
import * as benefitSvc from "@ajs/benefits/benefits.service";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { IUserInfo } from "@ajs/user";
import { BenefitLifeEventRequestModalService } from "../life-event/request-modal.service";
import { BenefitPlanViewModalService } from "../plans/view-modal.service";
import { AccountService } from '@ajs/core/account/account.service';

declare var _:any;
export class BenefitHomeController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        benefitSvc.SERVICE_NAME,
        DsStateService.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        BenefitPlanViewModalService.SERVICE_NAME,
        BenefitLifeEventRequestModalService.SERVICE_NAME
    ];

    constructor(
        $scope,
        accountService: AccountService,
        benefitSvc,
        state,
        MessageService,
        DsInlineValidatedInputService,
        planViewModal,
        lifeEventModal: BenefitLifeEventRequestModalService
    ) {

        var context;

        $scope.openEnrollments        = [];
        $scope.activeEmployeeBenefits = [];
        $scope.hasLifeEvent           = false;
        $scope.noCurrentBenefits      = true;
        $scope.userData               = {};

        $scope.selectEnrollment = function (openEnrollment, activeState) {
            context.selectEnrollment(openEnrollment);

            if(activeState === 1) {
                //state.router.go("ess.benefits.info");
                benefitSvc.redirect("info");
            } else {
                //state.router.go("ess.benefits.enrollment");
                benefitSvc.redirect("enrollment");
            }
        };

        function init() {
            $scope.isLoading = true;
            MessageService.loading(true);

            accountService.getUserInfo().then(function(userInfo) {
                $scope.userData = userInfo;

                benefitSvc.refreshBenefitContext(userInfo).then(function(ctx) {
                    context = ctx;

                    //open enrollments
                    $scope.openEnrollments = ctx.openEnrollmentInfo;
                    $scope.hasLifeEvent    = _.some(ctx.openEnrollmentInfo, {isLifeEvent:true, isDeclined:false});

                    //active benefit selections
                    $scope.activeEmployeeBenefits = ctx.currentBenefitInfo;
                    $scope.noCurrentBenefits = ctx.currentBenefitInfo.length === 0 || _.every(ctx.currentBenefitInfo, "isWaived");

                    $scope.isLoading = false;
                    MessageService.loading(false);
                });
            });
        }
        init();

        // --------------------------------------------------------------------------------------------
        // View Benefit MODAL DIALOG HANDLING
        // --------------------------------------------------------------------------------------------
        $scope.selectBenefit = function (coverageType) {
            planViewModal.open(coverageType, false);
        };
        // --------------------------------------------------------------------------------------------
        // Life Event MODAL DIALOG HANDLING
        // --------------------------------------------------------------------------------------------
        $scope.requestLifeEvent = function() {
            //Modal Popup for life event
            var modalInstance = lifeEventModal.open($scope.userData);

            modalInstance.result.then(
                function(data) {
                    //This gets called when they successfully submit in the modal.
                    $scope.waivedCoverage = false;
                    init();
                    //benefitSvc.loadOpenEnrollmentForDate(data.clientId, data.employeeId).then(enrollmentsLoaded);

                }, function(data) {
                    $scope.waivedCoverage = false;
                    DsInlineValidatedInputService.clearAllRegistrations();
            });
        };

    }
}
