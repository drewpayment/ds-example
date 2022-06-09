export interface JobCostingInfo {
results1: table1[];
results2: table2[];
results3: table3[];
}

export interface table1 {
    clientID: number;
    description: string;
    jobCostingTypeID: number;
    sequence: number;
    modifiedBy: number;
    modified: Date;
    isEnabled: Boolean;
    hideOnScreen: Boolean;
    isPostBack: Boolean;
    width: number;
    level: number;
    isRequired: Boolean;
}

export interface table2 {
    iD?: number;
    jobCostID?: number;
    description?: string;
    foreignKeyID?: number;
}

export interface table3 {
    iD?: number;
    description?: string;
    jobCostingTypeID?: number;
}