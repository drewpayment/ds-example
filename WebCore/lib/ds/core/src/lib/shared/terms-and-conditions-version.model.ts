export interface ITermsAndConditionsVersion {
    termsAndConditionsVersionID: number;
    effectiveDate: Date;
    lastEffectiveDate?: Date;
    fileName: string;
    fileLocation: string;
}