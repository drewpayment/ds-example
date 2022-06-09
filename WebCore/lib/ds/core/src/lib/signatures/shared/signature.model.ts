import { Moment } from "moment";

export interface ISignature {
    signatureId : number; 
    signatureName? : string;
    signatureDate : Date | string | Moment; 
    signeeFirstName : string;
    signeeLastName : string;
    signeeMiddle? : string;   
    signeeInitials? : string;
    signeeTitle? : string;    
    modifiedBy: number;
}