export interface BankDepositData {
    deposits: BankDeposit[];
}

export interface BankDeposit {
    name: string;
    employeePayType: string;
    netPay: number;
    taxName: string;
    amount: number;
    ssDefermentTax: number;
    ffcraCredit: number;
    mtdTotal: number;
    qtdTotal: number;
    ytdTotal: number;
    isVendor: boolean;
}