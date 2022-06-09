import { IEvaluationWithStatusInfo } from "../shared/evaluation-status-info.model";
import { IReview } from "@ds/performance/reviews";
import { IChartSettings } from "../shared/chart-settings.model";

export interface IEvaluationSummaryDialogData {
    evaluation: IEvaluationWithStatusInfo;
    review: IReview;
    score: number;
}