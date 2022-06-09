import { IEmployeeImage } from '@ajs/core/ds-resource/models';

export interface IEEOCEmployeeInfo {
    employeeId: number;
    clientId: number;
    clientCode: string;
    name: string;
    number: string;
    gender: string;
    race?: EEOCRace;
    locationId?: number;
    jobCategory?: EEOCJobCategory;
    isMissingEeocInfo: boolean;
    profileImage:        IEmployeeImage;
}

export enum EEOCRace {
    UNDISCLOSED = 0,
    HISPANIC_LATINO = 1,
    WHITE = 2,
    AFRICAN_AMERICAN = 3,
    HAWAIIAN_OTHER_ISLANDER = 4,
    ASIAN = 5,
    AMERICAN_INDIAN = 6,
    TWO_OR_MORE = 7
}

export enum EEOCJobCategory {
    NONE = 0,
    EXECS = 1,
    MANAGERS = 2,
    PROFESSIONALS = 3,
    TECHNICIANS = 4,
    SALES_WORKERS = 5,
    ADMIN_SUPPORT = 6,
    CRAFT_WORKERS = 7,
    OPERATIVES = 8,
    LABORERS = 9,
    SERVICE_WORKERS = 10,
    NOT_APPLICABLE = 99
}

