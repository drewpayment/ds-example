export interface DemographicData {
    employees: DemographicInfo[];
}

export interface DemographicInfo {
    firstName: string;
    lastName: string;
    status: string;
    payType: string;
    lengthOfService: string;
    lengthOfServiceFormatted: string;
    age: string;
    gender: string;
    genderFormatted: string;
    ethnicity: string;
}