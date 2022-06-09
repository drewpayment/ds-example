using System;

using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// IRS/Dominion error code related to ACA e-file processing. (Entity for aca.ErrorCode table)
    /// </summary>
    public partial class AcaErrorCode : Entity<AcaErrorCode>, IHasModifiedData
    {
        public virtual int                      ErrorCodeId           { get; set; } 
        public virtual string                   IrsErrorCode          { get; set; } 
        public virtual AcaIrsErrorSeverityType? IrsErrorSeverity      { get; set; } 
        public virtual string                   IrsDescription        { get; set; } 
        public virtual string                   SystemDescription     { get; set; } 
        public virtual string                   ResolutionDescription { get; set; } 
        public virtual bool                     IsPortalError         { get; set; }
        public virtual bool                     IsManifestError       { get; set; }
        public virtual bool                     Is1094CError          { get; set; }
        public virtual bool                     Is1095CError          { get; set; }
        public virtual bool                     IsClientCorrection    { get; set; } 
        public virtual bool                     IsSystemCorrection    { get; set; } 
        public virtual bool                     IsIrsCorrection       { get; set; } 
        public virtual bool                     IsDeleted             { get; set; } 
        public virtual DateTime                 Modified              { get; set; } 
        public virtual int                      ModifiedBy            { get; set; } 

        public virtual User.User ModifiedByUser { get; set; }
    }
}
