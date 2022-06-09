using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Validation
{
    public class LaborManagementValidationProvider : ILaborManagementValidationProvider
    {
        /// <summary>
        /// Validates the specified note.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Note to validate</param>
        /// <returns></returns>
        IOpResult ILaborManagementValidationProvider.ValidateClockClientNote<T>(T obj)
        {
            return new ClockClientNoteValidator().Verify(obj);
        }

        IOpResult ILaborManagementValidationProvider.ValidateClockClientOvertime<T>(T obj)
        {
            return new ClockClientOvertimeValidator().Verify(obj);
        }

        IOpResult ILaborManagementValidationProvider.ValidateClockClientException<T>(T obj)
        {
            return new ClockClientExceptionValidator().Verify(obj);
        }

        IOpResult ILaborManagementValidationProvider.ValidateClockClientHoliday<T>(T obj)
        {
            return new ClockClientHolidayValidator().Verify(obj);
        }

        IOpResult ILaborManagementValidationProvider.ValidateClockClientHolidayDetail<T>(T obj)
        {
            return new ClockClientHolidayDetailValidator().Verify(obj);
        }
    }
}