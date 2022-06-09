import { GeneralLedgerGroupHeader } from './general-ledger-group-header.model';

export interface ClientGLAssignmentCustom {
    includeAccrual: Boolean;
    includeProject: Boolean;
    includeSequence: Boolean;
    includeClassGroups: Boolean;
    includeOffset: Boolean;
    includeDetail: Boolean;
    cashAssignmentHeaders: GeneralLedgerGroupHeader[],
    liabilityAssignmentHeaders: GeneralLedgerGroupHeader[],
    expenseAssignmentHeaders: GeneralLedgerGroupHeader[],
    paymentAssignmentHeaders: GeneralLedgerGroupHeader[],

    saveGroupId: number,
    cashItemCount: number,
    liabilityItemCount: number,
    expenseItemCount: number,
    paymentItemCount: number
}