import { PlanSelectionModalController } from "./plan-selection-modal.controller";
import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import * as benefitSvc from "@ajs/benefits/benefits.service";

export class PlanSelectionModalService { 
    static readonly SERVICE_NAME = 'PlanSelectionModalService';
    static readonly $inject = [
        DsModalService.SERVICE_NAME,
        CountryStateService.SERVICE_NAME,
        benefitSvc.SERVICE_NAME
    ];

    constructor(private modal: DsModalService, private CountryStatesService: CountryStateService, private BenefitsService) {
    }
    
    open(coverageType, plan, userAccount, isWaived) {
        return this.modal.open({
            template: require('./plan-selection-modal.html'),
            controller: PlanSelectionModalController,
            resolve: {
                modalData: function() {
                    return {
                        coverageType:   coverageType,
                        plan:           plan,
                        waivedCoverage: isWaived
                    };
                },
                states: this.CountryStatesService.getStatesForUSA,
                userAccount: function () {
                    return userAccount;
                },
                waiverReasons: () => { return this.BenefitsService.getPlanWaiveReasons(); }
            }
            
        });
    }
}