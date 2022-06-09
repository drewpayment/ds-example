using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Geofence;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Employee.ClockEmployeeInfo;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using Dominion.LaborManagement.Dto.Scheduling;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface IGeoProvider
    {
        IOpResult<ClockEmployeePunchLocation> ProcessRealTimePunchLocation(RealTimePunchLocation request);
        IOpResult<bool> CheckPunchLocation(RealTimePunchLocation request);
        IOpResult<IEnumerable<ClockClientGeofenceDto>> GetClientGeofences(int clientId);
        IOpResult<DateTime?> GetClientLastModified(int clientId);
        IOpResult<ClockClientGeofenceDto> ProcessClientGeofence(ClockClientGeofenceDto geofence);
        IOpResult<ClockClientGeofenceDto> UpdateClientGeofence(ClockClientGeofenceDto geofence);
        IOpResult<IEnumerable<RealTimePunchLocationDto>> GetGeofencePunches(GeofencePunchFilterDto filters);
        IOpResult<bool> ClientUsesGeofencing();
        IOpResult<bool> TimePolicyUsesGeofencing();
        IOpResult<bool> UserRequiresGeofence();
        IOpResult<GeofenceOptInDto> SetClientGeofenceSetting(GeofenceOptInDto optIn);
        IOpResult<GeofenceOptInDto> GetClientGeofenceSetting();
        IOpResult<ClientFeaturesAgreementDto> GetClientAgreement(AccountFeatureEnum featureOptionId);
        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetGeofencePunchesByPunchId(IEnumerable<int> punchIds, int employeeId);
    }
}