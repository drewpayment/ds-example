using System;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Clients;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class ClientMaps
    {
        public class ToTimeClockClientDto : ExpressionMapper<Client, TimeClockClientDto>
        {
            public override Expression<Func<Client, TimeClockClientDto>> MapExpression
            {
                get
                {
                    return x => new TimeClockClientDto()
                    {
                        ClientId     = x.ClientId,
                        ClientName   = x.ClientName,
                        ClientCode   = x.ClientCode,
                        AddressLine1 = x.AddressLine1,
                        AddressLine2 = x.AddressLine2,
                        City         = x.City,
                        StateId      = x.State.Abbreviation,
                        PostalCode   = x.PostalCode
                    };
                }
            }
        }
    }
}