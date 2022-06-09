using System;
using System.ComponentModel.DataAnnotations;

using Dominion.Core.Dto.Common;
using Dominion.Core.Dto.Contact.Legacy;

namespace Dominion.Core.Dto.Employee
{
    [Flags]
    public enum EmployeeDependentSSNAccessLevels
    {
        View = 0x01, 
        Edit = 0x02
    }

    /// <summary>
    /// Data Transfer Object (DTO) representation of an Employee Dependent Entity
    /// </summary>
    [Serializable]
    public class EmployeeDependentDto : ContactNameDto
    {
        public int EmployeeDependentId { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }

        [RegularExpression("^\\d{3}-\\d{2}-\\d{4}$", 
            ErrorMessage = "Please enter a valid Social Security Number (eg ###-##-####)")]
        public string UnmaskedSocialSecurityNumber { get; set; }

        public string MaskedSocialSecurityNumber { get; set; }

        [Required(ErrorMessage = "Relationship must be specified.")]
        public string Relationship { get; set; }

        [RegularExpression("[MF]", ErrorMessage = "Gender must be specified as either \"M\" = Male or \"F\" = Female")]
        public string Gender { get; set; }

        public string Comments { get; set; }

        public DateTime? BirthDate { get; set; }
        public InsertStatus InsertStatus { get; set; }
        public bool IsAStudent { get; set; }
        public bool HasADisability { get; set; }
        public bool TobaccoUser { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public bool IsSelected { get; set; }
        public EmployeeDependentPcpDto PrimaryCarePhysician { get; set; }
        public bool HasPcp { get; set; }
        public int? EmployeeDependentsRelationshipId { get; set; }
        public bool IsChild { get; set; }
        public bool IsSpouse { get; set; }
        public bool IsInactive { get; set; }
        public DateTime? InactiveDate { get; set; }
    }
}