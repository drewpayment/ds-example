using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface IApplicantTrackingProvider
    {
        IOpResult<ICollection<ApplicationQuestionSectionWithQuestionDetailDto>> GetApplicationSectionsByApplicationId(
            int applicationId, int? applicationHeaderId);

        IOpResult<string> StartApplication(int applicationHeaderId,
            int postingId, int applicantId);

        IOpResult<int> AddApplicantApplicationHeader(int postingId, int applicantId);
        IOpResult<ApplicantApplicationHeaderDto> GetApplicantApplicationHeader(int postingId, int applicantId);
        IOpResult<IEnumerable<ResourceDto>> GetApplicantResourceDetails(int applicationHeaderId);
        IOpResult<int> GetPostingResumeOption(int applicantApplicationHeaderId);
        IOpResult<string> StartApplication(int postingId, int applicantId);
        IOpResult<AddApplicantDto> AddNewUser(AddApplicantDto dto);
    }
}
