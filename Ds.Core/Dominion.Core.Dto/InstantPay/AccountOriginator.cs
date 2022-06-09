namespace Dominion.Core.Dto.InstantPay
{
    // Reference from StackOverflow - Associating enums with strings in C#:
    // https://stackoverflow.com/questions/630803/associating-enums-with-strings-in-c-sharp
    public class AccountOriginator
    {
        private AccountOriginator(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public static AccountOriginator Personal => new AccountOriginator("Personal");
        public static AccountOriginator DailyPay => new AccountOriginator("DailyPay");
        public static AccountOriginator Branch => new AccountOriginator("Branch");
    }
}
