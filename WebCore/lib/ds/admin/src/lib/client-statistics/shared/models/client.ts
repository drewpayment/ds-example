import { ClientFeatures } from './clientFeatures';
import { ClientContact } from './clientContact';

export interface Client{
    clientId: number;
    clientName: string;
    clientCode: string;
    isActive: boolean;
    addressLine1: string;
    addressLine2: string;
    city: string;
    stateId: number;
    postalCode: string;
    countryId: number;
    accountFeatures: ClientFeatures[];
    hasLeaveManagement: boolean;
    hasUnemploymentSetup: boolean;
    contact: ClientContact;
}
