export interface IFeedbackResponseItem {
    responseId: number;
    responseItemId: number;
    isVisibleToEmployee: boolean;
    value: any;

    responseText(): string;
    hasResponse(): boolean;
}

export interface ITypedFeedbackResponseItem<TValue> extends IFeedbackResponseItem {
    value: TValue;
}