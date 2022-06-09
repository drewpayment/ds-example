import { STATES } from "../../shared/state-router-info";
import * as benefitSvc from "@ajs/benefits/benefits.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { IUserInfo } from "@ajs/user";
import { BenefitPlanViewModalService } from "../plans/view-modal.service";
import { AccountService } from '@ajs/core/account/account.service';

export class BenefitEnrollmentController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        benefitSvc.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsStateService.SERVICE_NAME,
        BenefitPlanViewModalService.SERVICE_NAME
    ];

    constructor(
        $scope: any,
        accountService: AccountService,
        benefitSvc,
        MessageService: DsMsgService,
        state: DsStateService,
        planViewModal) {

        var context;

        $scope.hasError                                         = false;
        $scope.isLoading                                        = true;
        $scope.selectedPlanCategory                             = {};
        $scope.selectedOpenEnrollment                           = {};
        $scope.lifeEventList                                    = [];
        $scope.userData                                         = {};
        $scope.nextPage                                         = "summary";
        $scope.selectedEmployeeOpenEnrollment                   = {};
        $scope.isSigned                                         = false;
        $scope.dateSigned                                       = "";
        $scope.totalCost                                        = 0.00;
        $scope.hasNoWaivedPlans                                 = true;

        function init(data) {

            context = benefitSvc.getBenefitContext();

            if (!context.hasSelectedEnrollment()) {
                //state.router.go("ess.benefits.home");
                benefitSvc.redirect("home");
                return;
            }

            $scope.userData = data;

            $scope.selectedOpenEnrollment = context.selectedOpenEnrollment;
            $scope.hasNoWaivedPlans       = $scope.selectedOpenEnrollment.hasNoWaivedPlans;
            $scope.isSigned               = $scope.selectedOpenEnrollment.isSigned;
            $scope.dateSigned             = $scope.selectedOpenEnrollment.dateSigned;

            $scope.isLoading = false;

        }

        /**
         * CALL INIT AFTER WE GET USER
         */
        accountService.getUserInfo()
            .then(user => init(user));


        $scope.editCoverage = function (coverageType) {
            context.selectCoverageType(coverageType);
            //state.router.go("ess.benefits.plans");
            benefitSvc.redirect("plans");
        };

        $scope.moveToNextPage = function () {
            if($scope.selectedOpenEnrollment.allCategoriesSelected){
                //state.router.go("ess.benefits.summary");
                benefitSvc.redirect("summary");
            }
        };

        // --------------------------------------------------------------------------------------------
        // View Benefit MODAL DIALOG HANDLING
        // --------------------------------------------------------------------------------------------
        $scope.selectBenefit = function (coverageType) {
            planViewModal.open(coverageType, true);
        };

        $scope.availableTypesFilter = (item: any): boolean => {
            return (!item.isWaived && item.isOffered && item.getPlanCount() > 0 );
        };
        $scope.waivedTypesFilter = (item: any): boolean => {
            return (item.isWaived && item.isOffered && item.getPlanCount() > 0 );
        };
    }
}
