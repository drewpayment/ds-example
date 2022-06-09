export interface IArClientCheckPayment{
    invoiceAmount: number;
    invoiceDate: Date;
    invoiceId: number;
    isManualInvoice: boolean;
    invoiceNumber: string;
    clientId: number;
    clientCode: string;
    clientName: string;
    prevPaidAmount: number;
    paymentAmount: number;
    paymentType?: PaymentType;
    paymentDate: Date;
    memo: string;
    isCredit: boolean;
    isPaid: boolean;
}

export enum PaymentType {
    Full = 0,
    Partial = 1
  }
