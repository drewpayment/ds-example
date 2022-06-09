import { FilingStatus } from './filing-status';
// WE NEED TO REMOVE THIS AND UPDATE ALL TO WITH DISPLAY ORDER
export interface IFilingStatus {
    filingStatus: FilingStatus;
    description: string;
}

export interface IFilingStatusWithDisplayOrder extends IFilingStatus {
    filingStatusId: FilingStatus;
    description: string;
    displayOrder: number;
}
