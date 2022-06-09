using System;

using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    public partial class AcaCorrection : Entity<AcaCorrection>, IHasModifiedData
    {
        public virtual short               Year                  { get; set; } 
        public virtual int                 CorrectionId          { get; set; } 
        public virtual EFileCorrectionType CorrectionTypeId      { get; set; } 
        public virtual int?                CompanyAcaAleMemberId { get; set; } 
        public virtual int?                ClientId              { get; set; } 
        public virtual int?                EmployeeId            { get; set; } 
        public virtual string              IssueDescription      { get; set; } 
        public virtual string              ResolutionDescription { get; set; } 
        public virtual string              Notes                 { get; set; }
        public virtual bool                IsClientCorrection    { get; set; } 
        public virtual bool                IsSystemCorrection    { get; set; } 
        public virtual bool                IsIrsCorrection       { get; set; } 
        public virtual bool                IsCorrected           { get; set; } 
        public virtual DateTime?           CorrectedDate         { get; set; } 
        public virtual int?                CorrectedBy           { get; set; } 
        public virtual DateTime            Modified              { get; set; } 
        public virtual int                 ModifiedBy            { get; set; } 

        public virtual AcaAleMemberClient   AleMemberClient   { get; set; }
        public virtual Client               Client            { get; set; }
        public virtual Employee.Employee    Employee          { get; set; }
        public virtual User.User            ModifiedByUser    { get; set; }
        public virtual User.User            CorrectedByUser   { get; set; }
        public virtual Aca1095C             Aca1095C          { get; set; }
        public virtual AcaTransmissionError TransmissionError { get; set; }
    }
}
