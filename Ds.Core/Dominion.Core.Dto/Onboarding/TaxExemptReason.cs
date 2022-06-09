namespace Dominion.Core.Dto.Onboarding
{
    public enum TaxExemptReason : byte
    {
        TaxLiabilityNotExpected        = 1,
        WagesAreExempt                 = 2,
        PermanentHomeInRenaissanceZone = 3,
        IMeetTheConditionsSetForthUnderTheServicememberCivilReliefAct = 4,
        IncomIsEarnedAsAnActiveDutyMemberOfTheArmedForcesOfTheUnitedStates = 5,
        QualifyForFortCampbellExemptionCertificate = 6, //KY State W4
        QualifyForNonresidentMilitarySpouseExemption = 7, //KY State W4
        WorkInThisStateButResideInAReciprocalState = 8, //KY State W4
        ResidentMilitaryServicememberStationedOutsideOfStateOnActiveDutyMilitaryOrders = 9, //Ohio W4
        NonResidentMilitaryServicememberStationedInThisStateDueToMilitaryOrders = 10, //Ohio W4
        FullTimeStudent = 11 //NY-E
    }
}