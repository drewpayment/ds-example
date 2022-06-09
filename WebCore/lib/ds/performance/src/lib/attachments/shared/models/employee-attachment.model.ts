export interface EmployeeAttachment {
    resourceId: number;
    clientID: number;
    employeeId: number;
    name: string;
    addedDate : Date;
    addedByUsername: string;
    folderId  : number;
    isViewableByEmployee : boolean;
    isAzure: boolean;
    sourceType: number;
    extension: string;
    source: string;
    cssClass: string;
    onboardingWorkflowTaskId: number;
    isCompanyAttachment: boolean;
}
