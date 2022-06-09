import { IReviewStatus } from "./review-status.model";
import { ReferenceDate } from '@ds/core/groups/shared/schedule-type.enum';
import { DateUnit } from '@ds/core/shared/time-unit.enum';
import { IReviewStatusWithApprovalStatus } from './review-status-with-approval-status.model';

export interface IReviewGroupStatus {
    reviewTemplateId : number;
    reviewTemplateName: string;
    reviewProcessStartDate?: string | Date | null;
    reviewProcessDueDate?: string | Date | null;
    payrollRequestEffectiveDate?: string | Date | null;
    supervisorEvalDueDate?: string | Date | null;
    referenceDateTypeId: ReferenceDate;
    delayAfterReference?: number;
    delayAfterReferenceUnitTypeId?: DateUnit;
    reviewProcessDuration?: number;
    reviewProcessDurationUnitTypeId?: DateUnit;
    reviews: IReviewStatusWithApprovalStatus[];
}

export interface IAssignedToEmployee {
    employeeId?: number;
    firstName: string;
    lastName: string;
    fullName: string;

}
