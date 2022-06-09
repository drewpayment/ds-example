import { ICompetencyModel } from '@ds/performance/competencies/shared/competency-model.model';
import { ClientDivisionDto } from '@ajs/ds-external-api/models/client-division-dto.model';
import { JobProfileDto } from '@ajs/ds-external-api/models/job-profile-dto.model';
import { IEmployeePayType } from '@ds/reports/shared/employee-pay-type.model';
import { ClientDepartmentDto } from '@ajs/ds-external-api/models/client-department-dto.model';
import { Group } from '../shared/group.model';
import { IReviewTemplate } from '../shared/review-template.model';
import { IClientCostCenterDto } from '@ajs/labor/punch/api';

export interface IGroupDialogData {
    compModels: ICompetencyModel[];
    divisions: ClientDivisionDto[];
    departments: ClientDepartmentDto[];
    jobProfiles: JobProfileDto[];
    costCenters: IClientCostCenterDto[];
    payTypes: IEmployeePayType[];
    templates: IReviewTemplate[];
    group: Group
}