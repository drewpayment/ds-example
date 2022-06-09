using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedArray<T>
    {
        [JsonProperty(PropertyName = "_total")]
        public int Total { get; set; }
        public IEnumerable<T> Values { get; set; }
    }
}