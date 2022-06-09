import { KeyValue } from "@models/key-value.model";

export interface IClientTax {
    clientTaxId: number,
    clientId: number,
    taxIdNumber: string,
    stateTaxId?: number,
    localTaxId?: number,
    description: string,
    unemploymentId: string, 
    calendarDebitId?: number,
    includeInElectronicInterface: boolean,
    disabilityTaxId?: number,
    alternateVendorName: string,
    LastSutaCatchupDate: Date,
    otherTaxId?: number,
    residentId?: number,
    legacyTaxType?: number,
    taxIsActive?: boolean,
    eftCreditId: string
}