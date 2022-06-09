using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee.Search
{

    public class ReviewTemplateOption : IEmployeeSearchFilterOption
    {
        
        public virtual EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.ReviewCycleReview;

        public int Id => ReviewTemplateId;

        public string Name { get; set; }
        [JsonIgnore]
        public int ReviewTemplateId { get; set; }

        public IEmployeeSearchFilterOption ParentOption => null;

        public virtual bool Evaluate(EmployeeSearchDto dto)
        {
            throw new NotImplementedException("Use implementation in one of the subclasses");
        }
    }

    public class HasReviewTemplateOption : ReviewTemplateOption
    {
        public override EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.DoesNotHaveReviewCycleReview;
        public override bool Evaluate(EmployeeSearchDto dto)
        {
            return dto.Groups.Any(g => (g.FilterType == FilterType || g.FilterType == EmployeeSearchFilterType.ReviewCycleReview) && g.Id == Id);
        }
    }

    public class DoesNotHaveReviewTemplateOption : ReviewTemplateOption
    {
        public override EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.DoesNotHaveReviewCycleReview;
        /// <summary>
        /// A function to determine how the result of the evaluation of this option should be combined with the result of the evaluations of other options.
        /// </summary>
        private readonly Func<bool, EmployeeSearchDto, bool> _buildPredicate = (prevResult, data) => prevResult;
        public DoesNotHaveReviewTemplateOption()
        {

        }
        public DoesNotHaveReviewTemplateOption(Func<bool, EmployeeSearchDto, bool> buildPredicate)
        {
            _buildPredicate = buildPredicate;
        }
        public override bool Evaluate(EmployeeSearchDto dto)
        {
            return _buildPredicate(!dto.Groups.Any(g => (g.FilterType == FilterType || g.FilterType == EmployeeSearchFilterType.ReviewCycleReview) && g.Id == Id), dto);
        }
    }
}
