using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IFeedbackQuery : IQuery<Feedback, IFeedbackQuery>
    {
        /// <summary>
        /// Filters feedback belonging to a specific client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IFeedbackQuery ByClient(int clientId);

        /// <summary>
        /// Filters by a single feedback.
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        IFeedbackQuery ByFeedbackId(int feedbackId);

        /// <summary>
        /// Filters feedback by its archived-status. Defaults to filtering out
        /// archived feedback.
        /// </summary>
        /// <returns></returns>
        IFeedbackQuery ByArchived(bool isArchived = false);

        /// <summary>
        /// Filters feedback by those belonging to specific steps of the review process.
        /// Selected steps will be "OR"-ed together (e.g. if <see cref="isForSupervisorEval"/>
        /// and <see cref="isForPeerEval"/> are true, then feedback results that belong to
        /// the Supervisor Eval step OR the Peer Eval step will be returned).
        /// </summary>
        /// <param name="isForSupervisorEval"></param>
        /// <param name="isForPeerEval"></param>
        /// <param name="isForSelfEval"></param>
        /// <param name="isForActionPlanning"></param>
        /// <returns></returns>
        IFeedbackQuery ByIsForSpecificReviewSteps(bool isForSupervisorEval = false, bool isForPeerEval = false, bool isForSelfEval = false, bool isForActionPlanning = false);
        IFeedbackQuery ByDefault();
        IFeedbackQuery ByFeedbackIdList(List<int> feedbackIdList);
    }
}
