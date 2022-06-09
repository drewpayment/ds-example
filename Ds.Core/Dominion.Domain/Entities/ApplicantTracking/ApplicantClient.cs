using System.Collections.Generic;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantClient : Entity<ApplicantClient>, IHasModifiedOptionalData
    {
        public virtual int ClientId { get; set; }
        public virtual string CorrespondenceEmailAddress { get; set; }
        public virtual string JobBoardTitle { get; set; }
        public bool? ShowAboutPage { get; set; }
        public string CompanyURL { get; set; }
        public string PhotoCaption { get; set; }
        public string AboutUs { get; set; }
        public virtual Client Client { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}