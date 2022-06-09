import { BillingFrequency } from '../../../../apps/ds-company/src/app/enums/billing-frequency.enum';
import { BillingPeriod } from '../../../../apps/ds-company/src/app/enums/billing-period.enum';
import { BillingItemDescription } from './billing-item-description.model';
import { BillingWhatToCount } from '../../../../apps/ds-company/src/app/enums/billing-what-to-count.enum';
import { BillingItemBase } from './billing-item.model';

export interface PendingBillingCredit extends BillingItemBase {
    pendingBillingCreditId  : number;
    featureOptionId         : number;
    requestedById           : number;
    needsApproval           : boolean;
    isApproved              : boolean;
    requestedByName         : string
}