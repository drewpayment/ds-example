import { DataRow } from './DataRow.model';

export interface GetTimeApprovalTableDto {
    table: DataRow[];
    totalPages: number;
    currentPage: number;
}
