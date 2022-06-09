import { IWeightedScoreItem } from "./weighted-score-item.model";

export interface IScoreGroup extends IWeightedScoreItem {
    evaluationGroupId: number;
    parentId: number;
    competencyGroupId: number;
    isGoals: boolean;
    isCompetencies: boolean;

    items: IWeightedScoreItem[] | IScoreGroup[];
}