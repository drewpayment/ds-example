export interface IArReportParameter {
    arReportId: number;
    fileFormat: string;
    clientId: number;
    invoiceId: number;
    arDepositId: number;
    startDate: Date;
    endDate: Date;
    billingItemCode: string;
    agingDate: Date;
    agingPeriod: number;
    reportType: number;
    lookBackDate: Date;
    gainsLossesToggle: number;
    orderBy: number;  
    payrollId: number;  
}