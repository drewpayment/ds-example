import { ReviewStatusType } from "./review-status-type.enum";
import { IReview } from "@ds/performance/reviews";
import { IEmployeeSearchResult } from "@ajs/employee/search/shared/models";
import { IEvaluationStatus } from "./evaluation-status.model";
import { IScoreGroup } from '@ds/performance/evaluations/shared/score-group.model';

export interface IReviewStatus {
    status: ReviewStatusType;
    review: IReview;
    employee: IEmployeeSearchResult;
    isSetupIncomplete: boolean;
    score: IScoreGroup;
    evaluationStatuses: IEvaluationStatus[];
}