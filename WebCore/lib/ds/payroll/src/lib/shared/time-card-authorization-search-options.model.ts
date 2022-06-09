import { Moment } from 'moment';


export interface TimeCardAuthorizationSearchOptions {
    employeeId: number;
    approvalStatusType: ApprovalStatusType;
    daysFilterType: DaysFilterType;
    startDate: Date | Moment | string;
    endDate: Date | Moment | string;
    payPeriodDropdownSelectedValue: number;
    payPeriodDropdownSelectedItemText?: string;
    filter1Dropdown: TimeCardAuthorizationFilterDropdown;
    category1Dropdown: TimeCardAuthorizationFilterDropdown;
    filter2Dropdown: TimeCardAuthorizationFilterDropdown;
    category2Dropdown: TimeCardAuthorizationFilterDropdown;
    clientId: number;
    page: number | null;
    pageSize: number | null;
}

export interface TimeCardAuthorizationFilterDropdown {
    value: number;
    visible: boolean;
}

export enum ApprovalStatusType {
    All = 1,
    Approved,
    NotApproved    
}

export enum DaysFilterType {
    AllDays = 1,
    AllWeekdays,
    AllSchedulesDays,
    DatesWithActivity,
    DaysWithExceptions,
}
