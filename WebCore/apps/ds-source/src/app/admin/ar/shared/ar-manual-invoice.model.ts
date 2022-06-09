export interface IArManualInvoice {
    arManualInvoiceId: number;
    clientId: number;
    invoiceNum: string;
    invoiceDate: Date;
    totalAmount: number;
    postedBy: number;
    isPaid: boolean;
    dominionVendorOpt: number;
    invoiceNumAndDate: string
    manualInvoiceDetails: IArManualInvoiceDetail[];
}

export interface IArManualInvoiceDetail {
    arManualInvoiceDetailId: number;
    arManualInvoiceId: number;
    amount: number;
    itemCode: string;
    itemDescription: string;
    billingYear;
    action: string;
    isEdit: boolean;
}
