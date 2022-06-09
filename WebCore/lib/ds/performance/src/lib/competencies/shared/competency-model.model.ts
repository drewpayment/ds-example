import { ICompetency } from './competency.model';
import { EmployeePerformanceConfiguration } from './employee-performance-configuration.model';
import { IJobProfileDisplayable } from './job-profile-displayable.model';

export interface ICompetencyModel extends ICompetencyModelBasic {
    description: string,
    empPerfConfigs: EmployeePerformanceConfiguration[],
    jobProfiles: IJobProfileDisplayable[]
}

export interface ICompetencyModelBasic {
    competencyModelId?: number,
    clientId?: number | null,
    name: string,
    competencies?: ICompetency[],
}