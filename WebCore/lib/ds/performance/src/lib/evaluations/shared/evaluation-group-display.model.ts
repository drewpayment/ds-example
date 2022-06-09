export interface EvaluationGroupDisplay {
    weightedValue: {
        weight: number;
        value: number;
    };
    name: string;
    childrenDisplayDtos: EvaluationGroupDisplay[];
}