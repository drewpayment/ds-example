import { IReviewStatusSearchOptions } from '../../shared/review-search-options.model';

export const PAYROLL_REQUEST_REPORT_KEY = 'payroll-request-report';

export interface PayrollRequestReportArgs {
    searchOptions: IReviewStatusSearchOptions,
    clientSideFilters: ClientSideFilters
}

export interface ClientSideFilters {
    showMerit: boolean;
    showOneTime: boolean;
    showTable: boolean;
    selectedApprovalStatus: number[];
}