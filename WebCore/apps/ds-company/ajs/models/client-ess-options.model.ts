

export interface ClientEssOptions {
    clientId: number;
    directDepositLimit: number | null;
    allowDirectDeposit: boolean;
    allowCheck: boolean;
    allowPaycard: boolean;
    allowPaystubEmails: boolean;
    allowImageUpload: boolean;
    welcomeMessage: string | null;
    finalDisclaimerMessage: string | null;
    finalDisclaimerAgreementText: string | null;
    isCustomMessage: boolean;
    manageDirectDeposit: boolean | null;
    manageDirectDepositAmountAndAccountInfo: boolean | null;
    directDepositDisclaimer: string | null;
    doInsert: boolean | null;
}