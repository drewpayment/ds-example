using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantsQuery : IQuery<Applicant, IApplicantsQuery>
    {
        IApplicantsQuery ByApplicantId(int applicantId);
        IApplicantsQuery ByUserId(int userId);
        IApplicantsQuery ByEmployeeId(int employeeId);
        IApplicantsQuery ByClientId(int clientId);
    }
}