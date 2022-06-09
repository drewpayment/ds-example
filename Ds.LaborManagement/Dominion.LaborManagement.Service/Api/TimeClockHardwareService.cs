
using Dominion.Core.Services.Interfaces;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Dto.Sprocs;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Domain.Entities.Employee;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.Utility.Security;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.TimeCard;
using Dominion.LaborManagement.Dto.TimeClockHardware;
using Dominion.Domain.Entities.TimeClock;

namespace Dominion.LaborManagement.Service.Api
{
    public class TimeClockHardwareService : ITimeClockHardwareService
    {
        private readonly ITimeCardAuthorizationProvider _timeCardAuthorizationProvider;
        private readonly IClientService _clientService;
        private readonly IDsDataServicesClockCalculateActivityService _clockCalculateActivityService;
        private readonly IDsDataServicesClockAutomatedPointsLogicService _ClockAutomatedPointsLogicService;
        private readonly IBusinessApiSession _session;
        private readonly IUserManager _userProvider;

        public TimeClockHardwareService(
            ITimeCardAuthorizationProvider timeCardAuthorizationProvider,
            IClientService clientService,
            IDsDataServicesClockCalculateActivityService clockCalculateActivityService,
            IDsDataServicesClockAutomatedPointsLogicService ClockAutomatedPointsLogicService,
            IBusinessApiSession session,
            IUserManager userProvider)
        {
            _timeCardAuthorizationProvider = timeCardAuthorizationProvider;
            _clientService = clientService;
            _clockCalculateActivityService = clockCalculateActivityService;
            _ClockAutomatedPointsLogicService = ClockAutomatedPointsLogicService;
            _session = session;
            _userProvider = userProvider;
        }

        IOpResult<IEnumerable<ClockClientHardwareDto>> ITimeClockHardwareService.GetClockClientHardwares(int clientId)
        {
            var result = new OpResult<IEnumerable<ClockClientHardwareDto>>();
            
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.Success)
            {
                result.TrySetData(_session.UnitOfWork.TimeClockRepository
                    .QueryClockClientHardware()
                    .ByClientId(clientId)
                    .ExecuteQueryAs(x => new ClockClientHardwareDto
                    {
                        ClockClientHardwareId = x.ClockClientHardwareId,
                        ClientId = x.ClientId,
                        Description = x.Description,
                        Model = x.Model,
                        Email = x.Email,
                        IPAddress = x.IPAddress,
                        ModifiedBy = x.ModifiedBy,
                        Modified = x.Modified,
                        Number = x.Number,
                        ClockClientHardwareFunctionId = x.ClockClientHardwareFunctionId,
                        SerialNumber = x.SerialNumber,
                        MACAddress = x.MACAddress,
                        FirmwareVersion = x.FirmwareVersion,
                        IsRental = x.IsRental,
                        PurchaseDate = x.PurchaseDate,
                        Warranty = x.Warranty,
                        WarrantyEnd = x.WarrantyEnd
                    }).ToList);
            }

            return result;
        }

        IOpResult<ClockClientHardwareDto> ITimeClockHardwareService.UpdateClockClientHardware(int clientId, ClockClientHardwareDto dto)
        {
            var opResult = new OpResult<ClockClientHardwareDto>();

            //_session.CanPerformAction(EmployeeManagerActionType.EmployeeDependentUpdate).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            if (dto.ClockClientHardwareId == 0)
            {
                var newClockClientHardware = new ClockClientHardware()
                {
                    ClockClientHardwareId = dto.ClockClientHardwareId,
                    ClientId = clientId,
                    Description = dto.Description,
                    Model = dto.Model,
                    Email = dto.Email,
                    IPAddress = dto.IPAddress,
                    ModifiedBy = _session.LoggedInUserInformation.UserId,
                    Modified = DateTime.Now,
                    Number = dto.Number,
                    ClockClientHardwareFunctionId = dto.ClockClientHardwareFunctionId,
                    SerialNumber = dto.SerialNumber,
                    MACAddress = dto.MACAddress,
                    FirmwareVersion = dto.FirmwareVersion,
                    IsRental = dto.IsRental ?? false,
                    PurchaseDate = dto.PurchaseDate,
                    Warranty = dto.Warranty,
                    WarrantyEnd = dto.WarrantyEnd
                };

                _session.UnitOfWork.RegisterNew(newClockClientHardware);
                _session.UnitOfWork.RegisterPostCommitAction(() => {
                    dto.ClockClientHardwareId = newClockClientHardware.ClockClientHardwareId;
                });
            }
            else
            {
                var editClockClientHardware = _session.UnitOfWork.TimeClockRepository
                                            .QueryClockClientHardware()
                                            .ByClockClientHardwareId(dto.ClockClientHardwareId)
                                            .FirstOrDefault();

                editClockClientHardware.ClockClientHardwareId = dto.ClockClientHardwareId;
                editClockClientHardware.ClientId = dto.ClientId;
                editClockClientHardware.Description = dto.Description;
                editClockClientHardware.Model = dto.Model;
                editClockClientHardware.Email = dto.Email;
                editClockClientHardware.IPAddress = dto.IPAddress;
                editClockClientHardware.ModifiedBy = _session.LoggedInUserInformation.UserId;
                editClockClientHardware.Modified = DateTime.Now;
                editClockClientHardware.Number = dto.Number;
                editClockClientHardware.ClockClientHardwareFunctionId = dto.ClockClientHardwareFunctionId;
                editClockClientHardware.SerialNumber = dto.SerialNumber;
                editClockClientHardware.MACAddress = dto.MACAddress;
                editClockClientHardware.FirmwareVersion = dto.FirmwareVersion;
                editClockClientHardware.IsRental = dto.IsRental ?? false;
                editClockClientHardware.PurchaseDate = dto.PurchaseDate;
                editClockClientHardware.Warranty = dto.Warranty;
                editClockClientHardware.WarrantyEnd = dto.WarrantyEnd;

                _session.UnitOfWork.RegisterModified(editClockClientHardware);
            }
            _session.UnitOfWork.Commit().MergeInto(opResult);
            opResult.SetDataOnSuccess(dto);
            return opResult;
        }

        IOpResult<ClockClientHardwareDto> ITimeClockHardwareService.DeleteClockClientHardware(int clockClientHardwareId)
        {
            var opResult = new OpResult<ClockClientHardwareDto>();

            //_session.CanPerformAction(EmployeeManagerActionType.EmployeeDependentUpdate).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            var dto =  _session.UnitOfWork.TimeClockRepository
                                .QueryClockClientHardware()
                                .ByClockClientHardwareId(clockClientHardwareId)
                                .ExecuteQueryAs(x => new ClockClientHardwareDto
                                {
                                    ClockClientHardwareId = x.ClockClientHardwareId,
                                    ClientId = x.ClientId,
                                    Description = x.Description,
                                    Model = x.Model,
                                    Email = x.Email,
                                    IPAddress = x.IPAddress,
                                    ModifiedBy = x.ModifiedBy,
                                    Modified = x.Modified,
                                    Number = x.Number,
                                    ClockClientHardwareFunctionId = x.ClockClientHardwareFunctionId,
                                    SerialNumber = x.SerialNumber,
                                    MACAddress = x.MACAddress,
                                    FirmwareVersion = x.FirmwareVersion,
                                    IsRental = x.IsRental,
                                    PurchaseDate = x.PurchaseDate,
                                    Warranty = x.Warranty,
                                    WarrantyEnd = x.WarrantyEnd
                                }).FirstOrDefault();

            var entity = _session.UnitOfWork.TimeClockRepository
                .QueryClockClientHardware()
                .ByClockClientHardwareId(clockClientHardwareId)
                .FirstOrDefault();

            _session.UnitOfWork.RegisterDeleted(entity);
            _session.UnitOfWork.Commit().MergeInto(opResult);
            opResult.SetDataOnSuccess(dto);
            return opResult;
        }

    }
}
