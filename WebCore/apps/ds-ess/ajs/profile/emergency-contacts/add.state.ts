import { IDsUiState, IUiState, DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { AddEmployeeEmergencyContactController } from "./add.controller";

//--------------------------------------------------------------------------------
// STATE: ess.profile.emergencyContacts.add
//--------------------------------------------------------------------------------
export class EssProfileEmergencyContactsAddState {
    static STATE_CONFIG: IUiState = {
        parent: 'ess.profile.emergencyContacts',
        name: 'add',
        url: '/Add',
        controller: AddEmployeeEmergencyContactController,
        template: require('./add.html')
    };
    static STATE_OPTIONS = {
        pageTitle: 'Add Emergency Contact',
        permissions: ['Employee.EmployeeEmergencyContactUpdate']
    };

    static $config() {
        let config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssProfileEmergencyContactsAddState.STATE_CONFIG, EssProfileEmergencyContactsAddState.STATE_OPTIONS);
        }
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}