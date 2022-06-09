export interface IReviewProfile {
    reviewProfileId: number;
    clientId: number;
    name: string;
    defaultInstructions: string;
    includeReviewMeeting: boolean;
    includeScoring: boolean;
    includePayrollRequests: boolean;
    isArchived: boolean;
}