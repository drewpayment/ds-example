export interface IW2Client {
    clientName: string;
    clientCode: string;
    lastPay: boolean;
    quantity: number;
    billed: number;
    toBill: number;
    clientId: number;
    hasOneTimeW2sReady: boolean;
    hasOneTime1099: boolean;
    hasOneTimeDelivery: boolean;
    createReport: boolean;
    w2sReady: boolean;
    isScheduled: boolean;
    creationDate?: Date;
    deliveryDate?: Date;
    createManifest: boolean;
    create: boolean;
    annualNotes: string;
    miscNotes: string;
    date1099?: Date;
    approvedForClient: boolean;
    hasNotes: boolean;
    employeesLastUpdatedOn: string;
    isApprovable: boolean;
    uniqueId: string;
}
