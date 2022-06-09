import { EvaluationStatusType } from "./evaluation-status-type.enum";

export interface IEvaluationStatus {
    evaluationId: number;
    status: EvaluationStatusType
}