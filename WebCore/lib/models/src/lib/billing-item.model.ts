import { BillingItemDescription } from './billing-item-description.model';
import { BillingPriceChart } from './billing-price-chart.model';
import { BillingFrequency } from '../../../../apps/ds-company/src/app/enums/billing-frequency.enum';
import { BillingWhatToCount } from '../../../../apps/ds-company/src/app/enums/billing-what-to-count.enum';
import { BillingPeriod } from '../../../../apps/ds-company/src/app/enums/billing-period.enum';

export interface BillingItem extends BillingItemBase {
    billingItemId: number;
}

export interface BillingItemBase {
    clientId: number;
    billingItemDescriptionId: number;
    billingPriceChartId: number;
    billingPriceChart: BillingPriceChart;
    billingFrequency: BillingFrequency;
    line: number;
    flat: number;
    perQty: number;
    billingWhatToCount: BillingWhatToCount;
    comment: string;
    isOneTime: Boolean;
    payrollId: number;
    modified: Date;
    billingPeriod: BillingPeriod;
    billingYear: number;
    isStopDiscount: Boolean;
    billingItemDescription: BillingItemDescription;
    startBilling: Date;
    modifiedBy: string | number;
    note?: string;
}

export enum ApplicantCorrespondenceTypeEnum {
    applicationResponse = 1,
    applicationCompleted = 2,
    applicationDisclaimer = 3,
    onboardingInvitation = 4,
}