using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Banks
{
    public class AchBank : Entity<AchBank>
    {
        public int       AchBankId                   { get; set; }                
        public string    ImmediateDestination        { get; set; }     
        public string    DestinationName             { get; set; }          
        public string    ImmediateOrigin             { get; set; }          
        public string    OriginName                  { get; set; }               
        public string    FileIdModifier              { get; set; }           
        public string    TraceNumberOption           { get; set; }        
        public bool?     IsReferenceCodeIsZeros      { get; set; }     
        public string    DsiBankCode                 { get; set; }              
        public bool?     IsUnbalanced                { get; set; }             
        public string    AchFileHeader               { get; set; }            
        public bool?     IsSeparateFileForEachClient { get; set; }
        public bool      IsTaxManagementBank         { get; set; }      
        public bool      IsCombineFileByFederalId    { get; set; }   
        public string    DisplayName                 { get; set; }              
        public TimeSpan? CutOffTime                  { get; set; }               
        public bool?     UseCcdBatchOffsetToBalance  { get; set; }
        public string    ReferenceCode               { get; set; }            
    }
}
