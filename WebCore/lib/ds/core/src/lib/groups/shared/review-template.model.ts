import { Moment } from "moment";
import { IEvaluationTemplate, IEvaluationTemplateBase } from "../../../../../performance/src/lib/review-policy/shared/evaluation-template.model";
import { ReferenceDate } from '@ds/core/groups/shared/schedule-type.enum';
import { DateUnit } from '@ds/core/shared/time-unit.enum';
import { Maybe } from '@ds/core/shared/Maybe';
import { MonthType } from '@ds/core/shared/month-type.enum';
import { IContact } from '@ds/core/contacts';

export interface IReviewTemplateDetail {
    reviewPolicyId: number;
    reviewProcessStartDate: string | Date | Moment | null;
    reviewProcessEndDate: string | Date | Moment | null;
    evaluationPeriodFromDate: string | Date | Moment | null;
    evaluationPeriodToDate: string | Date | Moment | null;

    evaluations: IEvaluationTemplate[];
}

export interface IReviewTemplate {
    reviewTemplateId: number;
    reviewProfileId: number;
    name: string;
    reviewProcessStartDate?: string | Date | Moment;
    reviewProcessEndDate?: string | Date | Moment;
    evaluationPeriodFromDate?: string | Date | Moment;
    evaluationPeriodToDate?: string | Date | Moment;
    clientId: number;
    isArchived: boolean;
    modified?: Date;
    modifiedBy?: number;
    payrollRequestEffectiveDate?: string | Date | Moment;
    includeReviewMeeting: boolean;
    referenceDateTypeId: ReferenceDate;
    delayAfterReference?: number;
    delayAfterReferenceUnitTypeId?: DateUnit;
    reviewProcessDuration?: number;
    reviewProcessDurationUnitTypeId?: DateUnit;
    evaluationPeriodDuration?: number;
    evaluationPeriodDurationUnitTypeId?: DateUnit;
    groups: number[];
    hardCodedAnniversary?: string | Date | Moment;
    isRecurring: boolean;
    

    evaluations: IEvaluationTemplateBase[];
    reviewReminders?: ReviewReminder[];
    reviewOwners?: ReviewOwner[];
}

export interface ReviewReminder {
    reviewReminderID: number;
    reviewTemplateId: number;
    durationPrior: number;
    durationPriorUnitTypeId: DateUnit;

}

export interface ReviewOwner {
    reviewTemplateId: number;
    userId: number;
    contact: IContact;
}

export interface UsersNotified {
    usersNotifiedDuration?: number;
    usersNotifiedUnitTypeId?: DateUnit;
}

export interface ReviewTemplateDialogFormData {
    template: IReviewTemplate;
    usersNotified: UsersNotified;
}

export function GetReviewTemplateName(template: IReviewTemplate): string {
    return template.name;
}

export function SortByName<T>(getNameProperty: (item: T) => string, templates?:T[]): T[] {
    return new Maybe(templates).map(y => y.sort((a, b) => getNameProperty(a).toLowerCase().localeCompare(getNameProperty(b).toLowerCase()))).valueOr(<T[]>[]);
}