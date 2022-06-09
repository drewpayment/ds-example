using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Common;

namespace Dominion.Domain.Entities.Employee
{
    public interface IEmployeeDependentEntity
    {
        int EmployeeDependentId { get; set; }
        int EmployeeId { get; set; }
        Employee Employee { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleInitial { get; set; }
        string SocialSecurityNumber { get; set; }
        string Relationship { get; set; }
        string Gender { get; set; }
        string Comments { get; set; }
        DateTime? BirthDate { get; set; }
        int ClientId { get; set; }
        bool TobaccoUser { get; set; }
        bool HasADisability { get; set; }
        bool IsAStudent { get; set; }
        int? EmployeeDependentsRelationshipId { get; set; }
        bool IsInactive { get; set; }
        DateTime? InactiveDate { get; set; }
        InsertStatus InsertStatus { get; set; }
    }
}
