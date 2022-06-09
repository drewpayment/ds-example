import { IFeedback } from "./feedback.model";
import { checkboxComponent } from "@ajs/applicantTracking/application/inputComponents";

export interface IFeedbackSetup extends IFeedback {
    clientId: number;
    isSupervisor: boolean;
    isPeer: boolean;
    isSelf: boolean;
    isActionPlan: boolean;
    isVisibleToEmployee:boolean;
}