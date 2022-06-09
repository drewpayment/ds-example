using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientUnemploymentSetup : Entity<ClientUnemploymentSetup>
    {
        public virtual int ClientUnemploymentSetupID { get; set; }
        public virtual int ClientID { get; set; }
        public virtual  int ClientUnemploymentProviderID { get; set; }
        public virtual string ContractPlanNumber { get; set; }
        public virtual int ClientUnemploymentFormatTypeID { get; set; }
        public virtual int FrequencyID { get; set; }
        public virtual string FileName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsWOTC { get; set; }
    }
}
