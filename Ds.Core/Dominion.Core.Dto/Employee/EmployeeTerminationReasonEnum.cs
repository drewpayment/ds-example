using Dominion.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dominion.Core.Dto.Employee
{
    public enum EmployeeTerminationReasonTypeEnum : int
    {
        [Description("Voluntary Termination")]
        VoluntaryTermination = 1,
        [Description("Involuntary Termination")]
        InvoluntaryTermination = 2,
        [Description("Termination for Cause")]
        TerminationForCause = 3,
        [Description("Other")]
        Other = 4,
    }

    public enum EmployeeTerminationReasonEnum : int
    {
        [Description("Absenteeism / Punctuality")]
        Absenteeism_Punctuality = 1, // EmployeeTerminationReasonTypeEnum.TerminationForCause
        [Description("Deceased")]
        Deceased = 2, // EmployeeTerminationReasonTypeEnum.Other
        [Description("Entered Military Service")]
        EnteredMilitaryService = 3, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Expiration of Work Term")]
        ExpirationOfWorkTerm = 4, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Family Obligations")]
        FamilyObligations = 5, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Further Education")]
        FurtherEducation = 6, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Health Reasons")]
        HealthReasons = 7, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Layoff")]
        Layoff = 8, // EmployeeTerminationReasonTypeEnum.InvoluntaryTermination
        [Description("Ineligible for Work")]
        IneligibleForWork = 9, // EmployeeTerminationReasonTypeEnum.Other
        [Description("New Job")]
        NewJob = 10, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Other")]
        Other = 11, // EmployeeTerminationReasonTypeEnum.Other
        [Description("Relocation")]
        Relocation = 12, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Retirement")]
        Retirement = 13, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Transportation")]
        Transportation = 14, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Unknown")]
        Unknown = 15, // EmployeeTerminationReasonTypeEnum.Other
        [Description("Unsatisfactory Performance")]
        UnsatisfactoryPerformance = 16, // EmployeeTerminationReasonTypeEnum.TerminationForCause
        [Description("Violated Company Policy")]
        ViolatedCompanyPolicy = 17, // EmployeeTerminationReasonTypeEnum.TerminationForCause
        [Description("Work Conditions")]
        WorkConditions = 18, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Violated Drug Policy")]
        ViolatedDrugPolicy = 19, // EmployeeTerminationReasonTypeEnum.TerminationForCause
        [Description("Incarceration")]
        Incarceration = 20, // EmployeeTerminationReasonTypeEnum.TerminationForCause
        [Description("Quit")]
        Quit = 21, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Personal Reasons")]
        PersonalReasons = 22, // EmployeeTerminationReasonTypeEnum.VoluntaryTermination
        [Description("Furlough - Natl Tragedy")]
        Furlough_NatlTragedy = 23, // EmployeeTerminationReasonTypeEnum.InvoluntaryTermination
    }

    public static class EmployeeTerminationReasonEnumExtensions
    {
        public static EmployeeTerminationReasonTypeEnum 
        GetEmployeeTerminationReasonTypeEnum(EmployeeTerminationReasonEnum employeeTerminationReasonEnum)
        {
            EmployeeTerminationReasonTypeEnum result;

            switch (employeeTerminationReasonEnum)
            {
                case EmployeeTerminationReasonEnum.Absenteeism_Punctuality:
                    result = EmployeeTerminationReasonTypeEnum.TerminationForCause;
                    break;
                case EmployeeTerminationReasonEnum.Deceased:
                    result = EmployeeTerminationReasonTypeEnum.Other;
                    break;
                case EmployeeTerminationReasonEnum.EnteredMilitaryService:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.ExpirationOfWorkTerm:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.FamilyObligations:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.FurtherEducation:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.HealthReasons:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.Layoff:
                    result = EmployeeTerminationReasonTypeEnum.InvoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.IneligibleForWork:
                    result = EmployeeTerminationReasonTypeEnum.Other;
                    break;
                case EmployeeTerminationReasonEnum.NewJob:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.Other:
                    result = EmployeeTerminationReasonTypeEnum.Other;
                    break;
                case EmployeeTerminationReasonEnum.Relocation:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.Retirement:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.Transportation:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.Unknown:
                    result = EmployeeTerminationReasonTypeEnum.Other;
                    break;
                case EmployeeTerminationReasonEnum.UnsatisfactoryPerformance:
                    result = EmployeeTerminationReasonTypeEnum.TerminationForCause;
                    break;
                case EmployeeTerminationReasonEnum.ViolatedCompanyPolicy:
                    result = EmployeeTerminationReasonTypeEnum.TerminationForCause;
                    break;
                case EmployeeTerminationReasonEnum.WorkConditions:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.ViolatedDrugPolicy:
                    result = EmployeeTerminationReasonTypeEnum.TerminationForCause;
                    break;
                case EmployeeTerminationReasonEnum.Incarceration:
                    result = EmployeeTerminationReasonTypeEnum.TerminationForCause;
                    break;
                case EmployeeTerminationReasonEnum.Quit:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.PersonalReasons:
                    result = EmployeeTerminationReasonTypeEnum.VoluntaryTermination;
                    break;
                case EmployeeTerminationReasonEnum.Furlough_NatlTragedy:
                    result = EmployeeTerminationReasonTypeEnum.InvoluntaryTermination;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }
    }
}
