export interface UserTermsAndConditionsDto  
{
userTermsAndConditionsID: number;
userId: number;
acceptDate?: Date | string;
userAccepted: boolean;
termsAndConditionsVersionId?: number; 
}