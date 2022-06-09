using Dominion.Core.Dto.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class GroupDto
    {
        public int GroupId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public GroupJsonDto jsonData { get; set; }
        public IEnumerable<int> ReviewTemplates { get; set; }
        public IEnumerable<int> CompetencyModels { get; set; }
    }

    public class GroupJsonDto
    {
        public IEnumerable<int> Statuses { get; set; }
        public ICollection<int> Departments { get; set; }
        public IEnumerable<int> JobTitles { get; set; }
        public IEnumerable<int> CostCenters { get; set; }
        public IEnumerable<int> PayTypes { get; set; }
        public DateUnit? DateUnit { get; set; }
        public int? Duration { get; set; }
        public LengthOfServiceBoundType? BoundType { get; set; }
    }

    public enum LengthOfServiceBoundType
    {
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
        EqualTo
    }

    public static class LengthOfServiceBoundTypeExtenstion
    {
        public static bool Evaluate(this LengthOfServiceBoundType bound, long first, long second)
        {
            switch (bound)
            {
                case LengthOfServiceBoundType.EqualTo:
                    return first == second;
                case LengthOfServiceBoundType.GreaterThan:
                    return first > second;
                case LengthOfServiceBoundType.GreaterThanOrEqualTo:
                    return first >= second;
                case LengthOfServiceBoundType.LessThan:
                    return first < second;
                case LengthOfServiceBoundType.LessThanOrEqualTo:
                    return first <= second;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
