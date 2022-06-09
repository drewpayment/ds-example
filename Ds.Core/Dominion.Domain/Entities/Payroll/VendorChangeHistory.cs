using System;
using Dominion.Core.Dto.Common;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class VendorChangeHistory : Entity<VendorChangeHistory>, IHasModifiedData, IHasChangeHistoryDataWithEnum, IVendorEntity
    {
        public virtual int VendorId { get; set; }
        public virtual string Name { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PhoneExtension { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual int TaxFrequencyId { get; set; }
        public virtual int? BankId { get; set; }
        public virtual string RoutingNumber { get; set; }
        public virtual int? AccountTypeId { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int AddressId { get; set; }
        public int ChangeId { get; set; }
        public ChangeModeType ChangeMode { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}