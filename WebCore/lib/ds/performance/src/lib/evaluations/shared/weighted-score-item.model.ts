import { IRemark } from '@ds/core/shared';

export interface IWeightedScoreItem {
    name: string;
    score: number;
    weight: number | null;
    weightPercent: number | null;
    weightedScore: number | null;
    /**
     * Only has a value if explicitly told to be included
     * @see PerformanceReviewsService.calculateEvalScore
     */
    evalInfo: CommentsAndIds | null;
}

export interface CommentsAndIds {
    comments: IRemark;
    /**
     * The composite key of the goal/competency evaluation
     */
    id: string;
    description: string;
    groupName: string;
}
