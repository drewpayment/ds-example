using Dominion.Core.Dto.Billing;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Geofence;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Domain.Entities.Employee.ClockEmployeeInfo;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.Utility.Constants;

namespace Dominion.LaborManagement.Service.Api
{
    public class GeoService : IGeoService
    {
        #region Variables and Properties

        private readonly IBusinessApiSession _session;
        private readonly IGeoProvider _geoProvider;
        private readonly IClientNotesProvider _clientNotesProvider;
        private readonly IAccountFeatureService _accountFeatureService;
        private readonly IBillingProvider _billingProvider;

        internal IGeoService Self { get; set; }

        #endregion

        #region Constructors and Initializers

        public GeoService(
            IBusinessApiSession session,
            IGeoProvider geoProvider,
            IClientNotesProvider clientNotesProvider,
            IAccountFeatureService accountFeatureService,
            IBillingProvider billingProvider
        )
        {
            Self = this;

            _session = session;
            _geoProvider = geoProvider;
            _clientNotesProvider = clientNotesProvider;
            _accountFeatureService = accountFeatureService;
            _billingProvider = billingProvider;
        }

        public IOpResult<ClockEmployeePunchLocation> ProcessRealTimePunchLocation(RealTimePunchLocation request)
        {
            var opResult = new OpResult<ClockEmployeePunchLocation>();
            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(opResult);
            _session.CanPerformAction(ClockActionType.User).MergeInto(opResult);

            if (opResult.HasError) return opResult.SetToFail("You do not have access to this client");

            if (request == null) return opResult;

            return _geoProvider.ProcessRealTimePunchLocation(request);
        }

        public IOpResult<bool> CheckPunchLocation(RealTimePunchLocation request)
        {
            var result = new OpResult<bool>();
            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this client");


            _geoProvider.CheckPunchLocation(request).MergeInto(result);

            return result;
        }

        public IOpResult<IEnumerable<ClockClientGeofenceDto>> GetClientGeofences(int clientId)
        {
            var result = new OpResult<IEnumerable<ClockClientGeofenceDto>>();
            var user = _session.LoggedInUserInformation;

            // _session.CanPerformAction(ClockActionType.CanOptInToCompanyFeature).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to geofences");

            var geofenceResult = _geoProvider.GetClientGeofences(clientId);
            geofenceResult.MergeInto(result);
            result.SetDataOnSuccess(geofenceResult.Data);

            return result;
        }

        public IOpResult<DateTime?> GetClientLastModified(int clientId)
        {
            var result = new OpResult<DateTime?>();
            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.CanOptInToCompanyFeature).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to geofences");

            var dateResult = _geoProvider.GetClientLastModified(clientId);
            dateResult.MergeInto(result);
            result.SetDataOnSuccess(dateResult.Data);

            return result;
        }

        public IOpResult<ClockClientGeofenceDto> AddClientGeofence(ClockClientGeofenceDto geofence)
        {
            var result = new OpResult<ClockClientGeofenceDto>();
            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.CanOptInToCompanyFeature).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to geofences");

            var geofenceResult = _geoProvider.ProcessClientGeofence(geofence);
            geofenceResult.MergeInto(result);
            result.SetDataOnSuccess(geofenceResult.Data);

            return result;
        }

        public IOpResult<ClockClientGeofenceDto> UpdateClientGeofence(ClockClientGeofenceDto geofence)
        {
            var result = new OpResult<ClockClientGeofenceDto>();
            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.CanOptInToCompanyFeature).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to these geofences");

            var geofenceResult = _geoProvider.UpdateClientGeofence(geofence);
            geofenceResult.MergeInto(result);
            result.SetDataOnSuccess(geofenceResult.Data);

            return result;
        }

        public IOpResult<IEnumerable<RealTimePunchLocationDto>> GetGeofencePunches(GeofencePunchFilterDto filters)
        {
            var result = new OpResult<IEnumerable<RealTimePunchLocationDto>>();
            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.CanOptInToCompanyFeature).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to these geofences");

            _geoProvider.GetGeofencePunches(filters).MergeInto(result);

            return result;
        }

        public IOpResult<bool> ClientUsesGeofencing()
        {
            var result = new OpResult<bool>();
            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this client");

            result.SetDataOnSuccess(_geoProvider.ClientUsesGeofencing().Data);

            return result;
        }

        public IOpResult<bool> TimePolicyUsesGeofencing()
        {
            var result = new OpResult<bool>();
            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this client");

            result.SetDataOnSuccess(_geoProvider.TimePolicyUsesGeofencing().Data);

            return result;
        }

        public IOpResult<bool> UserRequiresGeofence()
        {
            var result = new OpResult<bool>();
            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this client");

            result.SetDataOnSuccess(_geoProvider.UserRequiresGeofence().Data);

            return result;
        }

        public IOpResult<GeofenceOptInDto> SetClientGeofenceSetting(GeofenceOptInDto optIn)
        {
            var result = new OpResult<GeofenceOptInDto>();
            var user = _session.LoggedInUserInformation;
            var userId = user.UserId;
            var tmpUserPin = _session.UnitOfWork.UserRepository
                .QueryUserPins()
                .ByUserId(userId)
                .ExecuteQueryAs(x => new UserPinDto {
                    UserId = x.UserId,
                    ClientId = x.ClientId,
                    Pin = x.Pin,
                }).FirstOrDefault();

            if (tmpUserPin == null && !user.IsSystemAdmin) return result.SetToFail("There is no user pin associated with this account");

            if (tmpUserPin?.Pin != optIn.UserPin) return result.SetToFail("The user pin does not match the one associated with this account");

            _session.CanPerformAction(ClockActionType.CanOptInToCompanyFeature).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to these geofences");

            _geoProvider.SetClientGeofenceSetting(optIn).MergeInto(result);

            if (result.HasError) return result;

            int clientId = _session.LoggedInUserInformation.ClientId.Value;

            var clientFeatureHistory = _session.UnitOfWork.ClientAccountFeatureRepository
                .ClientAccountFeatureChangeHistoryQuery()
                .ByAccountFeatureId(AccountFeatureEnum.Geofencing)
                .ByClientId(clientId)
                .ExecuteQuery().FirstOrDefault();

            var clientFeatureInfo = _accountFeatureService.AddClientAccountFeature(AccountFeatureEnum.Geofencing, clientId).MergeInto(result);

            var billingDate = clientFeatureHistory == null ? DateTime.Now.AddDays(30) : DateTime.Now;

            _billingProvider.PerformAutomaticBilling(clientId, (int)AccountFeatureEnum.Geofencing, clientFeatureInfo.Data.Description, true, billingDate).MergeInto(result);

            var dto = new BillingItemDto
            {
                BillingItemId = CommonConstants.NEW_ENTITY_ID,
                ClientId = clientId,
                BillingItemDescriptionId = BillingItemDescriptionType.ActivationFee,
                BillingPriceChartId = 0,
                BillingFrequency = BillingFrequency.EveryPayroll,
                Line = 0,
                Flat = 150,
                PerQty = 0,
                BillingWhatToCount = null,
                Comment = "Opted into Geofencing",
                IsOneTime = true,
                PayrollId = null,
                Modified = DateTime.Now,
                ModifiedBy = user.UserName,
                BillingPeriod = BillingPeriod.NextPayroll,
                IsStopDiscount = true,
                BillingYear = null,
                StartBilling = billingDate,
                FeatureOptionId = (int)AccountFeatureEnum.Geofencing,
            };

            _billingProvider.SaveBillingItem(dto).MergeInto(result);

            if (result.HasNoError) _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        public IOpResult<GeofenceOptInDto> GetClientGeofenceSetting()
        {
            var result = new OpResult<GeofenceOptInDto>();
            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.CanOptInToCompanyFeature).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to these geofences");

            _geoProvider.GetClientGeofenceSetting().MergeInto(result);

            return result;
        }

        public IOpResult<ClientFeaturesAgreementDto> GetClientAgreement(AccountFeatureEnum featureOptionId)
        {
            var result = new OpResult<ClientFeaturesAgreementDto>();

            // _geoProvider.GetClientAgreement(featureOptionId).MergeInto(result);
            result.Data = _geoProvider.GetClientAgreement(featureOptionId).Data;

            return result;
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetGeofencePunchesByPunchId(IEnumerable<int> punchIds, int employeeId)
        {
            var result = new OpResult<IEnumerable<ClockEmployeePunchDto>>();

            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this client");

            result.SetDataOnSuccess(_geoProvider.GetGeofencePunchesByPunchId(punchIds,employeeId).Data);

            return result;

        }

        #endregion
    }
}