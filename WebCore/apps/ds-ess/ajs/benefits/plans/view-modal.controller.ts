import * as angular from "angular";
import * as benefitSvc from "@ajs/benefits/benefits.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsStateService } from "@ajs/core/ds-state/ds-state.service";

declare var _:any;
export class BenefitPlanViewModalController {
    static readonly $inject = [
        '$scope',
        '$modalInstance',
        benefitSvc.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsStateService.SERVICE_NAME,
        "coverageType",
        "isFromEnrollment"
    ];

    constructor($scope, $modalInstance, benefitSvc, MessageService, state, coverageType, isFromEnrollment) {

        var context;

        $scope.coverageType = coverageType;
        $scope.isFromEnrollment = !!isFromEnrollment;

        $scope.validateResource = validateResource;

        (function initModalState() {
            
            MessageService.loading(true);

            context = benefitSvc.getBenefitContext();

            var plans = [];

            angular.forEach(coverageType.selections, function(s) {
                if(!s.waivedCoverage && s.selectedPlan) {
                    
                    var plan = _.find(plans, { planId: s.selectedPlan.planId });
                    if(!plan) {
                        plan = s.selectedPlan;
                        plans.push(plan);
                    }
                }
            });

            $scope.plans = plans;

            angular.forEach(plans, function(p) {
                if(p.isPcpRequired) {
                    p.employeePcp = context.employeePcp;
                } 
                
                initPlanResources(p.resourceList);
            });
            
            MessageService.loading(false);
            
        })();


        $scope.editCoverage = function (ct) {
            context.selectCoverageType(ct);
            $modalInstance.dismiss();
            //state.router.go("ess.benefits.plans");
            benefitSvc.redirect("plans");
        };

        // ------------------------------------------------------------------------------------------
        // Close Modal
        // ------------------------------------------------------------------------------------------
         $scope.close = function () {
             $modalInstance.dismiss();
         };

         function initPlanResources(resources) {
             angular.forEach(resources, function (rec) {

                 rec.isAvailable = false;

                 if (rec.isFile) {
                     rec.url = benefitSvc.getFileResourceURL(rec.resourceId);
                     benefitSvc
                         .doesFileResourceExist(rec.resourceId)
                         .then(function (response) {
                             rec.isAvailable = response.doesFileExist;
                         });

                 } else {
                     rec.isAvailable = true;
                 }
             });
         }

         function validateResource(resource, event) {
             if (!resource.isAvailable) {
                 alert("File is not available");
                 event.preventDefault();
             }
         }
    }
}