import { ScoreMethodType } from  "./score-method-type.model";
import { ScoreRangeLimit } from "./score-range-limit.model";

export interface ScoreModel {
    scoreModelId: number,
    clientId: number,
    hasScoreRange: boolean,
    isScoreEmployeeViewable: boolean;
    name: string;
    scoreMethodType: ScoreMethodType;
    scoreRangeLimits: ScoreRangeLimit[];
    isActive: boolean;
    additionalEarnings: boolean;
}
