export enum ApplicantCorrespondenceTypeEnum {
    applicationResponse = 1,
    applicationCompleted = 2,
    applicationDisclaimer = 3,
    onboardingInvitation = 4,
}

export interface IApplicantApplicationEmailHistoryData {
    applicantApplicationEmailHistoryId : number;
    applicationHeaderId : number;
    applicantCompanyCorrespondenceId? : number;
    correspondenceTemplateName: string;
    correspondenceSubject : string;
    applicantStatusTypeId? : number;
    senderName : string;
    sentDate : Date;
    isText?: boolean;
    correspondenceTypetext: string; 
}

export interface IApplicantEmailTemplateData {
    applicantCompanyCorrespondenceId?: number;
    isText?: boolean;
    clientId?: number;
    description?: string;
    subject?: string;
    applicantCorrespondenceTypeId?: number;
    applicantCorrespondenceType?: string;
    body?: string;
    isActive?: boolean;
    modified?: Date;
    modifiedBy?: string;
    isApplicantAdmin?: boolean;
}

export interface IApplicantCorrespondenceTypeData {
    applicantCorrespondenceTypeId?: number;
    description?: string;
}

export interface IApplicantDetailData {
    applicantId?: number;
    applicantName?: string;
    applicantNameFlipped?: string;
    username: string;
    origPostingId?: number;
    employeeId?: number;
    postingId?: number;
    postingNo?: number;
    posting?: string;
    postingStartDate?: Date;
    applicationSubmittedOn?: Date;
    isFlagged?: boolean;
    isApplicantDenied?: boolean;
    isApplicationDenied?: boolean;
    isTextEnabled?: boolean;
    applicationHeaderId?: number;
    isRecommended?: boolean;
    applicantResumeId?: number;
    filledOn?: Date;
    applicantResumeRequiredId?: number;
    note: string;
    applicantOnBoardingProcessId?: number;
    applicantRejectionReasonId?: number;
    rejectionReason: string;
    hasViewed?: boolean;
    applicantStatusTypeId?: number;
    noteCount?: number;
    resumeLinkLocation: string;
    isExternalApplicant: boolean;
    externalSite: string;
    coverLetter: string;
    coverLetterId?: number;
    applicantCorrespondenceTypeId?: number;
    sentEmailsCount: number;
    jobSiteName: string;
    addedByAdmin: boolean;
    addedByUserName: string;
    score?: number;
    disclaimerId?: number;
    hireInfo: any;
    documentCount?: number;
}

export interface IEmailBodyData {
    body: string;
}

export interface ICustomizeSenderData {
    clientId: number,
    clientSMTPSettingId: number,
    senderEmail: string,
    smtpHost: string,
    smtpPort: string,
    smtpLogin: string,
    smtpPassword: string,
    secureConnection: string    
}

export interface IEmailData {
    toAddress: string,
    subject: string,
    body: string,
    isHtml: boolean
}