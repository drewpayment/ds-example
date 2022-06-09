namespace Dominion.Core.Dto.Labor
{
    public class TcaSearchOptions
    {
        public string ApprovalStatusDropdownSelectedValue { get; set; }
        public string DaysDropdownSelectedValue { get; set; }
        public string StartDateFieldText { get; set; }
        public string EndDateFieldText { get; set; }
        public int PayPeriodDropdownSelectedValue { get; set; }
        public string PayPeriodDropdownSelectedItemText { get; set; }
        public Dropdown Filter1Dropdown { get; set; }
        public Dropdown Category1Dropdown { get; set; }
        public Dropdown Filter2Dropdown { get; set; }
        public Dropdown Category2Dropdown { get; set; }
        public int clientId { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class Dropdown
    {
        public int Value { get; set; }
        public bool Visible { get; set; }
    }
}