using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking.Application;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantQuestionControlQuery : Query<ApplicantQuestionControl, IApplicantQuestionControlQuery>, IApplicantQuestionControlQuery
    {
        public ApplicantQuestionControlQuery(IEnumerable<ApplicantQuestionControl> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        public IApplicantQuestionControlQuery ByQuestionIds(IEnumerable<int> questionIds)
        {
            FilterBy(x => questionIds.Contains(x.QuestionId));
            return this;
        }

        IApplicantQuestionControlQuery IApplicantQuestionControlQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicantQuestionControlQuery IApplicantQuestionControlQuery.BySectionId(int sectionId)
        {
            FilterBy(x => x.SectionId == sectionId);
            return this;
        }

        IApplicantQuestionControlQuery IApplicantQuestionControlQuery.ByQuestionId(int questionId)
        {
            FilterBy(x => x.QuestionId == questionId);
            return this;
        }
        IApplicantQuestionControlQuery IApplicantQuestionControlQuery.ByQuestionText(string questionText)
        {
            FilterBy( x => x.Question.ToLower().Trim(new char[] {' ','?','.' }) == 
                            questionText.ToLower().Trim(new char[] { ' ', '?', '.' }) );
            return this;
        }

        IApplicantQuestionControlQuery IApplicantQuestionControlQuery.ByFieldTypeId(FieldType fieldTypeId)
        {
            FilterBy(x => x.FieldTypeId == fieldTypeId);
            return this;
        }

        IApplicantQuestionControlQuery IApplicantQuestionControlQuery.ByIsActive(bool flag)
        {
            FilterBy(x => x.IsEnabled == flag);
            return this;
        }
        IApplicantQuestionControlQuery IApplicantQuestionControlQuery.ByApplication(IEnumerable<int> applicationIds)
        {
            FilterBy(x => x.ApplicantQuestionSets.Any( y => applicationIds.Contains(y.ApplicationId) ) );
            return this;
        }
        IQueryResult<ApplicantQuestionControlAndQuestionSets> IApplicantQuestionControlQuery.getApplicationQuestionSets(IApplicantQuestionSetQuery questionSetQuery)
        {
            return questionSetQuery.Result.InnerJoin(this.Result, new ApplicantQuestionControlsApplicantQuestionSets());
            
        }

        private class ApplicantQuestionControlsApplicantQuestionSets : IInnerJoinExpressions<ApplicantQuestionSet, ApplicantQuestionControl, int, ApplicantQuestionControlAndQuestionSets>
        {
            public Expression<Func<ApplicantQuestionSet, int>> OuterKey
            {
                get { return set => set.QuestionId; }
            }
            public Expression<Func<ApplicantQuestionControl, int>> InnerKey
            {
                get { return control => control.QuestionId; }
            }
            public Expression<Func<ApplicantQuestionSet, ApplicantQuestionControl, ApplicantQuestionControlAndQuestionSets>> Select
            {
                get
                {
                    
                   return (set, control) => new ApplicantQuestionControlAndQuestionSets()
                   {
                       QuestionId = control.QuestionId,
                       Question = control.Question,
                       ResponseTitle = control.ResponseTitle,
                       FieldTypeId = control.FieldTypeId,
                       SectionId = control.SectionId,
                       OrderId = set.OrderId,
                       FlaggedResponse = control.IsFlagged ? control.FlaggedResponse : "",
                       IsRequired = control.IsRequired,
                       ClientId = control.ClientId,
                       IsEnabled = control.IsEnabled,
                       IsFlagged = control.IsFlagged,
                       SelectionCount = control.SelectionCount,
                       ApplicantQuestionDdlItem = control.ApplicantQuestionDdlItem,
                       ApplicationQuestionSection = control.ApplicationQuestionSection
                   };
                }
            }
        }
    }
}