import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { EssLeaveTimeoffState } from "./header.state";

//--------------------------------------------------------------------------------
// STATE: ess.leave.timeoff.activity
//--------------------------------------------------------------------------------
export class EssLeaveTimeOffActivityState {
    static STATE_CONFIG:IUiState = {
        parent: 'ess.leave.timeoff',
        name: 'activity',
        url: '/:policyName/Activity',
        views: {
            'content': {
                template: "<timeoff-activity time-off='$ctrl.timeoff'></timeoff-activity>",
                controller: [EssLeaveTimeoffState.TIMEOFF_MANAGER_RESOLVE_TOKEN, function(timeoff){
                    this.timeoff = timeoff;
                }],
                controllerAs: "$ctrl"
            }
        }
    };

    static STATE_OPTIONS:IDsUiStateOptions = {
        pageTitle: 'Time Off Activity'
    };

    static $config(){
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssLeaveTimeOffActivityState.STATE_CONFIG, EssLeaveTimeOffActivityState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
    