import { IUserUsername } from './user-username.model';

export interface ICheckStockOrder {
    checkStockOrderId? : number,
    clientId?          : number,
    requestedDate?     : Date,
    nextCheckNumber    : number,
    isDelivery         : Boolean,
    totalChecks        : number,
    checkEnvelopes     : number,
    w2Envelopes        : number,
    acaEnvelopes       : number,
    orderPrice?        : number,
    datePrinted?       : Date | null,
    requestedByUserId? : number | null,
    requestedByUser?   : IUserUsername,
    printedByUser?     : IUserUsername
}