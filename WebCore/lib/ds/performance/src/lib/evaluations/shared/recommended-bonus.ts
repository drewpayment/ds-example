import { IncreaseType } from '@ds/performance/competencies/shared/increase-type';

export interface RecommendedBonus {
    complete: number;
    total: number;
    targetIncreaseAmount?: number;
    targetIncreaseType?:IncreaseType;
}