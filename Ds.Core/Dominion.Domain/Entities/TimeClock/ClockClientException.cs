using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.TimeClock
{
    public class ClockClientException : Entity<ClockClientException>, IHasModifiedData
    {
        public int                                     ClockClientExceptionId      { get; set; }
        public int                                     ClientId                    { get; set; }
        public string                                  Name                        { get; set; }
        public int                                     ModifiedBy                  { get; set; }
        public DateTime                                Modified                    { get; set; }

        //reference entities
        public virtual ICollection<ClockClientExceptionDetail> ExceptionDetails            { get; set; } 
        public virtual Client                                  Client                      { get; set; }

    }
}