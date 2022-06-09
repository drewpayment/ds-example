import { EvaluationRoleType } from "@ds/performance/evaluations";
import { IReviewProfileEvaluationFeedback } from "./review-profile-evaluation-feedback.model";

export interface IReviewProfileEvaluation {
    reviewProfileEvaluationId: number;
    role: EvaluationRoleType;
    instructions: string;
    includeCompetencies: boolean;
    includeGoals: boolean;
    includeFeedback: boolean;
    isSignatureRequired: boolean;
    isDisclaimerRequired: boolean;
    isGoalsWeighted: boolean;
    disclaimerText: string;
    isActive: boolean;
    competencyOptionId?: number;
    goalOptionId?: number;
    isCompetencyCommentRequired?: boolean;
    isGoalCommentRequired?: boolean;
    feedback: IReviewProfileEvaluationFeedback[];
    selectedCompetenceyRatings: number[];
    selectedGoalRatings: number[];
    isApprovalProcess: boolean;
}

