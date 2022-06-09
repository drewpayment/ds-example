using System;
using System.Data;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class RequestTimeOffDetailsArgsDto : SprocParametersBase
    {
        private readonly  SqlParameterBuilder<int>       _requestTimeOffId;
        private readonly  SqlParameterBuilder<bool>      _oneTableOnly;
        private readonly  SqlParameterBuilder<bool>      _twoTablesOnly;


        public int RequestTimeOffId
        {
            get { return _requestTimeOffId.Value; } 
            set { _requestTimeOffId.Value = value; }
        }

        public bool OneTableOnly
        {
            get { return _oneTableOnly.Value; } 
            set { _oneTableOnly.Value = value; }
        }

        public bool TwoTablesOnly
        {
            get { return _twoTablesOnly.Value; } 
            set { _twoTablesOnly.Value = value; }
        }

        public RequestTimeOffDetailsArgsDto()
        {
            _requestTimeOffId   = AddParameter<int>("@RequestTimeOffID",    SqlDbType.Int);
            _oneTableOnly       = AddParameter<bool>("@ReturnOneTable",     SqlDbType.Bit);
            _twoTablesOnly      = AddParameter<bool>("@ReturnTwoTables",    SqlDbType.Bit);
        }

    }
}