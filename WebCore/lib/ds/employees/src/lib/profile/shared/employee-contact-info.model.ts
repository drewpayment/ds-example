export interface IEmployeeContactInfo {
    employeeId: number,
    employeeNumber: string,
    firstName: string,
    middleInitial: string,
    lastName: string
    addressLine1: string,
    addressLine2: string,
    city: string,
    postalCode: string,
    countryId: number,
    countryName: string,
    stateId: number,
    stateName: string,
    countyId: number,
    countyName: string,
    homePhoneNumber: string,
    cellPhoneNumber: string,
    relation: string,
    emailAddress: string,
    gender: string,
    birthDate: Date,
    jobProfileId: number,
    jobTitleInfoDescription: string,
    divisionId: number,
    divisionName: string,
    departmentId: number,
    departmentName: string,
    driversLicenseExpirationDate: Date,
    driversLicenseNumber: string,
    driversLicenseIssuingStateId: number,
    driversLicenseIssuingStateName: string,
    noDriversLicense: boolean

}
