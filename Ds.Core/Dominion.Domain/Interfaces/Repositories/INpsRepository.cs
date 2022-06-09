using System;
using Dominion.Domain.Interfaces.Query.Nps;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface INpsRepository : IRepository, IDisposable
    {
        IQuestionQuery QueryQuestions();

        IUserTypeQuestionQuery QueryUserTypeQuestions();

        IResponseQuery QueryResponses();
    }
}
