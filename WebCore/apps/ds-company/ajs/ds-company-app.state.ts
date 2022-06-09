import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { AccountService } from "@ajs/core/account/account.service";
import { HeaderController } from "./header/header.controller";

/**
 * @description
 * Configuration settings for the Company Management application module.
 */
export class DsCompanyAppDefaultState {

    static RESOLVE_KEYS = {
        userAccount: "userAccount"
    };

    //--------------------------------------------------------------------
    // ABSTRACT STATE DEFINITIONS
    // see individual <component>.state.js files for concrete state definitions
    //--------------------------------------------------------------------
    // STATE: Company | Root application state
    static STATE_CONFIG: IUiState = {
        name: 'company',
        'abstract': true,
        views: {
            '': {
                template: '<div ui-view></div>',
            },
            'header': {
                template: require('./header/header.html'),
                controller: HeaderController
            }
        },
        resolve: {
            // makes the active user account information available to all sub-states
            [DsCompanyAppDefaultState.RESOLVE_KEYS.userAccount]: [AccountService.SERVICE_NAME, function (svc: AccountService) {
                return svc.getUserInfo(true, true);
            }]
        }
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        //permissions: 'System.Administrator'
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(DsCompanyAppDefaultState.STATE_CONFIG, DsCompanyAppDefaultState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }

    static NG_STATE_CONFIG: IUiState = {
      name: 'ngx',
      views: {
        '': {
          template: ''
        }
      }
    };

    static $ng_config() {
      const config = (stateProv: DsStateServiceProvider) => {
        stateProv.registerState(DsCompanyAppDefaultState.NG_STATE_CONFIG, DsCompanyAppDefaultState.STATE_OPTIONS);
      }
      config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
      return config;
    }
}
