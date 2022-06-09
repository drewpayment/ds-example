export interface IArDominionCheckPayment{
    clientId: number;
    clientCode: string;
    invoiceId: number;
    invoiceNumber: string;
    invoiceDate: Date;
    checkNumber: number;
    checkDate: Date;
    totalAmount: number;
    isPaid: boolean;
    isManualInvoice: boolean;
    arPaymentId: number;
}
