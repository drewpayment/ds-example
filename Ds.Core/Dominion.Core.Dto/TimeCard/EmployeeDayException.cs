using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeDayException
    {
        /// <summary>
        ///         ''' Gets the description of the exception.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public string Description { get; set; }
        /// <summary>
        ///         ''' Gets or sets whether the exception is approvable.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool IsApprovable { get; set; }
        /// <summary>
        ///         ''' Gets the ID of the punch that this exception is for.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public int? ClockEmployeePunchID { get; set; }
        /// <summary>
        ///         ''' Gets the ID of the type of exception that this is for.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public int ClockExceptionID { get; set; }
        /// <summary>
        ///         ''' Gets the ID of the record that this exception object was built from.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public int ClockEmployeeExceptionHistoryID { get; set; }
        /// <summary>
        ///         ''' Gets the number of hours that this exception is "worth".
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public double? Hours { get; set; }

        public bool IsApproved { get; set; }
    }

}
