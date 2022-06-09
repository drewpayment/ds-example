using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Performance;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IPerformanceRepository
    {
        /// <summary>
        /// Query <see cref="Competency"/> data.
        /// </summary>
        /// <returns></returns>
        ICompetencyQuery QueryCompetencies();

        /// <summary>
        /// Query <see cref="CompetencyModel"/> data.
        /// </summary>
        /// <returns></returns>
        ICompetencyModelQuery QueryCompetencyModels();

        /// <summary>
        /// Query <see cref="EmployeePerformanceConfiguration"/> data.
        /// </summary>
        /// <returns></returns>
        IEmployeePerformanceConfigurationQuery QueryEmployeePerformanceConfigurations();

        /// <summary>
        /// Query <see cref="Evaluation"/> data;
        /// </summary>
        /// <returns></returns>
        IEvaluationQuery QueryEvaluations();

        /// <summary>
        /// Query <see cref="Review"/> data.
        /// </summary>
        /// <returns></returns>
        IReviewQuery QueryReviews();

        /// <summary>
        /// Query <see cref="Feedback"/> data.
        /// </summary>
        /// <returns></returns>
        IFeedbackQuery QueryFeedback();

        /// <summary>
        /// Query <see cref="ReviewProfile"/> data.
        /// </summary>
        /// <returns></returns>
        ICompetencyOptionsQuery CompetencyOptionsQuery();
        IGoalOptionsQuery GoalOptionsQuery();
        ICompetencyRateCommentRequiredQuery CompetencyRateCommentRequiredQuery();
        IGoalRateCommentRequiredQuery GoalRateCommentRequiredQuery();

        /// <summary>
        /// Query <see cref="ReviewTemplate"/> data.
        /// </summary>
        /// <returns></returns>
        IReviewTemplateQuery QueryReviewTemplates();

        ICompetencyEvaluationGroupQuery QueryCompetencyEvaluationGroups();
        IReviewProfileEvaluationQuery QueryReviewProfileEvaluations();
        
        /// <summary>
        /// Query <see cref="EvaluationGroup"/> data.
        /// </summary>
        /// <returns></returns>
        IEvaluationGroupQuery QueryEvaluationGroups();
        IApprovalProcessHistoryQuery QueryEvaluationHistory();
        IReviewProfileQuery QueryReviewProfiles();
        ICompetencyEvaluationQuery QueryCompetencyEvaluations();
        IGoalEvaluationQuery QueryGoalEvaluations();
        IEvaluationFeedbackResponseQuery QueryEvaluationFeedbackResponses();
        IEvaluationGroupEvaluationQuery EvaluationGroupEvaluationQuery();
    }
}
