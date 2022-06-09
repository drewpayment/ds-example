import { FieldType } from "@ds/core/shared";
import { IFeedbackItem } from "./feedback-item.model";

export interface IFeedback {
    feedbackId: number;
    body: string;
    fieldType: FieldType;
    isRequired: boolean;
    isEnabled: boolean;
    feedbackItems?: IFeedbackItem[];
    reviewProfileEvaluations?: number[];
    feedbackResponses: number[];
    isArchived: boolean;
}
