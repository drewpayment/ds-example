using System;


namespace Dominion.Core.Dto.Client
{
    [Serializable]
    public class ClientRateDto
    {
        public virtual int ClientRateId   { get; set; } 
        public virtual int ClientId       { get; set; } 
        public virtual string    Description    { get; set; } 
        public virtual string Code        { get; set; } 
        public virtual DateTime? Modified       { get; set; } 
        public virtual string ModifiedBy  { get; set; } 

    }

    [Serializable]
    public class ClientRateListDto
    {
        public virtual int    ClientRateId  { get; set; } 
        public virtual string Description   { get; set; } 
        public virtual string Code          { get; set; }
    }
}
