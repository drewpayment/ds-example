using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockEmployeePunchListByDateAndFilterArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _clientID;
        private readonly SqlParameterBuilder<int> _intUserID;
        private readonly SqlParameterBuilder<int> _intEmployeeID;
        private readonly SqlParameterBuilder<string> _strStartDate;
        private readonly SqlParameterBuilder<string> _strEndDate;
        private readonly SqlParameterBuilder<int> _intFilter;
        private readonly SqlParameterBuilder<int> _intFilterCategory;
        private readonly SqlParameterBuilder<int> _intSpecialOption;
        private readonly SqlParameterBuilder<int> _intFilter2;
        private readonly SqlParameterBuilder<int> _intFilterCategory2;

        public int intClientID
        {
            get { return _clientID.Value; }
            set { _clientID.Value = value; }
        }
        public int intUserID
        {
            get { return _intUserID.Value; }
            set { _intUserID.Value = value; }
        }

        public int intEmployeeID
        {
            get { return _intEmployeeID.Value;  }
            set { _intEmployeeID.Value = value; }
        }

        public string strStartDate
        {
            get { return _strStartDate.Value; }
            set { _strStartDate.Value = value; }
        }

        public string strEndDate
        {
            get { return _strEndDate.Value; }
            set { _strEndDate.Value = value; }
        }

        public int intFilter
        {
            get { return _intFilter.Value; }
            set { _intFilter.Value = value; }
        }

        public int intFilterCategory
        {
            get { return _intFilterCategory.Value; }
            set { _intFilterCategory.Value = value; }
        }

        public int intSpecialOption
        {
            get { return _intSpecialOption.Value; }
            set { _intSpecialOption.Value = value; }
        }

        public int intFilter2
        {
            get { return _intFilter2.Value; }
            set { _intFilter2.Value = value; }
        }

        public int intFilterCategory2
        {
            get { return _intFilterCategory2.Value; }
            set { _intFilterCategory2.Value = value; }
        }

        public GetClockEmployeePunchListByDateAndFilterArgsDto()
        {
            _clientID = AddParameter<int>("@ClientID", SqlDbType.Int);
            _intUserID = AddParameter<int>("@UserID", SqlDbType.Int);
            _intEmployeeID = AddParameter<int>("@EmployeeID", SqlDbType.Int);
            _strStartDate = AddParameter<string>("@StartDate", SqlDbType.DateTime);
            _strEndDate = AddParameter<string>("@EndDate", SqlDbType.DateTime);
            _intFilter = AddParameter<int>("@Filter", SqlDbType.Int);
            _intFilterCategory = AddParameter<int>("@FilterCategory", SqlDbType.Int);
            _intFilter2 = AddParameter<int>("@Filter2", SqlDbType.Int);
            _intFilterCategory2 = AddParameter<int>("@FilterCategory2", SqlDbType.Int);
            _intSpecialOption = AddParameter<int>("@SpecialOption", SqlDbType.Int);
        }
    }
}
