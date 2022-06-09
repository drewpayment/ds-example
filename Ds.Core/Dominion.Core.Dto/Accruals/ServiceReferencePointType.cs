namespace Dominion.Core.Dto.Accruals
{
    public enum ServiceReferencePointType
    {
        HireDate = 1,
        AnniversaryDate = 2,
        RehireDate = 3,
        HireOnly = 4,
        AnniversaryHire = 5,
        CalanderYear = 6,
        Eligibility = 7,
        FirstOfMonthAnnivYear = 8,

        // Based on querying the ClientAccrual records in DB, I don't think this is actually used by any ClientAccruals.
        // Adding for now, because it was a case in spAutoApplyAccrualPolicy: `CA.ServiceReferencePointID = 22`.
        NoClueButItWasInSQLQuery = 22, // See: spAutoApplyAccrualPolicy
    }
}
