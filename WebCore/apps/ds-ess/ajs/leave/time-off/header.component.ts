import { TimeOffManagerInstance, TimeOffManagerService } from './services/manager.service';
import { EssLeaveTimeoffState } from './header.state';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { DsStateService } from '@ajs/core/ds-state/ds-state.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';

export class TimeOffHeaderController {
    static readonly COMPONENT_NAME = 'timeoffHeader';
    static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: TimeOffHeaderController,
        template: require('./header.html'),
        bindings: {
            timeOff: '='
        }
    };
    static readonly $inject = [
        '$scope',
        DsMsgService.SERVICE_NAME,
        DsStateService.SERVICE_NAME
    ];

    timeOff: TimeOffManagerInstance;

    constructor($scope, msg: DsMsgService, private dsState: DsStateService) {
        $scope.$on(TimeOffManagerService.TIMEOFF_MANAGER_UPDATED, function (event) {
            msg.setTemporaryMessage('Time off updated successfully.', MessageTypes.success);
        });
    }

    requestNew() {
        return this.timeOff.addTimeOffEvent(this.timeOff.getActivePolicy()).then(() => {
             this.dsState.reload();
        });
    }
}
