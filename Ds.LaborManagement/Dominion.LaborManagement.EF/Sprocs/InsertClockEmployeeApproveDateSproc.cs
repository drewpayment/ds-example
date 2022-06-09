using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Sprocs;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Sprocs
{
    public class InsertClockEmployeeApproveDateSproc : SprocBaseNonQuery<InsertClockEmployeeApproveDateArgsDto>
    {
        #region Constructors And Initializers
        public InsertClockEmployeeApproveDateSproc(string connStr, InsertClockEmployeeApproveDateArgsDto args)
            : base(connStr, args)
        {
        }
        #endregion

        #region Methods

        public override int Execute()
        {
            return ExecuteSprocNonQuery("[dbo].[spInsertClockEmployeeApproveDate]");
        }

        #endregion

    }
}
