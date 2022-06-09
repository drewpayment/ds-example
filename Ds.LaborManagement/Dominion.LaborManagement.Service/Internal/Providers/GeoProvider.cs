using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Geofence;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ClientFeatures;
using Dominion.Domain.Entities.Employee.ClockEmployeeInfo;
using Dominion.Utility.Constants;
using Dominion.Utility.Containers;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Dto.User;
using static Dominion.Core.Dto.Labor.ClockClientTimePolicyDtos;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Scheduling;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public class GeoProvider : IGeoProvider
    {
        private readonly IBusinessApiSession _session;

        internal IGeoProvider Self { get; set; }

        public GeoProvider(IBusinessApiSession session)
        {
            Self = this;

            _session = session;
        }

        public IOpResult<ClockEmployeePunchLocation> ProcessRealTimePunchLocation(RealTimePunchLocation request)
        {
            var opResult = new OpResult<ClockEmployeePunchLocation>();

            if (request == null)
                return opResult;

            var loc = new ClockEmployeePunchLocation
            {
                ClockEmployeePunchLocationID = CommonConstants.NEW_ENTITY_ID,
                Accuracy = request.Accuracy,
                Altitude = request.Altitude,
                AltitudeAccuracy = request.AltitudeAccuracy,
                Floor = request.Floor,
                Heading = request.Heading,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Speed = request.Speed,
            };

            _session.UnitOfWork.RegisterNew(loc);
            _session.UnitOfWork.RegisterPostCommitAction(() => request.PunchLocationID = loc.ClockEmployeePunchLocationID);

            _session.UnitOfWork.Commit().MergeInto(opResult);

            return opResult;
        }

        public IOpResult<bool> CheckPunchLocation(RealTimePunchLocation request)
        {
            var result = new OpResult<bool>(false);
            var geoLocations = GetClientGeofenceList(_session.LoggedInUserInformation.ClientId.GetValueOrDefault(), false);

            if (geoLocations.Data == null || !geoLocations.Data.Any())
                return result;

            if (request == null) return result;

            for (var i = geoLocations.Data.Count() - 1; i >= 0; i--)
            {
                if (CheckInside(request, geoLocations.Data.ElementAt(i)) == true)
                    return new OpResult<bool>(true);
            }

            return result;
        }

        public IOpResult<ClockClientGeofenceDto> ProcessClientGeofence(ClockClientGeofenceDto request)
        {
            var opResult = new OpResult<ClockClientGeofenceDto>(request);

            if (request == null)
                return opResult;

            var geofence = new ClockClientGeofence
            {
                ClockClientGeofenceID = CommonConstants.NEW_ENTITY_ID,
                ClientID = _session.LoggedInUserInformation.ClientId.GetValueOrDefault(),
                Lat = request.Lat,
                Lng = request.Lng,
                Name = request.Name,
                Address = request.Address,
                Radius = request.Radius
            };

            _session.SetModifiedProperties(geofence);
            _session.UnitOfWork.RegisterNew(geofence);
            _session.UnitOfWork.RegisterPostCommitAction(() => opResult.Data.ClockClientGeofenceID = geofence.ClockClientGeofenceID);
            _session.UnitOfWork.Commit().MergeInto(opResult);

            return opResult;
        }

        public IOpResult<ClockClientGeofenceDto> UpdateClientGeofence(ClockClientGeofenceDto request)
        {
            var opResult = new OpResult<ClockClientGeofenceDto>(request);

            var requestEntity = new ClockClientGeofence
            {
                ClockClientGeofenceID = request.ClockClientGeofenceID,
                ClientID = request.ClientID,
                IsArchived = request.IsArchived,
                Lat = request.Lat,
                Lng = request.Lng,
                Name = request.Name,
                Address = request.Address,
                Radius = request.Radius
            };

            // REGISTER MODIFIED ENTITY

            var oldClockClientGeofence = _session.UnitOfWork.GeofenceRepository.QueryGeofences()
                .ByClockClientGeofenceID(request.ClockClientGeofenceID)
                .ByClientID(_session.LoggedInUserInformation.ClientId.GetValueOrDefault(), true)
                .ExecuteQueryAs(x => new ClockClientGeofenceDto
                {
                    Radius = x.Radius,
                    ClientID = x.ClientID,
                    ClockClientGeofenceID = x.ClockClientGeofenceID,
                    IsArchived = x.IsArchived,
                    Lat = x.Lat,
                    Lng = x.Lng,
                    Name = x.Name,
                    Address = x.Address
                }).FirstOrDefault();

            var propList = new PropertyList<ClockClientGeofence>();


            if (oldClockClientGeofence.Radius != requestEntity.Radius) propList.Add(x => x.Radius);

            if (oldClockClientGeofence.IsArchived != requestEntity.IsArchived) propList.Add(x => x.IsArchived);

            if (oldClockClientGeofence.Lng != requestEntity.Lng) propList.Add(x => x.Lng);

            if (oldClockClientGeofence.Lat != requestEntity.Lat) propList.Add(x => x.Lat);

            if (oldClockClientGeofence.Name != requestEntity.Name) propList.Add(x => x.Name);

            if (oldClockClientGeofence.Address != requestEntity.Address) propList.Add(x => x.Address);

            if (propList.Any())
            {
                propList.Add(x => x.Modified);
                propList.Add(x => x.ModifiedBy);

                _session.SetModifiedProperties(requestEntity);
                _session.UnitOfWork.RegisterModified(requestEntity, propList);
            }

            _session.UnitOfWork.Commit().MergeInto(opResult);

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientGeofenceDto>> GetClientGeofences(int clientId)
        {
            return GetClientGeofenceList(clientId, false);
        }

        public IOpResult<DateTime?> GetClientLastModified(int clientId)
        {
            var result = new OpResult<DateTime?>();
            var geofenceList = GetClientGeofenceList(clientId, true).MergeInto(result).Data;
            var lastModified = new DateTime?();

            if (result.HasError) return result;
            if (geofenceList == null) return result.SetToFail("Could not find geofence list.");

            foreach (var geofence in geofenceList.ToList())
            {
                if (lastModified == null)
                {
                    lastModified = geofence.Modified;
                }
                else if (geofence.Modified != null && geofence.Modified > lastModified)
                {
                    lastModified = geofence.Modified;
                }
            }

            result.Data = lastModified;

            return result;
        }

        public IOpResult<IEnumerable<RealTimePunchLocationDto>> GetGeofencePunches(GeofencePunchFilterDto filters)
        {
            var result = new OpResult<IEnumerable<RealTimePunchLocationDto>>();
            var punches = new OpResult<IEnumerable<ClockEmployeePunchAttemptDto>>();
            var punchAttempts = new OpResult<IEnumerable<ClockEmployeePunchAttemptDto>>();

            punches.Data = _session.UnitOfWork.TimeClockRepository
                .GetClockEmployeePunchQuery()
                .ByClientId(filters.ClientID)
                .ByDates(filters.StartDate, filters.EndDate)
                .ByNonNullPunchLocations()
                .ExecuteQueryAs(x => new ClockEmployeePunchAttemptDto
                {
                    ClockEmployeePunchLocationId = x.ClockEmployeePunchLocation.ClockEmployeePunchLocationID,
                    EmployeeId = x.EmployeeId,
                    ClientId = x.ClientId,

                }).ToList();

            punchAttempts.Data = _session.UnitOfWork.GeofenceRepository
                .QueryPunchAttempts()
                .ByClient(filters.ClientID)
                .ByDates(filters.StartDate, filters.EndDate)
                .ByNonNullPunchLocations()
                .ExecuteQueryAs(x => new ClockEmployeePunchAttemptDto
                {
                    ClientId = x.ClientID,
                    EmployeeId = x.EmployeeID,
                    ClockEmployeePunchLocationId = x.ClockEmployeePunchLocationID,

                }).ToList();

            punchAttempts.CombineAll(punches);

            var punchLocations = new List<int>();

            for (var i = 0; i < punchAttempts.Data.Count() - 1; i++)
            {
                punchLocations.Add(punchAttempts.Data.ElementAt(i).ClockEmployeePunchLocationId.GetValueOrDefault());
            }

            result.Data = _session.UnitOfWork.GeofenceRepository
                .QueryPunchLocations()
                .ByClockEmployeePunchLocationIDs(punchLocations)
                .ExecuteQueryAs(x => new RealTimePunchLocationDto
                {
                    Accuracy = x.Accuracy,
                    Altitude = x.Altitude,
                    AltitudeAccuracy = x.AltitudeAccuracy,
                    Floor = x.Floor,
                    Heading = x.Heading,
                    IsValid = punches.Data.Any(p => p.ClockEmployeePunchLocationId == x.ClockEmployeePunchLocationID),
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    PunchLocationID = x.ClockEmployeePunchLocationID,
                    Speed = x.Speed,
                }).ToList();

            return result;
        }

        public IOpResult<bool> ClientUsesGeofencing()
        {
            var result = new OpResult<bool>();

            return result.TrySetData(() => _session.UnitOfWork.ClientAccountFeatureRepository
                .ClientAccountFeatureQuery()
                .ByAccountFeatureId(AccountFeatureEnum.Geofencing)
                .ByClientId(_session.LoggedInUserInformation.ClientId.GetValueOrDefault())
                .ExecuteQueryAs(f => new ClientAccountFeatureDto
                {
                    ClientId = f.ClientId
                })
                .ToOrNewList()
                .Any());
        }

        public IOpResult<bool> TimePolicyUsesGeofencing()
        {
            var result = new OpResult<bool>(false);

            var timePolicy = _session.UnitOfWork.TimeClockRepository
                .GetClockEmployeeQuery()
                .ByEmployeeId(_session.LoggedInUserInformation.EmployeeId.GetValueOrDefault())
                .ExecuteQueryAs(ce => new ClockEmployeeDto
                {
                    ClockClientTimePolicyId = ce.ClockClientTimePolicyId
                })
                .FirstOrDefault();

            if (timePolicy?.ClockClientTimePolicyId != null)
            {
                var clockClient = _session.UnitOfWork.TimeClockRepository
                      .GetClockClientTimePolicyQuery()
                      .ByClockClientTimePolicyId(timePolicy.ClockClientTimePolicyId.Value)
                      .ByClientId(_session.LoggedInUserInformation.ClientId.GetValueOrDefault())
                      .ExecuteQueryAs(x => new ClockClientTimePolicyDto
                      {
                          GeofenceEnabled = x.GeofenceEnabled
                      }).FirstOrDefault();

                if (clockClient != null)
                    result.TrySetData(() => clockClient.GeofenceEnabled);
            }

            return result;
        }

        public IOpResult<bool> UserRequiresGeofence()
        {
            var result = new OpResult<bool>();

            var clockEmployee = _session.UnitOfWork.TimeClockRepository
                .GetClockEmployeeQuery()
                .ByEmployeeId(_session.LoggedInUserInformation.EmployeeId.GetValueOrDefault())
                .ExecuteQueryAs(ce => new ClockEmployeeDto
                {
                    GeofenceEnabled = ce.GeofenceEnabled
                }).FirstOrDefault();

            result.Data = clockEmployee?.GeofenceEnabled ?? false;

            return result;
        }

        public IOpResult<GeofenceOptInDto> SetClientGeofenceSetting(GeofenceOptInDto optIn)
        {
            // This is never being set to anything and we are just returning NULL all of the time? - Drew
            var result = new OpResult<GeofenceOptInDto>();

            // If we are exiting early, shouldn't we be returning an error? - Drew
            if (optIn == null) return result;

            // HERE WE NEED TO SAVE THAT THE CLIENT HAS OPTED INTO GEOFENCING
            var agreement = new ClientFeaturesAgreement
            {
                Agreed = true,
                ClientFeaturesAgreementID = CommonConstants.NEW_ENTITY_ID,
                ClientID = _session.LoggedInUserInformation.ClientId.GetValueOrDefault(),
                FirstName = optIn.FirstName,
                LastName = optIn.LastName,
                UserID = _session.LoggedInUserInformation.UserId,
                FeatureOptionID = AccountFeatureEnum.Geofencing,
            };

            _session.SetModifiedProperties(agreement);
            _session.UnitOfWork.RegisterNew(agreement);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        public IOpResult<ClientFeaturesAgreementDto> GetClientAgreement(AccountFeatureEnum featureOptionId)
        {
            var result = new OpResult<ClientFeaturesAgreementDto>()
                .TrySetData(() => _session.UnitOfWork.ClientFeaturesRepository
                    .QueryClientFeaturesAgreement()
                    .ByClientId(_session.LoggedInUserInformation.ClientId.GetValueOrDefault())
                    .ByFeatureId(featureOptionId)
                    .ExecuteQueryAs(x => new ClientFeaturesAgreementDto
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Agreed = x.Agreed,
                        ClientFeaturesAgreementID = x.ClientFeaturesAgreementID,
                        ClientID = x.ClientID,
                        FeatureOptionID = x.FeatureOptionID,
                        UserID = x.UserID
                    }).FirstOrDefault());

            if (result.HasNoErrorAndHasData)
            {
                var tmpUser = _session.UnitOfWork.UserRepository
                    .QueryUsers()
                    .ByUserId(result.Data.UserID)
                    .ByUserTypeId(UserType.SystemAdmin)
                    .ExecuteQueryAs(x => new
                    {
                        FirstName = "System",
                        LastName = "Admin"
                    })
                    .FirstOrDefault();

                if (tmpUser == null) return result;

                result.Data.FirstName = tmpUser.FirstName;
                result.Data.LastName = tmpUser.LastName;
            }

            return result;
        }

        public IOpResult<GeofenceOptInDto> GetClientGeofenceSetting()
        {
            var result = new OpResult<GeofenceOptInDto>();

            // GET THE CLIENTS SELECTED GEOFENCE OPTION

            return result;
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetGeofencePunchesByPunchId(IEnumerable<int> punchIds, int employeeId)
        {
            var result = new OpResult<IEnumerable<ClockEmployeePunchDto>>();
            //var punches = new int[] punchIds.ToArray();
            var user = _session.LoggedInUserInformation;

            result.Data = _session.UnitOfWork.TimeClockRepository
                .GetClockEmployeePunchQuery()
                .ByEmployeeId(employeeId)
                .ByClockEmployeePunchIdList(punchIds)
                .ExecuteQueryAs(x => new ClockEmployeePunchDto
                {
                    ClientId = x.ClientId,
                    ClockEmployeePunchId = x.ClockEmployeePunchId,
                    ClockEmployeePunchLocationId = x.ClockEmployeePunchLocationID,
                    EmployeeId = x.EmployeeId
                }).ToOrNewList();

            return result;
        }

        private IOpResult<IEnumerable<ClockClientGeofenceDto>> GetClientGeofenceList(int clientId, bool showArchived = false)
        {
            var result = new OpResult<IEnumerable<ClockClientGeofenceDto>>();

            result.Data = _session.UnitOfWork.GeofenceRepository
                .QueryGeofences()
                .ByClientID(clientId, showArchived)
                .ExecuteQueryAs(x => new ClockClientGeofenceDto
                {
                    ClientID = x.ClientID,
                    ClockClientGeofenceID = x.ClockClientGeofenceID,
                    IsArchived = x.IsArchived,
                    Lat = x.Lat,
                    Lng = x.Lng,
                    Modified = x.Modified,
                    ModifiedBy = x.ModifiedBy,
                    Name = x.Name,
                    Address = x.Address,
                    Radius = x.Radius,
                }).ToList();

            return result;
        }

        private IOpResult<IEnumerable<ClockClientGeofenceDto>> GetClientGeofence(int geofenceId)
        {
            var result = new OpResult<IEnumerable<ClockClientGeofenceDto>>();

            result.Data = _session.UnitOfWork.GeofenceRepository
                .QueryGeofences()
                .ByClockClientGeofenceID(geofenceId)
                .ExecuteQueryAs(x => new ClockClientGeofenceDto
                {
                    ClientID = x.ClientID,
                    ClockClientGeofenceID = x.ClockClientGeofenceID,
                    IsArchived = x.IsArchived,
                    Lat = x.Lat,
                    Lng = x.Lng,
                    Modified = x.Modified,
                    ModifiedBy = x.ModifiedBy,
                    Name = x.Name,
                    Address = x.Address,
                    Radius = x.Radius,
                }).ToList();

            return result;
        }

        private bool CheckInside(RealTimePunchLocation request, ClockClientGeofenceDto geoLocation)
        {
            // DOING CALCULATIONS FOR KM TO FIND DISTANCE BETWEEN PUNCH LAT/LNG AND GEOFENCE LAT/LONG
            double Rad = 6378137;
            double dLat = this.toRadian(((double)geoLocation.Lat - (double)request.Latitude));
            double dLng = this.toRadian(((double)geoLocation.Lng - (double)request.Longitude));
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.toRadian((double)request.Latitude)) * Math.Cos(this.toRadian((double)geoLocation.Lat)) *
                Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
            double circ = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double dist = Rad * circ;

            // CHECK TO MAKE SURE THE PUNCH IS WITHIN THE SELECTED RADIUS GIVEN THE ACCURACY OF THE LOCATION SENT FROM PUNCH
            //if (dist <= ((double)geoLocation.Radius + (double)request.Accuracy)) return true;

            //WHAT TO DO WHEN YOU DON'T CARE ABOUT ACCURACY
            if (dist <= ((double)geoLocation.Radius)) return true;

            return false;
        }

        private double toRadian(double degree)
        {
            return (Math.PI / 180) * degree;
        }
    }
}