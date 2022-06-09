using System;
using System.Data;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.W2
{
    public class W2ProcessingDto
    {
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public bool LastPay { get; set; }
        public int Quantity { get; set; }
        public decimal Billed { get; set; }
        public decimal ToBill { get; set; }
        public int ClientId { get; set; }
        public bool HasOneTimeW2sReady { get; set; }
        public bool HasOneTime1099 { get; set; }
        public bool HasOneTimeDelivery { get; set; }
        public bool Create { get; set; }
        public bool W2sReady { get; set; }
        public bool IsScheduled { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool CreateManifest { get; set; }
        public string AnnualNotes { get; set; }
        public string MiscNotes { get; set; }
        public DateTime? Date1099 { get; set; }
        public bool ApprovedForClient { get; set; }
        public bool IsActive { get; set; }
        public bool IsCallIn { get; set; }
        public string EmployeesLastUpdatedOn { get; set; }
        public bool HasNotes => !string.IsNullOrWhiteSpace(AnnualNotes) || !string.IsNullOrWhiteSpace(MiscNotes);
        public bool IsApprovable { get; set; } = false;

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> _year;
            private readonly SqlParameterBuilder<int> _filterId;

            public int Year
            {
                get => _year.Value;
                set => _year.Value = value;
            }

            public int FilterId
            {
                get => _filterId.Value;
                set => _filterId.Value = value;
            }

            public Args()
            {
                _year = AddParameter<int>("@Year", SqlDbType.Int);
                _filterId = AddParameter<int>("@FilterID", SqlDbType.Int);
            }
        }
    }
}