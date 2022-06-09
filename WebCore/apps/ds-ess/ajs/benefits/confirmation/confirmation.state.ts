import { IUiState, DsStateServiceProvider } from '@ajs/core/ds-state/ds-state.service';
import { BenefitConfirmationController } from './confirmation.controller';

// --------------------------------------------------------------------------------
// STATE: ess.benefit.confirmation
// --------------------------------------------------------------------------------
export class EssBenefitsConfirmationState {
    static COMPONENT_NAME = 'ajsEssBenefitsConfirmation';
    static COMPONENT_OPTIONS: ng.IComponentOptions = {
        controller: BenefitConfirmationController,
        template: require('./confirmation.html'),
    };
    static STATE_CONFIG: IUiState = {
        parent: 'ess.benefits',
        name: 'confirmation',
        url: '/confirmation',
        template: require('./confirmation.html'),
        controller: BenefitConfirmationController
    };

    static STATE_OPTIONS: {
        pageTitle: 'Benefit Confirmation',
        permissions: ['Benefit.GetPlans']
    };

    static $config() {
        const config = (stateProv: DsStateServiceProvider) => {
            stateProv.registerState(EssBenefitsConfirmationState.STATE_CONFIG, EssBenefitsConfirmationState.STATE_OPTIONS);
        };
        config.$inject = [DsStateServiceProvider.PROVIDER_NAME];
        return config;
    }
}
