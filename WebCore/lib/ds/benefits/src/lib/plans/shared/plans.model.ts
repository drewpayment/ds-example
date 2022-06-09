import { Moment } from 'moment';

export interface IPlanAdminSummary {
    planId: number;
    planName: string;
    planTypeId: string
    planTypeName: string;
    categroyName: string;
    startDate?: Date;
    endDate?: Date;
    numberEmployeesEnrolled: number;
    numberEmployeesSelected: number;
    planYearTitle: number;
}

export interface ICopyPlansDialogData {
    clientId: number;
}

export interface IPlan {
    planId: number;
    planName: string;
    selected: boolean;
}

export interface ICopyPlanConfig {
    startDate: Moment | string | null,
    endDate: Moment | string | null,
    planYear: number,
    plans: number[]
}
