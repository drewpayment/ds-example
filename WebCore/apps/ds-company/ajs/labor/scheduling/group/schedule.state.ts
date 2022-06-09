import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { GroupScheduleSchedulingController } from "./schedule.controller";
import { DsPlannerGroupService } from "@ajs/labor/group-scheduler/planner/schedule-group.service";
import { AccountService } from "@ajs/core/account/account.service";

/**
 * @description 
 * Configures the Group Schedule Scheduling state within the Company Management application. Allows users to assign
 * employees to a group schedule.
 */
export class CompanyLaborScheduleGroupScheduleState {
    static STATE_CONFIG: IUiState = {
        parent: 'company.labor.schedule.group',
        name: 'schedule',
        url: '/Scheduling',
        template: require('./schedule.html'),
        controller: GroupScheduleSchedulingController,
        resolve: {
            scheduleGroups: ['userAccount', DsPlannerGroupService.SERVICE_NAME, function (userAccount, svc:DsPlannerGroupService) {
                return svc.getList(); 
            }],
            rateAccess: ['$q', AccountService.SERVICE_NAME, function ($q, accountSvc: AccountService) {
                var deferred = $q.defer(),
                    access = {
                        canViewRates: false
                    };

                var viewRatesPromise = accountSvc.canPerformActions('ClientRate.ViewHourlyRates').then(
                    function (){ access.canViewRates = true;  },
                    function (){ access.canViewRates = false; });

                $q.all([viewRatesPromise]).then(function () {
                    deferred.resolve(access); 
                });

                return deferred.promise;
            }],
            budgetAccess: ['$q', AccountService.SERVICE_NAME, function ($q, accountSvc: AccountService) {
                var deferred = $q.defer(),
                    access = {
                        canViewBudgeting: false
                    };

                var viewBudgetingPromise = accountSvc.canPerformActions('Budgeting.BudgetingAdministrator').then(
                    function (){ access.canViewBudgeting = true;  },
                    function (){ access.canViewBudgeting = false; });

                $q.all([viewBudgetingPromise]).then(function () {
                    deferred.resolve(access); 
                });

                return deferred.promise;
            }]
        }
    };

    static STATE_OPTIONS: IDsUiStateOptions = {
        permissions: ['LaborManagement.LaborScheduleAdministrator']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(CompanyLaborScheduleGroupScheduleState.STATE_CONFIG);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}

    