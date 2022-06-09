using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    /// <summary>
    /// Defines a list of punch option types that an employee may have.
    /// </summary>
    public enum PunchOptionType
    {
        /// <summary>
        ///         ''' Defines that no punch option type has been selected for the user.
        ///         ''' Usually an invalid state.
        ///         ''' </summary>
        NoValueSelected = 0,
        /// <summary>
        ///         ''' Defines that the employee is not allowed to punch from the website, and only through an external device such as a
        ///         ''' punch clock terminal.
        ///         ''' </summary>
        None = 3,
        /// <summary>
        ///         ''' Defines that the employee is allowed to punch from the website or through a clock terminal.
        ///         ''' </summary>
        NormalPunch = 1,
        /// <summary>
        ///         ''' Defines that the employee is allowed to enter in the number of hours they worked directly.
        ///         ''' </summary>
        InputHours = 2
    }

}
