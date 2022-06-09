using System;
using Dominion.Domain.Entities.Banks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientBankInfo  : Entity<ClientBankInfo>
    {
		public virtual int      ClientId              { get; set; } 
		public virtual int?     AchBankId             { get; set; } 
		public virtual string   BankAccount           { get; set; } 
		public virtual int?     RoutingId             { get; set; } 
		public virtual string   AltBankAccount        { get; set; } 
		public virtual int?     AltRoutingId          { get; set; } 
		public virtual string   TaxAccount            { get; set; } 
		public virtual int?     TaxRoutingId          { get; set; } 
		public virtual string   DebitAccount          { get; set; } 
		public virtual int?     DebitRoutingId        { get; set; } 
		public virtual string   NachaPrefix           { get; set; } 
		public virtual string   TaxAccountNachaPrefix { get; set; } 
		public virtual DateTime Modified              { get; set; } 
		public virtual string   ModifiedBy            { get; set; } 
		public virtual bool     IsSepChkNumbers       { get; set; } 
		public virtual decimal  AchLimit              { get; set; } 

		//FOREIGN KEYS
		public virtual Client Client { get; set; } 
        public virtual Bank Bank { get; set; }
    }
}
