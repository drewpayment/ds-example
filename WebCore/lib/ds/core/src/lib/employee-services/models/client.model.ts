import { Moment } from 'moment';
import { ClientStatus } from '../enums'; 
import { State } from '.';

export interface Client {
    clientStatus:string;
    clientStatusCode:string;
    clientStatusId:ClientStatus;
    clientName:string;
    clientId:number;
    clientCode:string;
    isCurrentlyActive:boolean;
    terminationDate:Moment|Date|string;
    federalId:number;
    isTaxManagement:boolean;
    allowAccessUntilDate:Moment|Date|string;
    addressLine1:string;
    addressLine2:string;
    city:string;
    state:State;
    postalCode:string;
}