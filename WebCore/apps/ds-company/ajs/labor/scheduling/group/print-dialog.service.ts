import * as angular from "angular";
import { DsModalService } from "@ajs/ui/modal/ds-modal.service";
import { GroupSchedulePrintDialogController } from "./print-dialog.controller";

export class GroupSchedulePrintDialogService {
    static readonly SERVICE_NAME = 'GroupSchedulePrintDialogService';
    static readonly $inject = [
        DsModalService.SERVICE_NAME
    ];

    constructor (private modal: DsModalService) {

    }

    open(settings) {
        return this.modal.open({
            template: require('./print-dialog.html'),
            controller: GroupSchedulePrintDialogController,
            windowClass: 'info-dialog',
            resolve: {
                settings: function () {
                    var defaults = {
                        includeOtherShifts: false,
                        includeJobTitles: true
                    };

                    settings = settings || {};

                    return angular.extend(defaults, settings);
                }
            }
        });
    }
}