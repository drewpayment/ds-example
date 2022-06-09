import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { BenefitPlanViewModalController } from "./view-modal.controller";

export class BenefitPlanViewModalService {
    static readonly SERVICE_NAME = 'BenefitPlanViewModalService';
    static readonly $inject = [DsModalService.SERVICE_NAME];

    constructor(private modal:DsModalService){

    }
    open(coverageType, isFromEnrollment:boolean) {
        return this.modal.open({
            template: require('./view-modal.html'),
            controller: BenefitPlanViewModalController,
            windowClass: 'benefit-modal',
            resolve: {
                coverageType: function () {
                    return coverageType;
                },
                isFromEnrollment: function() {
                    return isFromEnrollment;
                }
            }
        });
    }
}