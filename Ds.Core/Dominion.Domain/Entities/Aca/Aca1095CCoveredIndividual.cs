using System;

using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// 1095-C Covered Individual information for an employee or employee dependent. (Entity for [dbo].[CompanyAca1095CCoveredIndividual] table)
    /// </summary>
    public partial class Aca1095CCoveredIndividual : Entity<Aca1095CCoveredIndividual>, IHasModifiedData
    {
        public virtual int          CoveredIndividualId { get; set; } 
        public virtual int?         EmployeeDependentId { get; set; } 
        public virtual int          EmployeeId          { get; set; } 
        public virtual short        Year                { get; set; } 
        public virtual bool         IsAllYear           { get; set; } 
        public virtual bool         IsJanuary           { get; set; } 
        public virtual bool         IsFebruary          { get; set; } 
        public virtual bool         IsMarch             { get; set; } 
        public virtual bool         IsApril             { get; set; } 
        public virtual bool         IsMay               { get; set; } 
        public virtual bool         IsJune              { get; set; } 
        public virtual bool         IsJuly              { get; set; } 
        public virtual bool         IsAugust            { get; set; } 
        public virtual bool         IsSeptember         { get; set; } 
        public virtual bool         IsOctober           { get; set; } 
        public virtual bool         IsNovember          { get; set; } 
        public virtual bool         IsDecember          { get; set; } 
        public virtual Aca1095CTinValidationOverrideType? ValidationOverrideType  { get; set; }
        public virtual DateTime     Modified            { get; set; } 
        public virtual int          ModifiedBy          { get; set; } 
        
        public virtual Employee.Employee Employee          { get; set; }
        public virtual EmployeeDependent EmployeeDependent { get; set; }
        public virtual User.User         ModifiedByUser    { get; set; }
        public virtual Aca1095C          Aca1095C          { get; set; }
    }
}
