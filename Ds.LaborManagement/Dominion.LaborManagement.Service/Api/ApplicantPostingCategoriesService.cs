using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Mapping;
using Dominion.Core.Services.Security.Authorization;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.User;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Dto.Department;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;
using Dominion.LaborManagement.Service.Internal;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.LaborManagement.Service.Mapping.Clock;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query.LinqKit;
using Dominion.Utility.Security;
using AuthorizationStreamExtensions = Dominion.LaborManagement.Service.Internal.AuthorizationStreamExtensions;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.Service.Api
{
    public class ApplicantPostingCategoriesService : IApplicantPostingCategoriesService
    {
        #region Variables and Properties

        private readonly IBusinessApiSession _session;
        private readonly IServiceAuthorizer _authorizer;

        #endregion

        #region Constructors and Initializers

        public ApplicantPostingCategoriesService(IBusinessApiSession session)
        {
            _session = session;
            _authorizer = new ServiceAuthorizer(session);
        }

        #endregion

        #region Methods and Commands 


        #endregion
    }
}
