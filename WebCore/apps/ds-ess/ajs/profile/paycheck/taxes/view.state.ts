import { DsStateServiceProvider, IUiState, IDsUiStateOptions } from "@ajs/core/ds-state/ds-state.service";
import { EssHeaderController } from 'apps/ds-ess/ajs/common/header/header.controller';
import { MainSidebarController } from 'apps/ds-ess/ajs/common/main-sidebar/main-sidebar.controller';

export class EssPayViewState {
    static STATE_CONFIG: IUiState = {
        parent: 'ess.pay',
        name: 'view',
        'abstract': true,
        template: "<div ui-view></div>"
        // see state.js for use of resolve (shared by ess.taxes.edit & ess.taxes.view states)
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Paycheck Settings'
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssPayViewState.STATE_CONFIG, EssPayViewState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}

//--------------------------------------------------------------------------------
// STATE: ess.pay.view.all
//--------------------------------------------------------------------------------
export class EssPayViewAllState {
    static STATE_CONFIG: IUiState = {
        parent: 'ess.pay.view',
        name: 'all',
        // url:'',
        views: {
            '' : {
                template: require('./view.html')
            }
        }
    };
    static STATE_OPTIONS: IDsUiStateOptions = {
        pageTitle: 'Paycheck Settings'
    };

    static $config() {
        const config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssPayViewAllState.STATE_CONFIG, EssPayViewAllState.STATE_OPTIONS);
        };
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
