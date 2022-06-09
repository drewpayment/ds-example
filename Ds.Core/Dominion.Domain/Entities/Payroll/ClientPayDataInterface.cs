using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    public class ClientPayDataInterface : Entity<ClientPayDataInterface>
    {
        public virtual int ClientPayDataInterfaceId { get; set; }
        public virtual PayrollPayDataInterfaceType PayrollPayDataInterfaceId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string InputFilePath { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual bool IsIncludeOverrideRate { get; set; }
        public virtual string ClientInFile { get; set; }
        public virtual bool? IsUseWorkedWg3Code { get; set; }
        public virtual bool? IsReduceSalaryHours { get; set; }
        public virtual bool IsCombineWg2AndWg3 { get; set; }
        public virtual bool IsIgnorePayDesignation { get; set; }
        public virtual bool IsRoundTotalToNearestQtrHour { get; set; }

        public virtual PayrollPayDataInterface PayrollPayDataInterface { get; set; }
        public virtual Client Client { get; set; }
    }
}