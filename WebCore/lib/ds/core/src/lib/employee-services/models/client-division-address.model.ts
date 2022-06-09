import { IState } from '@ajs/applicantTracking/shared/models';
import { ClientContact } from '@ds/admin/client-statistics/shared/models/clientContact';
import { ClientDivision } from './client-divison.model';

export interface ClientDivisionAddress {
    clientDivisionAddressId: number;
    clientDivisionId:number;
    clientContactId:number;
    name:string;
    address1:string;
    address2:string;
    city:string;
    stateAbbreviation?:string;
    stateId:number;
    zip:string;
    countryId:number;
    
    clientDivision?: ClientDivision;
    clientContact?: ClientContact;
    state?: IState;
}
