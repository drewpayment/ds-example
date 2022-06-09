using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetReportClockEmployeeOnSiteArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _ClientID;
        //Need to change this for employee List 
        private readonly SqlParameterBuilder<int> _UserID;
        private readonly SqlParameterBuilder<DateTime> _StartDate;
        private readonly SqlParameterBuilder<DateTime> _EndDate;
        private readonly SqlParameterBuilder<int> _FilterCategory;
        private readonly SqlParameterBuilder<int> _Filter;
        private readonly SqlParameterBuilder<int> _PayType;

        public int ClientID
        {
            get { return _ClientID.Value; }
            set { _ClientID.Value = value; }
        }
        //Fix Me for list
        public int UserID
        {
            get { return _UserID.Value; }
            set { _UserID.Value = value; }
        }
        public DateTime StartDate
        {
            get { return _StartDate.Value; }
            set { _StartDate.Value = value; }
        }
        public DateTime EndDate
        {
            get { return _EndDate.Value; }
            set { _EndDate.Value = value; }
        }
        public int FilterCategory
        {
            get { return _FilterCategory.Value; }
            set { _FilterCategory.Value = value; }
        }
        public int Filter
        {
            get { return _Filter.Value; }
            set { _Filter.Value = value; }
        }
        public int PayType
        {
            get { return _PayType.Value; }
            set { _PayType.Value = value; }
        }


        public GetReportClockEmployeeOnSiteArgsDto()
        {
            _ClientID = AddParameter<int>("@ClientID", SqlDbType.Int);
            //fix me for list
            _UserID = AddParameter<int>("@UserID", SqlDbType.Int);
            _StartDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime);
            _EndDate = AddParameter<DateTime>("@EndDate", SqlDbType.DateTime);
            _FilterCategory = AddParameter<int>("@FilterCategory", SqlDbType.Int);
            _Filter = AddParameter<int>("@Filter", SqlDbType.Int);
            _PayType = AddParameter<int>("@PayType", SqlDbType.Int);
        }
    }
}
