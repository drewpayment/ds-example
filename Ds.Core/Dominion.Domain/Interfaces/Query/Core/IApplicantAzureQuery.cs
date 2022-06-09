using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IApplicantAzureQuery : IQuery<ApplicantAzure, IApplicantAzureQuery>
    {
        IApplicantAzureQuery ByApplicant(int applicantId);
    }
}
