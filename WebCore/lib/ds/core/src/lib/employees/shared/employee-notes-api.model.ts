export interface IEmployeeNotes {
    remarkId:       number,
    employeeId:     number,
    securityLevel:  number,
    description:    String,
    addedBy:        number,
    addedDate:      Date,
    noteSource:     String,
    noteSourceId:   number,
    showToggle:     boolean | false
    activity: 		    any;
    isArchived:     boolean | false;
    tags:           INoteTag[];
    attachments:    any[];
    files:              any[];
    employeeViewable: boolean;
    directSupervisorViewable: boolean;
    usersViewable:  string;
}

export interface INewEmployeeNote {
    remarkId:       number;
    employeeId:     number;
    securityLevel:  number;
    description:    String;
    noteSourceId:   number;
    reviewId?:      number;
    addedBy:        number;
}

export interface IEditEmployeeNote {
    remarkId?:       number;
    description:    String;
    noteSourceId:   number;
    reviewId?:      number;
    addedBy:        number;
    employeeId:     number;
}

export interface INoteSource {
    noteSourceId:   number;
    noteSource:     String;
}

export interface IEmpName {
    firstName:  string;
    lastName:   string;
}

export interface IAttachmentIds {
    resourceId: number; 
    modified: Date;
    modifiedBy: number;
}

export interface IEmpAttachmentChanges {
    attachmentId: number;
    remarkId: number;
    fileName: string;
    change: string;
}

export interface IAssignTags {
    tags: INoteTag[];
    remarkId: number;
}

export interface INoteTag {
    noteTagId: number;
    tagID: number;
    tagName: string;
    change: string;
}

export interface IShareSettings {
    remarkId:                   number;
    securityLevel:              number;
    directSupervisorViewable:   boolean;
    employeeViewable:           boolean;
    usersViewable:              string;
}

