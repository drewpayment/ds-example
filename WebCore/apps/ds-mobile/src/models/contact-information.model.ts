import { Moment } from 'moment';

export interface ContactInformation {
    employeeId: number;
    employeeNumber: number;
    firstName: string;
    middleInitial: string;
    lastName: string;
    jobTitleInfoDescription: string;
    addressLine1: string;
    addressLine2: string;
    city: string;
    stateId: number;
    stateName: string;
    countryId: number;
    countryName: string;
    postalCode: string;
    countyId?: number;
    gender: string;
    birthDate: Date | Moment | string;
    jobProfileId: number;
    divisionId: number;
    divisionName: string;
    departmentId: number;
    departmentName: string;
    homePhoneNumber: string;
    cellPhoneNumber: string;
    emailAddress: string;
    driversLicenseExpirationDate: Date | Moment | string;
    driversLicenseNumber: string;
    driversLicenseIssuingStateId: number;
    driversLicenseIssuingStateName: string;
    noDriversLicense: boolean;
    clientClientName: string;
}
