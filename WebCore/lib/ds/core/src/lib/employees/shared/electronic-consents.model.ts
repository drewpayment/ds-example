export interface IElectronicConsents {
    clientId: number;

    // W2 Properties
    employeeW2ConsentId: number;
    employeeId: number;
    employeeName: string;
    consentDateW2: Date;
    withdrawalDateW2: Date;
    taxYear : number;
    primaryEmailAddress: string;
    secondaryEmailAddress: string;

    modified: Date;
    modifiedBy: number;
    isEmailVerified: boolean;

    // 1095 Properties
    has1095C: boolean;
    consentDate1095C: Date;
    withdrawalDate1095C: Date;

    isW2Insert: boolean;
    is1095CInsert: boolean;
    isAcaEnabled: boolean;

    hasOnlyW2: boolean;
    hasOnly1095C: boolean;
}
