using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Push
{
    public class PushUploadFileHistory : Entity<PushUploadFileHistory>
    {
        public virtual int       Id              { get; set; }
        public virtual string    FileName        { get; set; }
        public virtual string    DestinationPath { get; set; }
        public virtual DateTime? CreateTime      { get; set; }
    }
}
