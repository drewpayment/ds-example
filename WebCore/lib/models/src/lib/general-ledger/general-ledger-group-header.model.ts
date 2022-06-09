import { ClientGLControlItem } from './client-gl-control-item.model';
import { ClientGLAssignment } from './client-gl-assignment.model';

export interface GeneralLedgerGroupHeader {
    generalLedgerGroupHeaderId : number,
    description                : string,
    sequenceId                 : number,
    generalLedgerGroupId       : number,
    clientGLControlItems       : ClientGLControlItem[],
    clientGLAssignments        : ClientGLAssignment[]
}