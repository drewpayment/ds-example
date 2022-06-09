using System;
using Dominion.Core.Dto.InstantPay;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;

namespace Dominion.Domain.Entities.InstantPay
{
    public class InstantPayDirectDepositChange : Entity<InstantPayDirectDepositChange>
    {
        public virtual int                InstantPayChangeId   { get; set; }
        public virtual int                ClientId             { get; set; }
        public virtual int                EmployeeId           { get; set; }
        public virtual InstantPayProvider InstantPayProviderId { get; set; }
        public virtual string             AccountOriginator    { get; set; }
        public virtual string             RoutingNumber        { get; set; }
        public virtual string             AccountNumber        { get; set; }
        public virtual bool               IsActive             { get; set; }
        public virtual DateTime           ChangeDate           { get; set; }


        //Foreign Keys
        public virtual Client            Client       { get; set; }
        public virtual Employee.Employee Employee     { get; set; }
    }
}
