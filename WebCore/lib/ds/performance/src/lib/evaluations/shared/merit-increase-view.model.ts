import { IMeritIncreaseCurrentPayAndRates } from "./merit-increase-current-pay-and-rates.model";
import { IMeritLimit } from "./merit-limit.model";
import { OneTimeEarning } from '@ds/performance/competencies/shared/one-time-earning.model';

export interface IMeritIncreaseView {
    payrollRequestEffectiveDate?: Date | string;
    recommendedMeritPercent?: number;
    canViewRates: boolean;
    currentPayInfo: IMeritIncreaseCurrentPayAndRates[];
    oneTimeEarning: OneTimeEarning;
    meritRecommendations?: IMeritLimit[];
}