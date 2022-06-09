import { IPayrollRequestItem } from './payroll-request-item.model';
import { IReview } from '@ds/performance/reviews';

export interface IPayrollRequest {
    reviewTemplateId: number,
    reviewTemplateName: string,
    meritIncreaseCount: number,
    oneTimeCount: number,
    approvedMeritIncreaseCount: number,
    approvedAdditionalEarningCount: number,
    declinedMeritIncreaseCount: number,
    declinedAdditionalEarningCount: number,
    pendingMeritIncreaseCount: number,
    pendingAdditionalEarningCount: number,
    reviews: IReview[],
    payrollRequestItems: IPayrollRequestItem[]
}