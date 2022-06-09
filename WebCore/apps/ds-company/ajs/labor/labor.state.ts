import { IUiState, IDsUiStateOptions, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";

/**
 * @description 
 * Configures the root LaborSource state within the Company Management application.
 */
export class CompanyLaborState {
    static STATE_CONFIG: IUiState = {
        parent: 'company',
        name: 'labor',
        url: '/Labor',
        'abstract': true,
        data: {
            menuName: 'Labor Source'
        }
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(CompanyLaborState.STATE_CONFIG);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}

    