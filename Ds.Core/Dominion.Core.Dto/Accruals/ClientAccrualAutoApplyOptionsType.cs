using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals
{
    public enum AutoApplyAccrualPolicyType
    {
        NoAutoApply = 0,
        SelectAllQualifyingEmployees = 1,
        UnselectAllEmployees = 2,

        /**
        <asp:ListItem Selected="True" Text="" Value="0"></asp:ListItem>
        <asp:ListItem Text="Select All Qualifying Employees" Value="1"></asp:ListItem>
        <asp:ListItem Text="Unselect All Employees" Value="2"></asp:ListItem>
        */

    }
}
