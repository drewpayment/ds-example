import { IClockClientTimePolicy } from '@ajs/labor/models';
import { IEEOCLocationData } from '@ajs/job-profiles/shared/models';
import { IUserInfo } from '@ajs/user';
import { IProfileImage } from '@ds/core/contacts';
import { IPayFrequencyData } from '@ajs/employee/hiring/shared/models';

export interface ISortedGeofenceEmployees {
    employees: IGeofenceEmployeeInfo[];
    timePolicyId: number;
}

export interface IGeofenceEmployeeInfo {
    clientDepartmentId: number;
    clientId: number;
    employeeNumber: string;
    firstName: string;
    isEmployeeActive: boolean;
    jobTitle: string;
    lastName: string;
    department: IDepartment;
    clockEmployee: IClockEmployee;
    eeocLocationId:  number;
    directSupervisorId: number;
    directSupervisor: IUserInfo;
    eeocLocation: IEEOCLocationData;
    profileImage: IProfileImage;
    employeeId: number;
    payFrequency: IPayFrequencyData;
}

export interface IClockEmployee {
    clientId: number;
    clockClientTimePolicyId: number;
    geofenceEnabled: boolean;
    timePolicy: IClockClientTimePolicy;
}

export interface IDepartment {
    clientDepartmentId: number;
    clientDivision: number;
    clientId: number;
    isActive: boolean;
    name: string;
}
