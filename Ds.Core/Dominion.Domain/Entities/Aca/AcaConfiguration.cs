using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// ACA software settings used for e-filing and other high-level configuration. (Entity for [aca].[Configuration] table)
    /// </summary>
    public partial class AcaConfiguration : Entity<AcaConfiguration>, IHasModifiedData
    {
        public virtual int      ConfigurationId      { get; set; } 
        public virtual short    TaxYear              { get; set; }
        public virtual bool     IsPriorYear          { get; set; }
        public virtual string   EnvironmentInd       { get; set; } 
        public virtual string   TranmissionTcc       { get; set; } 
        public virtual string   SoftwareDeveloperTcc { get; set; } 
        public virtual string   SoftwareId           { get; set; } 
        public virtual string   FormTypeCde          { get; set; }
        public virtual string   IrsUrl               { get; set; } 
        public virtual DateTime Modified             { get; set; } 
        public virtual int      ModifiedBy           { get; set; } 
    }
}
