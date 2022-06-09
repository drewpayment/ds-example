export interface ClientGLControlItem {
    clientGLControlItemId : number,
    clientGLControlId     : number,
    clientID              : number,
    description           : string,
    generalLedgerTypeId   : number,
    assignmentMethodId    : number,
    _isSplit              : Boolean,
    _showHeader           : Boolean,
    _headerName           : string,
    foreignKeyId          : number | null,
    sequenceId            : number
}