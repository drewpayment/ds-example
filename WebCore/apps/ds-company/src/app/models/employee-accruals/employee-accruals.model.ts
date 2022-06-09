import { FieldType } from '@ds/core/shared';

export interface IBenefitPackage {
    benefitPackageId?: number;
    name?: string;
    clientId: number,
}

export interface IClientEmploymentClass {
    clientEmploymentClassId: number;
    code: string;
    description: string;
    clientId: number,
    isEnabled: boolean,
}
export interface ICustomBenefitField{
    customFieldId: number;
    clientId: number;
    name: string;
    fieldKey: string;
    fieldType: FieldType;
    isEmployeeField: boolean;
    isDependentField: boolean;
    isArchived: boolean;
}

export interface ICustomBenefitFieldValue {
    customFieldValueId: number;
    customFieldId: number;
    name:string;
    clientId: number;
    employeeId: number;
    dependentId: number;
    textValue: string;
}

export interface IBenefitDependent
{
    dependentId: number;
    firstName: string;
    lastName: string;
    birthDate?: Date;
    customFields: Array<ICustomBenefitFieldValue>;
    isDirty?: boolean;
}

export interface IClientBenefitSetting
{
    clientId: number;
    showRetirementEligibilityOption: boolean;
}

export interface IEmployeeBenefitSettings {
    employeeId: number,
    clientId: number,
    isBenefitEligible: boolean,
    benefitEligibilityDate?: Date
    benefitPackageId?: number,
    isTobaccoUser: boolean,
    birthDate?: Date,
    gender  : string,
    defaultSalaryMethod?: number,
    clientEmploymentClassId?: number,
    isRetirementEligible: boolean,
    customValues: ICustomBenefitFieldValue[],
}

export interface IEmployeeAccrualInfo {
    employeeId: number,
    employeeAccrualId?: number,
    clientAccrualId: number,
    description: string,
    isActive: boolean,
    numDecimals: number,
    allowScheduledAwards: boolean,
    rate: number,
    balance: number,
    isVisible: boolean,
    showReferenceDate: boolean,
    referenceDate?: Date,
    referencePoint: string,
    hovered: boolean,
    isDirty?: boolean,
}

export interface IEmployeeAccrualList {
    employeeId?: number,
    clientAccrualId: number,
    beforeAfterDate: Date,
	description: string,
    allowScheduledAwards: boolean,
    isActive: boolean,
    display4Decimals: boolean,
    serviceReferenceDate?: Date,
    planType: ServicePlanTypeType,
    beforeAfterId: ServiceBeforeAfterType,
    serviceReferencePointId: ServiceReferencePointType,
    isDirty?: boolean,
    hovered?: boolean
}

export enum ServicePlanTypeType {
    NotSet           = 0,
    Auto             = 1,
    Manual           = 2,
    Custom           = 3,
    Payout           = 4,
    PayoutExtraCheck = 5
}

export enum ServiceBeforeAfterType
{
    NotSet = 0,
    BeforeReferenceDate = 1,
    StartingReferenceDate = 2
}

export enum ServiceReferencePointType
{
    HireDate = 1,
    AnniversaryDate = 2,
    RehireDate = 3,
    HireOnly = 4,
    AnniversaryHire = 5,
    CalanderYear = 6,
    Eligibility = 7,
    FirstOfMonthAnnivYear = 8,

    // Based on querying the ClientAccrual records in DB, I don't think this is actually used by any ClientAccruals.
    // Adding for now, because it was a case in spAutoApplyAccrualPolicy: `CA.ServiceReferencePointID = 22`.
    NoClueButItWasInSQLQuery = 22, // See: spAutoApplyAccrualPolicy
}