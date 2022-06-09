import { IEvaluation } from '@ds/performance/evaluations';
import { IReviewStatus } from '..';

export interface IReviewSummaryDialogResult {
    selectedEvalToView?: IEvaluation;
    reviewStatus?: IReviewStatus;
}
