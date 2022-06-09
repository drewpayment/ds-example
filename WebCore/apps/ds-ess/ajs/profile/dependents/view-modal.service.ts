import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { DependentViewModalController } from "./view-modal.controller";

export class DependentViewModalService {
    static readonly SERVICE_NAME = 'DependentViewModalService';
    static readonly $inject = [DsModalService.SERVICE_NAME];

    constructor(private modal: DsModalService) {
    }

    open(allDependents, selectedDependent) {
        return this.modal.open({
            template: require('./view-modal.html'),
            controller: DependentViewModalController,
            resolve: {
                modalData: function() {
                    return {
                        dependents: allDependents,
                        selected: selectedDependent
                    };
                }
            }
        });
    }
}