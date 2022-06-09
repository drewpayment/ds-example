using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    public class CustomBenefitFieldValue : Entity<CustomBenefitFieldValue>, IHasModifiedData
    {
        public int      CustomFieldValueId { get; set; }
        public int      CustomFieldId      { get; set; }
        public int      ClientId           { get; set; }
        public int?     EmployeeId         { get; set; }
        public int?     DependentId        { get; set; }
        public string   TextValue          { get; set; }
        public int      ModifiedBy         { get; set; }
        public DateTime Modified           { get; set; }

        public CustomBenefitField Field     { get; set; }
        public Client             Client    { get; set; }
        public Employee.Employee  Employee  { get; set; }
        public EmployeeDependent  Dependent { get; set; }
    }
}