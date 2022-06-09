using System;
using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Nps
{
    public class ScoreDto
    {
        public int UserTypeId { get; set; }
        public string      Title   { get; set; }
        public bool      IsApplicable       { get; set; }
        public decimal Score { get; set; }
        public decimal Numerator { get; set; }
        public decimal Denominator   { get; set; }
        public decimal Percentage     { get; set; }
        public string ColorCode { get; set; }
    }
}
