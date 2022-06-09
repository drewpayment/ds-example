export interface IArPayment {
    arPaymentId: number;
    clientId: number;
    clientCode: string;
    clientName: string;
    genBillingHistoryId?: number;
    manualInvoiceId?: number;
    arDepositId?: number;
    invoiceNum: string;
    paymentDate: Date;
    amount: number;
    memo: string;
    isCredit: boolean;
    postedBy: number;
    isNsf: boolean;
    markedNsfBy?: boolean;
    markedNsfDate?: boolean;
}
