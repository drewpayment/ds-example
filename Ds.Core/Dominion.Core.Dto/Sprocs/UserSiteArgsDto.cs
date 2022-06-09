using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class UserSiteArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<string> _userName;
        private readonly SqlParameterBuilder<string> _companyCode;


        /// <summary>
        /// OPTIONAL (NULLABLE): This is the leave management policy. Example: PTO, Sick, Volunteer, etc.
        /// </summary>
        public string UserName
        {
            get { return _userName.Value; }
            set { _userName.Value = value; }
        }

        /// <summary>
        /// OPTIONAL (NULLABLE): The status of the leave request.
        /// </summary>
        public string CompanyCode
        {
            get { return _companyCode.Value; }
            set { _companyCode.Value = value; }
        }

        
        public UserSiteArgsDto()
        {
            _userName = AddParameter<string>("@UserName", SqlDbType.VarChar);
            _companyCode = AddParameter<string>("@CompanyCode", SqlDbType.VarChar);

        }
    }
}

