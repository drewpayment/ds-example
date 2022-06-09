export interface IEmployeeChangeInfo {
    dominionColumnsId      : number;
    tFriendlyDesc          : string;
    cFriendlyDesc          : string;
    friendlyDesc           : string;
    changeLogIds           : string;
    selected               : Boolean;
}

export interface IEmployeeChangeList {
    tFriendlyDesc       : string;
    isChecked           : Boolean;
    changedItem         : IEmployeeChangeInfo[];
    smColWidth          : Boolean;
    filtered?           : IEmployeeChangeInfo[];
}