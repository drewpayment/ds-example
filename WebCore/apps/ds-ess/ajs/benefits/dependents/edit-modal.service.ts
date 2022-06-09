import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { IEmployeeDependent } from "@ajs/employee/models";
import { BenefitDependentEditModalController } from "./edit-modal.controller";

export class BenefitDependentEditModalService {
    static readonly SERVICE_NAME = 'BenefitDependentEditModalService';
    static readonly $inject = ['DsModal'];

    constructor(private modal: DsModalService) {
    }

    open(dependent: IEmployeeDependent) {
        return this.modal.open({
            template: require('./edit-modal.html'),
            controller: BenefitDependentEditModalController,
            resolve: {  
                dependent: function () {
                    return dependent;
                }
            }
        });
    }
}