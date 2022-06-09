using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public enum ApprovalOptionType
    {
        /// <summary>
        ///     ''' Defines that no days need to be approved,
        ///     ''' and therefore no approval checkboxes should show up.
        ///     ''' </summary>
        None = 1,
        /// <summary>
        ///     ''' Defines that days with hours or benefits need to be approved.
        ///     ''' </summary>
        Hours_And_Benefits = 2,
        /// <summary>
        ///     ''' Defines that only days with exceptions need to be approved. 
        ///     ''' </summary>
        Exceptions = 3,
        /// <summary>
        ///     ''' Defines that every day needs to be approved.
        ///     ''' </summary>
        Everyday = 4,
        /// <summary>
        ///     ''' Defines that all days with "activity" need to be approved.
        ///     ''' This includes days with punches, benefits, or exceptions.
        ///     ''' </summary>
        All_Activity = 5,
        /// <summary>
        ///     ''' Defines that all cost centers for a single day need to be approved individually.
        ///     ''' </summary>
        Cost_Center = 6
    }

}
