using Dominion.Core.Dto.Labor;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Validation
{
    internal interface ILaborManagementValidationProvider
    {
        /// <summary>
        /// Validates the specified note.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Note to validate</param>
        /// <returns></returns>
        IOpResult ValidateClockClientNote<T>(T obj) where T : IHasClockClientNoteValidation;

        IOpResult ValidateClockClientOvertime<T>(T obj) where T : IHasClockClientOvertimeValidation;

        IOpResult ValidateClockClientException<T>(T obj) where T : IHasClockClientExceptionValidation;

        IOpResult ValidateClockClientHoliday<T>(T obj) where T : IHasClockClientHolidayValidation;

        IOpResult ValidateClockClientHolidayDetail<T>(T obj) where T : IHasClockClientHolidayDetailValidation;
    }
}
