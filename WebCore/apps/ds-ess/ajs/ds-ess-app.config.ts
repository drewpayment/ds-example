import { DsStateServiceProvider, IUiState, IDsUiStateOptions, IDsUiState } from '@ajs/core/ds-state/ds-state.service';
import { DsConfigurationServiceProvider } from '@ajs/core/ds-configuration/ds-configuration.service';
import { AccountService } from '@ajs/core/account/account.service';
import { DsStyleLoaderService } from '@ajs/ui/ds-styles/ds-styles.service';
import { STATES } from './shared/state-router-info';
import { EssHeaderController } from './common/header/header.controller';
import { EssOnboardingHeaderController } from './common/header/headerOnboarding.controller';
import { MainSidebarController } from './common/main-sidebar/main-sidebar.controller';
import { WorkflowSidebarController } from './common/main-sidebar/workflow-sidebar.controller';
import { EssLayoutController } from './layout/ess-layout.controller';
import { EssLeaveTimeoffState } from './leave/time-off/header.state';
import { TimeOffManagerService } from './leave/time-off/services/manager.service';
import { IUserInfo } from '@ajs/user';
import { EssBenefitsHomeState } from './benefits/home/home.state';
import { EssBenefitsInfoState } from './benefits/info-review/info-review.state';
import { EssBenefitsConfirmationState } from './benefits/confirmation/confirmation.state';
import { EssBenefitsEnrollmentState } from './benefits/enrollment/enrollment.state';
import { EssBenefitsPlansState } from './benefits/plans/plans.state';
import { EssBenefitsSummaryState } from './benefits/summary/summary.state';

DsEssAppAjsModuleConfig.$inject = [
    DsStateServiceProvider.PROVIDER_NAME,
    '$urlRouterProvider',
    '$locationProvider',
    DsConfigurationServiceProvider.PROVIDER_NAME,
    '$urlMatcherFactoryProvider'
];

/**
 * @ngdoc config
 * @name ds.ess.app
 *
 * @param DsStateProvider - State provider used to link HTML views, controllers
 *      and urls to a give application state.
 * @param $urlRouterProvider - Angular-UI's url provider used to specify a default route when
 *      an unknown state is specified.
 * @param $locationProvider - Angular's location provider used to configure how the
 *		current URL will be displayed to the end user.
 *
 * @description
 * Configures routes & URL scheme for the ESS application.
 */
export function DsEssAppAjsModuleConfig(
    dsStateProvider: DsStateServiceProvider,
    $urlRouterProvider,
    $locationProvider: ng.ILocationProvider,
    configProvider: DsConfigurationServiceProvider,
    $urlMatcherFactory: any
) {
    $urlMatcherFactory.caseInsensitive(true);
    $urlMatcherFactory.strictMode(false);

    // set base API URL used for all service calls
    configProvider.setApiBaseUrl('api');
    // set the application's name (used in each state's page title)
    configProvider.setApplicationName('ESS');

    // --------------------------------------------------------------------
    // ABSTRACT STATE DEFINITIONS
    // see individual <component>.state.js files for concrete state definitions
    // --------------------------------------------------------------------
    // STATE: ess
    const headerTemplate = {
        template: require('./common/header/header.html'),
        controller: EssHeaderController
    };
    const sidebarTemplate = {
        template: require('./common/main-sidebar/main-sidebar.html'),
        controller: MainSidebarController
    };

    const ngx: IDsUiState = {
        name: 'ng',
        url: '/ng{children:.*}',
        template: '',
    };

    const layout: IDsUiState = {
        name: 'ess',
        abstract: true,
        template: require('./layout/ess-layout.html'),
        controller: EssLayoutController,
        controllerAs: '$ctrl',
        resolve: {
            // makes the active user account information available to all sub-states
            [STATES.ds.ess.RESOLVE.availableAssets]: [DsStyleLoaderService.SERVICE_NAME, function(svc: DsStyleLoaderService) {
                return svc.resolveAvailableStyles();
            }]
        },
        data: {
            menuName: STATES.ds.ess.MENU_NAME
        }
    };

    const onboarding: IDsUiState = {
        parent: layout,
        name: 'onboarding',
        'abstract': true,
        url: '/onboarding',
        views: {
            'header': {
                template: require('./common/header/headerOnboarding.html'),
                controller: EssOnboardingHeaderController
            },
            '': {
                template: '<div class="onboarding" ui-view></div>'
            },
            'sidebar': {
                template: require('./common/main-sidebar/workflow-sidebar.html'),
                controller: WorkflowSidebarController
            }
        },
        data: {
            menuName: STATES.ds.ess.onboarding.MENU_NAME
        }
    };

    const benefits: IDsUiState = {
        parent: 'ess',
        name: 'benefits',
        url: '/benefits',
        views: {
            '' : {
                template: "<div ui-view></div>"
            },
            'sidebar' : {
                template: ''
            }
        },
        data: {
            menuName: STATES.ds.ess.benefits.MENU_NAME
        },
    };
    const benefitsOptions: IDsUiStateOptions = {
        permissions: ['Benefit.GetPlans']
    };

    const performance: IDsUiState = {
        parent: 'ess',
        name: 'performance',
        url: '/performance{path:.*}',
        views: {
            'sidebar': {
                template: '<ess-sidebar></ess-sidebar>'
            },
            '': {
                template: '<div class="row"><div class="col-md-12"><ds-performance></ds-performance></div></div>'
            }
        },
        data: {
            stylesheet: 'main'
        }
    };

    const performanceGoalsShim: IDsUiState = {
        parent: 'ess.performance',
        name: 'goals',
        url: '/performance/goals',
        views: {
            '': {
                template: '',
            }
        }
    };

    const leaveManagement: IDsUiState = {
        parent: 'ess',
        name: 'leave',
        'abstract': true,
        url: '/timeoff',
        views: {
            'sidebar': sidebarTemplate,
            '': {
                template: '<div ui-view=""></div>'
            }
        },
        resolve: {
            [STATES.ds.ess.RESOLVE.userAccount]: [AccountService.SERVICE_NAME, function (svc: AccountService) {
                return svc.getUserInfo();
            }],
            [EssLeaveTimeoffState.TIMEOFF_MANAGER_RESOLVE_TOKEN]:
                ['userAccount', TimeOffManagerService.SERVICE_NAME, function (user: IUserInfo, svc: TimeOffManagerService) {
                    return svc.manageUserTimeOff(user);
                }
            ]
        },
        data: {
            menuName: STATES.ds.ess.MENU_NAME
        }
    };

    const leaveOptions: IDsUiStateOptions = {
        pageTitle: 'Time Off'
    };

    const timeoff: IDsUiState = {
        parent: 'ess.leave',
        name: 'timeoff',
        url: '',
        views: {
            '': {
                template: '<timeoff-header time-off=\'$ctrl.timeoff\'></timeoff-header>',
                controller: [EssLeaveTimeoffState.TIMEOFF_MANAGER_RESOLVE_TOKEN, function (to) {
                    this.timeoff = to;
                }],
                controllerAs: '$ctrl'
            },
            'content@ess.leave.timeoff': {
                template: '<timeoff-summary time-off=\'$ctrl.timeoff\'></timeoff-summary>',
                controller: [EssLeaveTimeoffState.TIMEOFF_MANAGER_RESOLVE_TOKEN, function(to) {
                    this.timeoff = to;
                }],
                controllerAs: '$ctrl'
            },
            'calendar' : {
                template: require('./leave/time-off/calendar.html')
            }
        }
    };

    const timeoffActivity: IUiState = {
        parent: 'ess.leave.timeoff',
        name: 'activity',
        url: '/:policyName/Activity',
        views: {
            'sidebar': {
                template: sidebarTemplate
            },
            'content': {
                template: '<timeoff-activity time-off=\'$ctrl.timeoff\'></timeoff-activity>',
                controller: [EssLeaveTimeoffState.TIMEOFF_MANAGER_RESOLVE_TOKEN, function(to) {
                    this.timeoff = to;
                }],
                controllerAs: '$ctrl'
            }
        }
    };

    const profile: IDsUiState = {
        parent: 'ess',
        name: 'profile',
        url: '/profile',
        views: {
            sidebar: sidebarTemplate,
            '': {
                template: require('./profile/home/view.html')
            }
        },
        data: {
            menuName: STATES.ds.ess.MENU_NAME
        }
    };

    const resources: IDsUiState = {
        parent: 'ess',
        name: 'resources',
        url: '^/resources',
        views: {
            'sidebar': {
                template: require('./common/main-sidebar/main-sidebar.html'),
                controller: MainSidebarController
            },
            '': {
                template: require('./profile/resources/resources.html'),
            }
        },
    };
    const resourcesOptions: IDsUiStateOptions = {
        pageTitle: 'Resources'
    };

    const consents: IDsUiState = {
        parent: 'ess',
        name: 'consents',
        url: '^/consents',
        views: {
            'sidebar': {
                template: require('./common/main-sidebar/main-sidebar.html'),
                controller: MainSidebarController
            },
            '': {
                template: '<div ui-view=""></div>'
            }
        },
    };
    const consentsOptions: IDsUiStateOptions = {
        pageTitle: 'Electronic Consents'
    };

    const notes: IDsUiState = {
        parent: 'ess',
        name: 'notes',

        url: '/notes',
        views: {
            '' : {
                template: require('./profile/notes/notes.html'),
            },
            'sidebar' : {
                template: ''
            },
        },
        data: {
            menuName: STATES.ds.ess.notes.MENU_NAME
        },
    };
    const notesOptions: IDsUiStateOptions = {
        pageTitle: 'Notes'
    };

    const pay: IDsUiState = {
        parent: 'ess',
        name: 'pay',
        url: '/pay',
        template: '<div ui-view></div>',
        views: {
            '': {
                template: require('./profile/paycheck/taxes/view.html')
            },
            'sidebar': {
                template: require('./common/main-sidebar/main-sidebar.html'),
                controller: MainSidebarController
            }
        },
        data: {
            menuName: STATES.ds.ess.MENU_NAME
        }
    };

    const accountSettings: IUiState = {
        parent: 'ess',
        name: 'account',
        url: '/account',
        views: {
            '': {
                template: require('./profile/auth-settings/auth-settings.html')
            },
            'sidebar': {
                template: require('./common/main-sidebar/main-sidebar.html'),
                controller: MainSidebarController
            }
        }
    };

    const accountSettingsOptions: IDsUiStateOptions = {
        pageTitle: 'Account Settings',
        permissions: ['User.ChangeOwnPassword']
    };

    dsStateProvider
        .register(ngx)
        .register(layout)
        .register(onboarding)
        .register(benefits, benefitsOptions)
        .register(EssBenefitsHomeState.STATE_CONFIG, EssBenefitsHomeState.STATE_OPTIONS)
        .register(EssBenefitsInfoState.STATE_CONFIG, EssBenefitsInfoState.STATE_OPTIONS)
        .register(EssBenefitsConfirmationState.STATE_CONFIG, EssBenefitsConfirmationState.STATE_OPTIONS)
        .register(EssBenefitsEnrollmentState.STATE_CONFIG, EssBenefitsEnrollmentState.STATE_OPTIONS)
        .register(EssBenefitsPlansState.STATE_CONFIG, EssBenefitsPlansState.STATE_OPTIONS)
        .register(EssBenefitsSummaryState.STATE_CONFIG, EssBenefitsSummaryState.STATE_OPTIONS)
        .register(performance, { pageTitle: 'Performance' })
        .register(performanceGoalsShim, { pageTitle: 'Goals' })
        .register(leaveManagement, leaveOptions)
        .register(timeoff, { pageTitle: 'Time Off' })
        .register(timeoffActivity, { pageTitle: 'Time Off' })
        .register(profile, { pageTitle: 'Profile' })
        .register(resources, resourcesOptions)
        .register(consents, consentsOptions)
        .register(notes, notesOptions)
        .register(pay, { pageTitle: 'Paycheck Settings' })
        .register(accountSettings, accountSettingsOptions);

    // --------------------------------------------------------------------
    // DEFAULT URL/STATE
    // --------------------------------------------------------------------
    $urlRouterProvider.otherwise('/profile');

    // --------------------------------------------------------------------
    // setup HTML5 url mode ('site.com/view1') for browsers which support it
    // otherwise, fallback to hash-bang url mode ('site.com/#!/view1')
    // --------------------------------------------------------------------
    $locationProvider.html5Mode(true).hashPrefix('!');
}
