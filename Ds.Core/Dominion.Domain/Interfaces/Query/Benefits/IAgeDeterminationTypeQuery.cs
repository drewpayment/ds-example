﻿using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IAgeDeterminationTypeQuery : IQuery<AgeDeterminationType, IAgeDeterminationTypeQuery>
    {
        IAgeDeterminationTypeQuery ByAgeDeterminationType(AgeDetermination ageDeterminationTypeId);
    }
}