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
    public interface IApplicantReferenceQuery : IQuery<ApplicantReference, IApplicantReferenceQuery>
    {
        IApplicantReferenceQuery ByApplicantReferenceId(int applicantReferenceId);
        IApplicantReferenceQuery ByApplicantId(int applicantId);
        IApplicantReferenceQuery IsEnabled(bool isEnabled);
    }
}
