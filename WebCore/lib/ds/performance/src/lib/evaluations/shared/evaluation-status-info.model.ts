import { IEvaluationDetail } from "./evaluation-detail.model";

export interface IEvaluationWithStatusInfo extends IEvaluationDetail {
    goalsLength?: number;
    hasCompetencies?: boolean;
    hasGoals?: boolean;
    hasFeedback?: boolean;
    isReadOnly?: boolean;
    isEvalComplete?: boolean;
    isUserEvaluator?: boolean;
    hasSummaryData?: boolean;
}
