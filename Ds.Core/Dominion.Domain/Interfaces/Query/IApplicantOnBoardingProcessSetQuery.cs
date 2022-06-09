using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantOnBoardingProcessSetQuery : IQuery<ApplicantOnBoardingProcessSet, IApplicantOnBoardingProcessSetQuery>
    {
        IApplicantOnBoardingProcessSetQuery ByApplicantOnboardingProcessId(int applicantOnboardingProcessId);
        IApplicantOnBoardingProcessSetQuery ByApplicantOnboardingTaskId(int applicantOnboardingTaskId);
        IApplicantOnBoardingProcessSetQuery OrderByOrderId();
    }
}
