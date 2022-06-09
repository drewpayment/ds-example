using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantOnBoardingTaskQuery : IQuery<ApplicantOnBoardingTask, IApplicantOnBoardingTaskQuery>
    {
        IApplicantOnBoardingTaskQuery ByApplicantOnBoardingTaskId(int applicantOnBoardingTaskId);
        IApplicantOnBoardingTaskQuery ByClientId(int clientId);
        IApplicantOnBoardingTaskQuery IsActive(bool isActive);
        IApplicantOnBoardingTaskQuery ExcludeTasksByProcessId(int processId);
    }
}