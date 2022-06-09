using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class IndeedApplicationMaps
    {
        public static Func<ApplicantQuestionControlDto, IndeedQuestion> IndeedQuestionMap =>
                x => new IndeedQuestion()
                {
                    Id = x.QuestionId.ToString(),
                    Type = x.FieldTypeId.FieldTypeToIndeedQuestionType(),
                    Question = x.Question,
                    Options = x.FieldTypeId == FieldType.List || x.FieldTypeId == FieldType.MultipleSelection ?
                        x.ApplicantQuestionDdlItem.Select(y => new IndeedQuestionOption()
                        {
                            Label = y.Description,
                            Value = y.Value
                        }) : x.FieldTypeId == FieldType.Boolean ?
                        new[]{
                            new IndeedQuestionOption()
                        {
                            Label = "Yes",
                            Value = "Yes"
                        }, new IndeedQuestionOption()
                            {
                                Label = "No",
                                Value = "No"
                            }
                        } : null,
                    Required = x.IsRequired,
                    Format = (x.FieldTypeId == FieldType.Date ? "MM/dd/yyyy" :
                                  (x.FieldTypeId == FieldType.Numeric ? "numeric_text" : null)
                                 ),
                    Limit = 1000, // this is the max length of the 'Response' field in the 'ApplicantApplicationDetail' table
                };
    }
}
