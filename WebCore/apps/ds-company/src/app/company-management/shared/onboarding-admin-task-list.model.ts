export interface IOnboardingAdminTaskList {
    onboardingAdminTaskListId: number;
    name: string;
    clientId: number;
    onboardingAdminTasks: Array<IOnboardingAdminTask>;
    jobProfiles: Array<IJobProfileDetail>;
    isDirty: boolean;
    hovered: boolean;
}

export interface IOnboardingAdminTask{
    onboardingAdminTaskId: number;
    onboardingAdminTaskListId: number;
    description: string;
    isEditing: boolean;
}

export interface IJobProfileDetail {
    jobProfileId: number;
    clientId: number;
    description: string;
    isActive: boolean;
    directSupervisorId?: number;
    onboardingAdminTaskListId?: number;
}

export enum Modification {
    none     = 0,
    onlyName = 1,
    onlyTask = 2,
    all      = 3,
}