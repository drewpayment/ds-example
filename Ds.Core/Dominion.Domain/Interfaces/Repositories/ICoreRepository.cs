using Dominion.Domain.Entities.Banks;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Forms;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Banks;
using Dominion.Domain.Interfaces.Query.Core;
using Dominion.Domain.Interfaces.Query.Forms;
using Dominion.Domain.Interfaces.Query.Performance;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository providing query access to core elements of the application.
    /// </summary>
    public interface ICoreRepository
    {
        /// <summary>
        /// Constructs a new query on <see cref="Resource"/> data.
        /// </summary>
        /// <returns></returns>
        IResourceQuery QueryResources();

        /// <summary>
        /// Constructs a new query on <see cref="EmployeeAttachment"/> data.
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentQuery QueryEmployeeAttachments();

        /// <summary>
        /// Constructs a new query on <see cref="EmployeeAttachmentFolder"/> data.
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery QueryEmployeeAttachmentFolders();

        /// <summary>
        /// Constructs a new query on <see cref="FormType"/> data.
        /// </summary>
        /// <returns></returns>
        IFormTypeQuery QueryFormTypes();

        /// <summary>
        /// Constructs a new query on <see cref="FormDefinition"/> data.
        /// </summary>
        /// <returns></returns>
        IFormDefinitionQuery QueryFormDefinitions();

        /// <summary>
        /// Constructs a new query on <see cref="FormDefinitionFields"/> data.
        /// </summary>
        /// <returns></returns>
        IFormDefinitionFieldsQuery QueryFormDefinitionFields();

        /// <summary>
        /// Constructs a new query on <see cref="Form"/> data.
        /// </summary>
        /// <returns></returns>
        IFormQuery QueryForms();

        ///<summary>
        /// Constructs a new query on <see cref="RoundingRuleTypeInfo"/> data.
        /// </summary>
        /// <returns></returns>
        IRoundingRuleTypeInfoQuery QueryRoundingRuleTypeInfo();

        IClientAzureQuery QueryClientAzure();
        IAlertQuery QueryAlerts();
        IEmployeeAzureQuery QueryEmployeeAzure();
        IImageResourceQuery QueryImageResource();
        IApplicantAzureQuery QueryApplicantAzure();

        IImageSizeTypeInfoQuery QueryImageSizeTypeInfo();

        /// <summary>
        /// Constructs a new query on <see cref="ReviewRating"/> data.
        /// </summary>
        /// <returns></returns>
        IReviewRatingQuery ReviewRatingQuery();

        /// <summary>
        /// Constructs a new query on <see cref="CompetencyGroup" /> data.
        /// </summary>
        /// <returns></returns>
        ICompetencyGroupQuery CompetencyGroupQuery();

        /// <summary>
        /// Constructs a new query on <see cref="Task"/> data.
        /// </summary>
        /// <returns></returns>
        ITaskQuery TaskQuery();

        /// <summary>
        /// Constructs a new query on <see cref="Goal"/> data.
        /// </summary>
        /// <returns></returns>
        IGoalQuery GoalQuery();
        IGroupQuery GroupQuery();

        /// <summary>
        /// Constructs a new query on <see cref="Remark"/> data.
        /// </summary>
        /// <returns></returns>
        IRemarkQuery RemarkQuery();

        /// <summary>
        /// Constructs a new query on <see cref="RemarkChangeHistory"/> data.
        /// </summary>
        /// <returns></returns>
        IRemarkChangeHistoryQuery RemarkChangeHistoryQuery();

        /// <summary>
        /// Constructs a new query on <see cref="EmployeeGoal"/> data.
        /// </summary>
        /// <returns></returns>
        IEmployeeGoalQuery EmployeeGoalQuery();

        /// <summary>
        /// Constructs a new query on <see cref="ClientGoal"/> data.
        /// </summary>
        /// <returns></returns>
        IClientGoalQuery ClientGoalQuery();

        /// <summary>
        /// Constructs a new query on <see cref="Meeting"/> data.
        /// </summary>
        /// <returns></returns>
        IMeetingQuery QueryMeetings();
        IScoreModelQuery ScoreModelQuery();
        IScoreMethodTypeQuery ScoreMethodTypeQuery();
        IScoreRangeLimitQuery ScoreRangeLimitQuery();
        IProposalQuery QueryProposals();

        /// <summary>
        /// Constructs a new query on <see cref="Bank"/> data.
        /// </summary>
        /// <returns></returns>
        IBankQuery QueryBanks();

        /// <summary>
        /// Constructs a new query on <see cref="AchBank"/> data.
        /// </summary>
        /// <returns></returns>
        IAchBankQuery QueryAchBanks();
        IBetaFeatureQuery BetaFeatureQuery();
        ITermsAndConditionsVersionQuery QueryTermsAndConditionsVersions();
        ISystemFeedbackQuery SystemFeedbackQuery();
    }
}
