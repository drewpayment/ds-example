import { IJobProfileSkill } from './job-profile-skill.model';
import { IJobProfileResponsibility } from './job-profile-responsibility.model';

export interface IJobProfileBasicInfo {
    jobProfileId: number,
    clientId: number,
    clientName: string,
    description: string,
    code: string,
    requirements: string,
    isActive: boolean,
    workingConditions: string,
    benefits: string,
    isBenefitPortalOn: boolean,
    eEOCLocation: string,
    clientDepartment: string,
    jobProfileResponsibilities: IJobProfileResponsibility[],
    jobProfileSkills: IJobProfileSkill[]
}
