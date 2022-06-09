import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { TimeOffManagerService } from "./services/manager.service";
import { IUserInfo } from "@ajs/user";

//--------------------------------------------------------------------------------
// STATE: ess.leave.timeoff
//--------------------------------------------------------------------------------
export class EssLeaveTimeoffState {
    static TIMEOFF_MANAGER_RESOLVE_TOKEN = 'timeOffManager';
    static STATE_CONFIG: IUiState = {
        parent: 'ess.leave',
        name: 'timeoff',
        url: '',
        views: {
            '': {
                template: "<timeoff-header time-off='$ctrl.timeoff'></timeoff-header>",
                controller: [EssLeaveTimeoffState.TIMEOFF_MANAGER_RESOLVE_TOKEN, function(timeoff){
                    this.timeoff = timeoff;
                }],
                controllerAs: "$ctrl"
            },
            'content@ess.leave.timeoff' : {
                template: "<timeoff-summary time-off='$ctrl.timeoff'></timeoff-summary>",
                controller: [EssLeaveTimeoffState.TIMEOFF_MANAGER_RESOLVE_TOKEN, function(timeoff){
                    this.timeoff = timeoff;
                }],
                controllerAs: "$ctrl"
            },
            'calendar@ess.leave.timeoff' : {
                template: require('./calendar.html')
            }
        },
        resolve: {
            [EssLeaveTimeoffState.TIMEOFF_MANAGER_RESOLVE_TOKEN]: ['userAccount', TimeOffManagerService.SERVICE_NAME, function (user: IUserInfo, svc:TimeOffManagerService) {
                return svc.manageUserTimeOff(user);
            }]
        }
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Time Off'
    };
}
