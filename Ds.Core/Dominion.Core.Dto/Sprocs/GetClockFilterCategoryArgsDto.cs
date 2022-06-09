using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockFilterCategoryArgsDto : SprocParametersBase

    {

        private readonly SqlParameterBuilder<string>    _categoryString;  

        public string CategoryString
        {
            get { return _categoryString.Value; }
            set { _categoryString.Value = value; }
        }
        public GetClockFilterCategoryArgsDto()
        {
            _categoryString = AddParameter<string>("@CategoryString", SqlDbType.VarChar);
        }

    }
}
