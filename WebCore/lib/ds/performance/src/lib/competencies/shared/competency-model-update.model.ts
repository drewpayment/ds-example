import { ICompetency } from './competency.model';
import { EmployeePerformanceConfiguration } from './employee-performance-configuration.model';
import { IJobProfileDisplayable } from './job-profile-displayable.model';

export interface ICompetencyModelUpdate {
    competencyModelId: number,
    clientId: number | null,
    name?: string,
    addedCompetencies?: ICompetency[],
    removedCompetencies?: ICompetency[],
    addedEmpPerfConfigs?: EmployeePerformanceConfiguration[],
    removedEmpPerfConfigs?: EmployeePerformanceConfiguration[],
    addedJobProfiles?: IJobProfileDisplayable[];
    removedJobProfiles?: IJobProfileDisplayable[];
}