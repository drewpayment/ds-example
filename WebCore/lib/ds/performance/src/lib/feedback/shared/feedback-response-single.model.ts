import { IFeedbackResponse } from "./feedback-response.model";
import { IContact } from "@ds/core/contacts";
import { FieldType, ViewRemark } from "@ds/core/shared";
import { ITypedFeedbackResponseItem } from "./feedback-response-item.model";

export interface ISingleFeedbackResponse<TValue> extends IFeedbackResponse, ITypedFeedbackResponseItem<TValue> {
    isActive: boolean;
}

export class BaseSingleFeedbackResponse<TValue> implements ISingleFeedbackResponse<TValue> {
    responseItemId: number;
    feedbackId: number;
    responseId: number;
    feedbackBody: string;
    responseByContact: IContact;
    fieldType: FieldType;
    isRequired: boolean;
    value: TValue;
    isVisibleToEmployee: boolean;
    isActive: boolean;
    activityFeed?: ViewRemark[];
    isLoading: boolean;
    oldVal: any;

    responseText() {
        return this.hasResponse() ?  this.value.toString() : "No response given";
    }

    hasResponse() {
        return this.value !== null;
    }
}
