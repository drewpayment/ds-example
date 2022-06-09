using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Tax.Legacy;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Tax
{
    /// <summary>
    /// Container class for a Client Tax entity.
    /// </summary>
    public class ClientTax : Entity<ClientTax>, IModifiableEntity<ClientTax>, IClientTaxWithLegacyTaxes<LegacyLocalTax, LegacyStateTax, LegacyDisabilityTax>
    {
        public virtual int               ClientTaxId                    { get; set; }
        public virtual int               ClientId                       { get; set; }
        public virtual Client            Client                         { get; set; }
        public virtual string            TaxIdNumber                    { get; set; }
        public virtual int               TaxFrequencyId                 { get; set; }
        public virtual int?              StateTaxId                     { get; set; }
        public virtual int?              LocalTaxId                     { get; set; }
        public virtual string            Description                    { get; set; }
        public virtual string            UnemploymentId                 { get; set; }
        public virtual byte?             CalendarDebitId                { get; set; }
        public virtual bool              IsIncludeInElectronicInterface { get; set; }
        public virtual byte?             SutaCalendarDebitId            { get; set; }
        public virtual bool              IsIncludeInW2ElectronicFile    { get; set; }
        public virtual int?              DisabilityTaxId                { get; set; }
        public virtual string            AlternateVendorName            { get; set; }
        public virtual DateTime          LastSutaCatchupDate            { get; set; }
        public virtual int?              OtherTaxId                     { get; set; }
        public virtual int?              ResidentId                     { get; set; }
        public virtual LegacyTaxType     LegacyTaxType                  { get; set; }
        public virtual LegacyTaxTypeInfo LegacyTaxTypeInfo              { get; set; }
        public virtual bool?             TaxIsActive                    { get; set; }
        public virtual string            EftCreditId                    { get; set; }

        public virtual LegacyLocalTax LegacyLocalTax { get; set; }
        public virtual LegacyLocalTax LegacyOtherTax { get; set; }
        public virtual LegacyStateTax LegacyStateTax { get; set; }
        public virtual LegacyDisabilityTax LegacyDisabilityTax { get; set; }
        public virtual ICollection<ClientTaxInfo> TaxInfos { get; set; }
        public ICollection<ClientTaxPaymentInfo> TaxPaymentInfos { get; set; }

        #region IModifiableEntity IMPLEMENTATION

        public virtual DateTime LastModifiedDate { get; set; }
        public virtual string LastModifiedByDescription { get; set; }
        public virtual User.User LastModifiedByUser { get; private set; }
        public virtual int LastModifiedByUserId { get; private set; }

        public void SetLastModifiedValues(
            int lastModifiedByUserId, 
            string lastModifiedByUserName, 
            DateTime lastModifiedDate)
        {
            LastModifiedByUserId      = lastModifiedByUserId;
            LastModifiedByDescription = lastModifiedByUserId.ToString();
            LastModifiedDate          = lastModifiedDate;
        }

        #endregion // IModifiableEntity IMPLEMENTATION
    }
}