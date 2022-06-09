import { BillingItemDescription } from './billing-item-description.model';
import { BillingFrequency } from '../../../../apps/ds-company/src/app/enums/billing-frequency.enum';

export interface AutomaticBilling {
    automaticBillingId: number;
    featureOptionId: number;
    billingItemDescriptionId: number;
    billingFrequency: BillingFrequency;
    line: number;
    flat: number;
    perQty: number;
    billingWhatToCountId: number;
    billingItemDescription: BillingItemDescription;
}