import { IArPayment } from './ar-payment.model';

export interface ArDeposit {
    arDepositId: number;
    type: string;
    createdDate: Date;
    createdBy: number;
    createdByUsername: string;
    total: number;
    postedDate?: Date;
    postedBy?: number;
    postedByUsername: string;
    arPayments: IArPayment[];
}
