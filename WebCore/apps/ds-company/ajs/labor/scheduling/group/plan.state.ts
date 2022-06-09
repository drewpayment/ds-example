import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { GroupSchedulePlanningController } from "./plan.controller";
import { DsPlannerGroupScheduleService } from "@ajs/labor/group-scheduler/planner/group-schedule.service";

/**
 * @description
 * Configures the Group Schedule Planning state within the Company Management application.
 */
export class CompanyLaborScheduleGroupPlanState {
    static STATE_CONFIG: IUiState = {
        parent: 'company.labor.schedule.group',
        name: 'plan',
        url: '/Planning',
        template: require('./plan.html'),
        controller: GroupSchedulePlanningController,
        resolve: {
            schedules: ['userAccount', DsPlannerGroupScheduleService.SERVICE_NAME, function (userAccount, svc:DsPlannerGroupScheduleService) {
                return svc.getList(true);
            }]
        }
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        permissions: ['LaborManagement.LaborPlanAdministrator']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(CompanyLaborScheduleGroupPlanState.STATE_CONFIG);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
