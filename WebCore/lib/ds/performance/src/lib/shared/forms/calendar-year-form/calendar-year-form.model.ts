import { Type } from '@angular/core';

export interface ICalendarYearForm {
    evaluationPeriodFromDate: any;
    evaluationPeriodToDate: any;
    reviewProcessStartDate: any;
    reviewProcessDueDate: any;
    supervisorEvaluationDueDate: any;   
    supervisorEvaluationStartDate: any;
    supervisorEvaluationId: any;
    supervisorRCRReviewProfileEvalId: any;
    employeeEvaluationDueDate: any;
    employeeEvaluationStartDate: any;
    employeeEvaluationId: any;
    employeeRCRReviewProfileEvalId: any;
    payrollRequestDate: any;
    hasReviewMeeting: any;
    hasSupervisorEval: any;
    hasEmployeeEval: any;
    includePayrollRequests:any;
    empEvalCompleteDate: any;
    supEvalCompleteDate: any;
    supEvalSignatures: any;
    supEvalSignatureId: any;
    empEvalSignatureId: any;
    currentAssignedSupervisor: any;
}

export interface IFormItem<T> {
    data: T;
    component: Type<IFormItemComponent<T>>;
}

export abstract class IFormItemComponent<T> {
    data: T;
}