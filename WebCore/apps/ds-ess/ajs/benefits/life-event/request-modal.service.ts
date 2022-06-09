import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { IUserInfo } from "@ajs/user";
import { BenefitLifeEventRequestModalController } from "./request-modal.controller";

export class BenefitLifeEventRequestModalService {
    static readonly $inject = [DsModalService.SERVICE_NAME];
    static readonly SERVICE_NAME = 'BenefitLifeEventRequestModalService';

    constructor(private modal: DsModalService){
    }

    open(userAccount: IUserInfo) {
        return this.modal.open({
            template: require('./request-modal.html'),
            controller: BenefitLifeEventRequestModalController,
            resolve: {
                userAccount: function () { return userAccount; }
            }
        });
    }
}