import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { EmergencyContactViewModalController } from "./view-modal.controller";

export class EmergencyContactViewModalService {
    static readonly SERVICE_NAME = 'EmergencyContactViewModalService';
    static readonly $inject = [DsModalService.SERVICE_NAME];

    constructor(private modal:DsModalService){
    }

    open(allContacts, selectedContact) {
        return this.modal.open({
            template: require('./view-modal.html'),
            controller: EmergencyContactViewModalController,
            resolve: {
                modalData: function() {
                    return {
                        emergencyContacts: allContacts,
                        selected: selectedContact
                    };
                }
            }
        });
    }
}