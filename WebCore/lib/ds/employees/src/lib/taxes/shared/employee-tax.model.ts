export interface IEmployeeTax {
    employeeTaxId: number,
    employeeId: number,
    filingStatusId: number,
    filingStatusDescription: string,
    numberOfExemptions: number,
    additionalAmountTypeId: number,
    additionalAmount: number,
    additionalPercent: number,
    description: string,
    clientId: number,
    taxCredit: number,
    otherTaxableIncome: number,
    wageDeduction: number,
    hasMoreThanOneJob: boolean,
    using2020FederalW4Setup: boolean
}

export interface IFormSignatureDefinition {
    formDefinitionId: number,
    signatureDefinitionId: number
}


