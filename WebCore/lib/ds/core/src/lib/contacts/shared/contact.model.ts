import { IProfileImage } from './profile-image.model';
import { IClientDivisionData, IClientDepartmentData } from '@ajs/applicantTracking/shared/models';
import { UserInfo } from '@ds/core/shared';
import { Moment } from 'moment';
import { ICompetencyModel } from '@ds/performance/competencies';

export interface IContact {
    userId?: number | null;
    employeeId?: number | null;
    firstName: string;
    lastName: string;
    birthDate?: Date;
    employeeNumber?: string;
    profileImage?: IProfileImage;
    userType?: number;
}

export interface IContactWithClient extends IContact {
    clientId: number;
}

export interface IReviewContact extends IContact {
    division?: IClientDivisionData;
    department?: IClientDepartmentData;
    jobTitle?: string;
    supervisor?: UserInfo;
    hireDate?: Date|string|Moment;
    payType?: string;
    competencyModel?: ICompetencyModel;
}

export interface IClientContact extends IContact {
    clientContactId: number;
    clientId: number;
    firstName: string;
    lastName: string;
    title?: string;
    email?: string;
    phone?: string;
    phoneExtension?: string;
    mobilePhone?: string;
    isPrimary?: boolean;
    isDelivery?: boolean;
    isActive?: boolean;
}
