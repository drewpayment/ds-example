import { IUiState, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";

/**
 * @description 
 * Configures the root LaborSource Scheduling state within the Company Management application.
 */
export class CompanyLaborScheduleState {
    static STATE_CONFIG: IUiState = {
        'abstract': true,
        parent: 'company.labor',
        name: 'schedule',
        url: '/Schedule'
    };
    
    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(CompanyLaborScheduleState.STATE_CONFIG);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
    