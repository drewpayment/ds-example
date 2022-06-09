using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;
using Dominion.Core.Dto.Core;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface ISystemFeedbackQuery : IQuery<SystemFeedback, ISystemFeedbackQuery>
    {
        /// <summary>
        /// Filters entities by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ISystemFeedbackQuery ByUserId(int userId);

        ISystemFeedbackQuery BySystemFeedbackType(SystemFeedbackType type);
    }
}
