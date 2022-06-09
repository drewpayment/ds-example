import { Client, ClientDepartment } from '.';

export interface ClientDivision {
    clientDivisionId:number;
    clientId:number;
    clientContactId:number|null;
    name:string;
    addressLine1:string;
    addressLine2:string;
    city:string;
    stateAbbreviation:string;
    stateId:number;
    zip:string;
    countryId:number;
    sendCorrespondenceTo:number;
    isUseSeparateAccountRoutingNumber:boolean|null;
    accountNumber:string;
    bankId:number|null;
    isActive:boolean;
    isUseAsStubAddress:boolean;
    locationId?:string;
    dateLocationOpened?:Date;
    dateLocationClosed?:Date;
    
    client?:Client;
    departments?:ClientDepartment[];
}
