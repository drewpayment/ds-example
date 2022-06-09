using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Nps;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Nps
{
    public interface IResponseQuery : IQuery<Response, IResponseQuery>
    {
        IResponseQuery ByResponseId(int responseId);

        IResponseQuery ByQuestionId(int questionId);

        IResponseQuery ByUserId(int userId);
        IResponseQuery ByUserTypes(IEnumerable<UserType> userTypes);

        IResponseQuery ByClientId(int clientId);

        IResponseQuery ByClientIds(List<int> clientIds);

        IResponseQuery ByClientCode(string clientCode);

        IResponseQuery ByFromDate(DateTime fromDate);

        IResponseQuery ByToDate(DateTime toDate);

        IResponseQuery ByClientStatusId(ClientStatusType clientStatusId);

        //IResponseQuery ByClientOrganizationId(int clientOrganizationId);

        IResponseQuery ByResponseTypeId(ResponseType responseTypeId);

        IResponseQuery ByResponseTypes(IEnumerable<ResponseType> responseTypes);

        IResponseQuery HideResponsesWithoutFeedback(bool flag);

        IResponseQuery ByIsResolved(bool flag);

        IResponseQuery BySearchFeedback(string searchString);
    }
}
