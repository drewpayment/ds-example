export interface IBankDepositInfo {
    isCurrent          : boolean;
    isPrevious         : Boolean;
    taxPaymentTotal    : number;
    eePaymentTotal     : number;
    vendorPaymentTotal : number;
    bankDepositTotal   : number;
    showPrevious       : Boolean;
    percentDifference  : number;
    materialIcon       : string;
}