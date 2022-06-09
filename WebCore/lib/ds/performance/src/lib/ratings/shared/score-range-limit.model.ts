export interface ScoreRangeLimit {
    description?: string;
    label?: string;
    maxScore?: number;
    scoreModelId: number;
    scoreModelRangeId: number;
    meritPercent?: number;
}