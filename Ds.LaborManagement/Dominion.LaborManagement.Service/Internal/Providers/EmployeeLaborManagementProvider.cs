using Dominion.LaborManagement.Service.Internal.Security;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    using Dto.EmployeeLaborManagement;
    using Utility.Msg.Specific;
    using Utility.OpResult;
    using System.Linq;
    using Core.Services.Interfaces;
    using Dominion.Core.Dto.Misc;
    using Dominion.Core.Dto.Client;
    using System.Collections.Generic;
    using Dominion.Core.Dto.Labor;
    using Dominion.Domain.Entities.Labor;
    using Dominion.Utility.Containers;

    internal class EmployeeLaborManagementProvider : IEmployeeLaborManagementProvider
    {
        private readonly IBusinessApiSession _session;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session"></param>
        public EmployeeLaborManagementProvider(IBusinessApiSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Internal method for checking Badge Number and Employee Pin availability
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IOpResult<bool> CanAssignEmployeePinToEmployee(ClockEmployeeSetupDto dto)
        {
            var opResult  = new OpResult<bool>();
            var clientId  = _session.LoggedInUserInformation.ClientId.GetValueOrDefault();
            var pinLength = 4;

            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(opResult);

            if (opResult.HasError)
                return opResult;

            var employees = opResult.TryGetData(_session.UnitOfWork.TimeClockRepository
                .GetClockEmployeeQuery()
                .ByClientId(dto.ClientId, true)
                .ExecuteQueryAs(x => new ClockEmployeeSetupDto
                {
                    EmployeeId  = x.EmployeeId,
                    Pin         = x.EmployeePin,
                    BadgeNumber = x.BadgeNumber
                }).ToList);

            #region GetIpadPinLength
                var clientOption = _session.UnitOfWork.ClientAccountFeatureRepository
                .ClientOptionQuery()
                .ByClientId(clientId)
                .ByOption(AccountOption.TimeClock_IPadPinNumberLength)
                .ExecuteQueryAs(x => new ClientAccountOptionDto
                {
                    ClientAccountOptionId = x.ClientAccountOptionId,
                    Value                 = x.Value
                }).ToList();

                if (clientOption.Any())
                {
                    var clientItem = _session.UnitOfWork.ClientAccountFeatureRepository
                        .AccountOptionItemQuery()
                        .ByClientOptionItemId(System.Int32.Parse(clientOption[0].Value))
                        .ExecuteQueryAs(x => new Core.Dto.Misc.AccountOptionItemDto
                        {
                            AccountOptionItemId = x.AccountOptionItemId,
                            Value               = x.Value
                        }).ToList();

                    if (clientItem.Any())
                        pinLength = (int)clientItem[0].Value;
                    else
                        pinLength = 4;
                }
                else
                    pinLength = 4;
            #endregion

            if (opResult.HasError)
                return opResult;


            if (dto.Pin != null)
            {
                if (dto.Pin.Length > pinLength || !int.TryParse(dto.Pin, out var isNumeric))
                {
                    string gMsg = "Pin should only be numeric and" + pinLength + "characters long.";
                    opResult.AddMessage(new GenericMsg(gMsg)).SetToFail();
                    return opResult;
                }

                if (employees.Any(x => x.Pin == dto.Pin && x.EmployeeId != dto.EmployeeId))
                {
                    opResult.AddMessage(new GenericMsg("Employee PIN Number already in use.")).SetToFail();
                    return opResult;
                }
            }

            if (dto.BadgeNumber != null)
            {
                if (dto.BadgeNumber.Length > 15)
                {
                    opResult.AddMessage(
                        new GenericMsg("Badge number should be no longer than 15 characters long.")).SetToFail();
                    return opResult;
                }

                if (employees.Any(x => x.BadgeNumber == dto.BadgeNumber && x.EmployeeId != dto.EmployeeId))
                {
                    opResult.AddMessage(new GenericMsg("Badge Number already in use")).SetToFail();
                    return opResult;
                }
            }

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> UpdateGeofenceOnTimePolicy(IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> dto)
        {
            var result = new OpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>>();
            
            foreach(var timePolicy in dto)
            {
                var tmpTimePolicyEntity = new ClockClientTimePolicy
                {
                    GeofenceEnabled = timePolicy.GeofenceEnabled,
                    ClockClientTimePolicyId = timePolicy.ClockClientTimePolicyId,
                };

                var propList = new PropertyList<ClockClientTimePolicy>();

                propList.Add(x => x.GeofenceEnabled);

                _session.UnitOfWork.RegisterModified(tmpTimePolicyEntity, propList);
            }

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }
    }
}
