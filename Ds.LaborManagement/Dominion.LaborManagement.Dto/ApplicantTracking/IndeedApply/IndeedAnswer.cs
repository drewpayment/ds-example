using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedAnswer
    {
        public string Id { get; set; }
        public IEnumerable<IndeedAnswer> HierarchicalAnswers { get; set; }
        public IEnumerable<IndeedFilePropertyBag> Files { get; set; }
        public string Value { private get; set; }

        private IEnumerable<string>  _values;
        public IEnumerable<string> Values {
            private get {
                return _values == null ? new List<string>() : _values;
            } set
            {
                _values = value;
            }
        }

        public IEnumerable<string> GetValues()
        {
            return Value == null ? Values : new[] { Value };
        }
    }
}
