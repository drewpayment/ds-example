import { EvaluationRoleType } from "@ds/performance/evaluations";
import { Moment } from "moment";
import { DateUnit } from '@ds/core/shared/time-unit.enum';

export interface IEvaluationTemplate {
    reviewProfileEvaluationId: number;
    role: EvaluationRoleType;
    startDate: string | Date | Moment | null;
    dueDate: string | Date | Moment | null;
}

export interface IEvaluationTemplateBase {
    reviewProfileEvaluationId: number;
    reviewTemplateId: number;
    role: EvaluationRoleType;
    startDate: string | Date | Moment | null;
    dueDate: string | Date | Moment | null;
    evaluationDuration?: number;
    evaluationDurationUnitTypeId?: DateUnit;
    conductedBy?:number;
}