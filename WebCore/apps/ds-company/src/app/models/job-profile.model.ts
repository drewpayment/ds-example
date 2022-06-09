import { Moment } from 'moment';

export interface IClientDepartmentData {
    clientDepartmentId?: number;
    name?: number;
}

export interface IJobProfileAccrualsData {
    jobProfileId?: number;
    clientAccrualId?: number;
    description?: string;
}

export interface IJobDetailData {

    jobProfileId?: number;
    description?: string;
    isActive?: boolean;
    clientId?: number;
    code?: string;
    requirements?: string;
    workingConditions?: string;
    benefits?: string;
    isBenefitPortalOn?: boolean;
    isApplicantTrackingOn?: boolean;
    sourceURL?: string;
    classifications?: IJobProfileClassificationsData;
    compensation?: IJobProfileCompensationData;
    jobProfileAccruals?: Array<IJobProfileAccrualsData>;
    competencyModelId?: number;
    jobProfileOnboardingWorkflows?: Array<IJobProfileOnboardingWorkflowData>;
    isOnboardingEnabled?: boolean;
    isPerformanceReviewsEnabled?: boolean;
    onboardingAdminTaskListId?: number;
}

export interface IJobProfileBasicInfoData {

    jobProfileId?: number;
    description?: string;
    isActive?: boolean;
    clientId?: number;
    clientName?: string;
    code?: string;
    requirements?: string;
    workingConditions?: string;
    benefits?: string;
    isBenefitPortalOn?: boolean;
    jobProfileResponsibilities: Array<IJobResponsibilitiesData>;
    jobProfileSkills: Array<IJobSkillsData>;
}

export interface IJobSkillsData {
    jobSkillId?: number;
    clientId?: number;
    description?: string;
    isSelect?: boolean;
    isSkillEditing?: boolean;
    editDescription?: string;
}

export interface IJobResponsibilitiesData {
    jobResponsibilityId?: number;
    clientId?: number;
    description?: string;
    isSelect?: boolean;
    isResponsibilityEditing?: boolean;
    editDescription?: string;
}

export interface IJobProfileClassificationsData {
    jobProfileId?: number;
    clientDivisionId?: number;
    clientDepartmentId?: number;
    clientGroupId?: number;
    employeeStatusId?: number;
    applicantPostingCategoryId?: number;
    jobClass?: string;
    eeocLocationId?: number;
    eeocJobCategoryId?: number;
    clientWorkersCompId?: number;
    clientCostCenterId?: number;
    clientShiftId?: number;
    directSupervisorId?: number;

    departments: Array<IClientDepartmentData>;
    jobResponsibilities: Array<IJobResponsibilitiesData>;
    jobSkills: Array<IJobSkillsData>;
}

export interface IJobProfileCompensationData {
    employeeTypeID?: number;
    isExempt?: boolean;
    isTipped?: boolean;
    payFrequencyID?: number;
    isBenefitsEligibility?: boolean;
    benefitPackageId?: number;
    salaryMethodTypeId?: number;
    hours?: number;
}

export interface IJobProfileOnboardingWorkflowData{
    jobProfileId?: number;
    onboardingWorkflowTaskId?: number;
    formTypeId?: number;
}

export interface IClientDivisionData {
    clientDivisionId?: number;
    name?: string;
}

export interface IClientDepartmentData {
    clientDepartmentId?: number;
    name?: number;
}

export interface IClientGroupData {
    clientGroupId?: number;
    description?: string;
}

export interface IDirectSupervisorData {
    userId?: number;
    firstName?: string;
}

export interface IPayFrequencyListData {
    payFrequencyId?: number;
    name?: string;
}

export interface IClientShiftData {
    clientShiftId?: number;
    description?: string;
}

export interface IEmployeeStatusData {
    employeeStatusId?: number;
    description?: string;
}

export interface ICoreClientCostCenterData {
    clientCostCenterId?: number;
    description?: string;
}

export interface IAddressData {
    addressId?: number;
    addressLine1?: string;
    addressLine2?: string;
    city?: string;
    stateId?: number;
    countryId?: number;
    countyId?: number;
    ZipCode?: string;
}
export interface IEEOCLocationData {
    eeocLocationId?: number;
    eeocLocationDescription?: string;
    clientId: number;
    isActive: boolean;
    isHeadquarters: boolean;
    unitAddressId?: number;
    unitNumber: string;
    unitAddress: IAddressData;
}

export interface IEEOCJobCategoryData {
    jobCategoryId?: number;
    description?: string;
}

export interface IBenefitPackagesData {
    benefitPackageId?: number;
    name?: string;
}

export interface IClientWorkersCompData {
    clientWorkersCompId?: number;
    description?: string;
}

export interface IJobProfileCompensationData {
    employeeTypeID?: number;
    isExempt?: boolean;
    isTipped?: boolean;
    payFrequencyID?: number;
    isBenefitsEligibility?: boolean;
    BenefitPackageId?: number;
    SalaryMethodTypeId?: number;
    Hours?: number;
}

export interface ISalaryDeterminationMethodData {
    salaryMethodTypeId?: number;
    description?: string;
}

export interface IOnboardingAdminTaskListData {
    onboardingAdminTaskListId?: number;
    name?: string;
}

export interface IJobProfileDefaultData {
    id?: number;
    label?: string;
    description?: string;
    jobProfileId?: number;
    itemId?: number;
}