using System;

using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Single 1095-C line item for an employee (eg: Box 14, 15, 16, etc). (Entity for [dbo].[CompanyAca1095C] table)
    /// </summary>
    public partial class Aca1095CLineItem : Entity<Aca1095CLineItem>, IHasModifiedData
    {
        public virtual int                 Aca1095LineItemId { get; set; } 
        public virtual int?                 EmployeeId        { get; set; } 
        public virtual Aca1095CBoxCodeType BoxCodeId         { get; set; } 
        public virtual short?               Year              { get; set; } 
        public virtual string              AllYear           { get; set; } 
        public virtual string              January           { get; set; } 
        public virtual string              February          { get; set; } 
        public virtual string              March             { get; set; } 
        public virtual string              April             { get; set; } 
        public virtual string              May               { get; set; } 
        public virtual string              June              { get; set; } 
        public virtual string              July              { get; set; } 
        public virtual string              August            { get; set; } 
        public virtual string              September         { get; set; } 
        public virtual string              October           { get; set; } 
        public virtual string              November          { get; set; } 
        public virtual string              December          { get; set; } 
        public virtual DateTime            Modified          { get; set; } 
        public virtual int                 ModifiedBy        { get; set; } 

        public virtual Aca1095C                Aca1095C                { get; set; }
        public virtual Aca1095CBoxCodeTypeInfo BoxCodeTypeInfo         { get; set; }
        public virtual Employee.Employee       Employee                { get; set; }
        public virtual User.User               ModifiedByUser          { get; set; }
    }
}
