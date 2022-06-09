import { IUiState, IDsUiStateOptions, DsStateServiceProvider, DsStateService } from "@ajs/core/ds-state/ds-state.service";
import { AccountService } from "@ajs/core/account/account.service";

/**
 * @description 
 * Configures the root Group Scheduling state within the Company Management application.
 */
export class CompanyLaborScheduleGroupState {
    static STATE_CONFIG: IUiState = {
        parent: 'company.labor.schedule',
        name: 'group',
        url: '/Group',
        views: {
            '@': {
                template: require('./group.html'),
                controller: [ '$scope', 'scheduleAccess', DsStateService.SERVICE_NAME, function ($scope, scheduleAccess, stateSvc: DsStateService) {
                    $scope.access = scheduleAccess;
                    $scope.isPlannerActive = function () {
                        return stateSvc.router.is('company.labor.schedule.group.plan');
                    }
                    $scope.isScheduleActive = function () {
                        return stateSvc.router.is('company.labor.schedule.group.schedule');
                    }

                        if(stateSvc.router.is('company.labor.schedule.group')) {
                            if(scheduleAccess.isPlanner) {
                                stateSvc.router.go('company.labor.schedule.group.plan');
                            } else {
                                stateSvc.router.go('company.labor.schedule.group.schedule');  
                            }
                        }
                    }
                ]
            }
        },
        resolve: {
            scheduleAccess: ['$q', AccountService.SERVICE_NAME, function ($q, accountSvc:AccountService) {
                var deferred = $q.defer(),
                    access = {
                        isPlanner: false,
                        isScheduler: false
                    };

                var plannerPromise = accountSvc.canPerformActions('LaborManagement.LaborPlanAdministrator').then(
                    function (){ access.isPlanner = true;  },
                    function (){ access.isPlanner = false; });

                var schedulerPromise = accountSvc.canPerformActions('LaborManagement.LaborScheduleAdministrator').then(
                    function (){ access.isScheduler = true;  },
                    function (){ access.isScheduler = false; });

                $q.all([plannerPromise, schedulerPromise]).then(function () {
                    deferred.resolve(access); 
                });

                return deferred.promise;
            }]
        }
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Group Scheduling',
        permissions: ['LaborManagement.LaborScheduleAdministrator']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(CompanyLaborScheduleGroupState.STATE_CONFIG, CompanyLaborScheduleGroupState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}

    