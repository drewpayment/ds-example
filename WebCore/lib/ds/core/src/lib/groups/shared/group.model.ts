import { DateUnit } from '@ds/core/shared/time-unit.enum';
import { LengthOfServiceBoundType } from './length-of-service-bound-type.model';

export interface Group {
    groupId?: number;
    clientId?: number;
    name?: string;
    jsonData?: GroupJsonDto;
    competencyModels: number[];
    reviewTemplates?: number[];
}

export interface GroupJsonDto {
    statuses?: number[];
    departments?: number[];
    jobTitles?: number[];
    costCenters?: number[];
    payTypes?: number[];
    dateUnit?: DateUnit;
    duration?: number;
    boundType?: LengthOfServiceBoundType;
}