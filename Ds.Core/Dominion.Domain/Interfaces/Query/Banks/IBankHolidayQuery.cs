using Dominion.Domain.Entities.Banks;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Banks
{
    public interface IBankHolidayQuery : IQuery<BankHoliday, IBankHolidayQuery>
    {
        IBankHolidayQuery FutureBankHolidays();
        IBankHolidayQuery ByYear(int year);
        IBankHolidayQuery ByBankHolidayId(int bankHolidayId);
    }
}
