export interface IUpdateEmployeeContact {
    employeeEmergencyContactId: number;
    employeeId: number;
    firstName: string;
    lastName : string;
    homePhoneNumber: string;
    workPhoneNumber: string;
    cellPhoneNumber: string;
    relation: string;
    emailAddress: string;
    requested: boolean;
}
