import { ClientStatusType } from '@ajs/client/client-status-type';
import { UserType } from '@ds/core/shared';

export const NPS_DASHBOARD_FILTERS_KEY = "qualify-dashboard-filters";

export interface NpsResponse {
    responseId: number;
    questionId: number;
    userId: number;
    userTypeId: UserType;
    score: number;
    responseDate: Date;
    clientId: number;
    clientName: string;
    clientCode: string;
    employeeFirstName: string;
    employeeLastName: string;
    feedback: string;
    isResolved?: boolean;
    resolvedByUserId?: number;
    resolvedByUserName?: string;
    resolvedDate?: Date;
}

export interface NpsScore {
    userTypeId: number;
    title: string;
    isApplicable: boolean;
    score: number;
    numerator: number;
    denominator: number;
    percentage: number;
    colorCode: string;
}


export interface NpsResponseFilter {
    fromDate?: Date;
    toDate?: Date;
    clientOrganizationId?: number;
    clientId?: number;
    clientStatus?: ClientStatusType;
    isResolved?: boolean;
    userTypes?: Array<UserType>;
    responseTypes?: Array<NpsResponseType>;
    hideResponsesWithoutFeedback?: boolean;
    searchFeedback?: string;
}

export enum NpsFilterType {
    clientOrganizationId,
    clientId,
    clientStatus,
    isResolved,
    userType,
    responseType,
    hideResponsesWithoutFeedback,
    searchFeedback
}

export interface IEnabledNpsFilter {
    id: string,
    type: NpsFilterType,
    value: string
}

export enum NpsResponseType
{
    Detractor = 1, 
    Passive = 2, 
    Promoter = 3
}