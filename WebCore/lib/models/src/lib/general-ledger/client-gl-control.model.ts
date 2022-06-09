import { ClientGLControlItem } from './client-gl-control-item.model';
import { GeneralLedgerGroupHeader } from './general-ledger-group-header.model';

export interface ClientGLControl {
    clientGLControlId: number;
    clientId: number;
    includeAccrual: Boolean;
    includeProject: Boolean;
    includeSequence: Boolean;
    includeClassGroups: Boolean;
    includeOffset: Boolean;
    includeDetail: Boolean;
    saveGroupId: number;

    clientGLControlItems: ClientGLControlItem[];
    cashControlHeaders: GeneralLedgerGroupHeader[];
    expenseControlHeaders: GeneralLedgerGroupHeader[];
    liabilityControlHeaders: GeneralLedgerGroupHeader[];
    paymentControlHeaders: GeneralLedgerGroupHeader[];
}