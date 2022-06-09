import { IRemark } from "@ds/core/shared";

export enum SystemFeedbackType{
    MenuWrapper = 1
}

export interface ISystemFeedbackData {
    systemFeedbackId: number;
    clientId : number;
    remarkId : number;
    systemFeedbackTypeId: SystemFeedbackType;
    remark: IRemark;
}