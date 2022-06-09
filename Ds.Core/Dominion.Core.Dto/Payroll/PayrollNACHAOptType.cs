namespace Dominion.Core.Dto.Payroll
{
    public enum PayrollNACHAOptType
    {
        Default = 1,
        NoPrenotes = 2,
        StopBankCheckDebit = 4,
        UseTaxAccountForVendors = 5,
        StopBatchFileDebit = 6,
        CheckingAccountCredit = 7
    }
}
