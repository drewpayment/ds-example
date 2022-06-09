using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantDocument : Entity<ApplicantDocument>
    {
        public virtual int ApplicantDocumentId { get; set; }
        public virtual string DocumentName { get; set; }
        public virtual int ApplicationHeaderId { get; set; }
        public virtual string LinkLocation { get; set; }
        public virtual DateTime DateAdded { get; set; }

        //FOREIGN KEYS
        public virtual ApplicantApplicationHeader ApplicantApplicationHeader { get; set; }

        public ApplicantDocument()
        {

        }
    }
}