using System;


namespace Dominion.Core.Dto.Onboarding
{
    [Serializable]
    public class I9DocumentDto
    {
        public virtual int        I9DocumentId                 { get; set; }
        public virtual string       Category                     { get; set; }
        public virtual string     Name                         { get; set; }
       
    }
}
