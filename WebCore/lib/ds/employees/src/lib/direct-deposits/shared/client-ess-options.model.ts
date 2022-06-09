export interface IClientEssOptions {
    clientId            : number,
    directDepositLimit  : number | null,
    allowDirectDeposit  : boolean,
    allowCheck          : boolean,
    allowPaycard        : boolean,
    allowPaystubEmails  : boolean,
    allowImageUpload    : boolean,
    welcomeMessage: string,
    manageDirectDepositAmount: boolean,
    manageDirectDepositAmountAndAccountInfo: boolean,
    directDepositDisclaimer: string,
    doInsert            : boolean
}
