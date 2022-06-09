import { ClientContact } from './clientContact';

export interface ClientPayroll {
    clientId: number;
    clientName: string;
    clientCode: string;
    addressLine1: string;
    addressLine2: string;
    city: string;
    stateId: number;
    postalCode: string;
    dataEntry: string;
    frequency: string;
    processDay: string;
    status: string;
    lastCheckDateRun: Date;
    lastPayrollAccepted: Date;
    contact: ClientContact;
}
