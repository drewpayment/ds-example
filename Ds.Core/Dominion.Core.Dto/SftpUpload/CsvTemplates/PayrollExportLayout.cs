using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates
{
    public class PayrollExportLayout
    {
        /// <summary>
        /// Employee Number - Employee number getting paid.
        /// </summary>
        [Description("Employee Number")]
        public string col00_employeeNumber { get; set; }

        /// <summary>
        /// Code - column is mainly informational will contain any earning, deduction codes, or identify adjustments & 2nd checks
        /// </summary>
        [Description("Code")]
        public string col01_code { get; set; }

        /// <summary>
        /// Description - What PWC will map to when they import the file.
        /// </summary>
        [Description("Description")]
        public string col02_description { get; set; }

        /// <summary>
        /// Amount - Current amount being shown on the employee's check or our payroll reports
        /// </summary>
        [Description("Amount")]
        public decimal? col03_amount { get; set; }

        /// <summary>
        /// Rate - Rate used to calculate the pay
        /// </summary>
        [Description("Rate")]
        public double? col04_rate { get; set; }

        /// <summary>
        /// Hours - the amount of hours the employee works.
        /// </summary>
        [Description("Hours")]
        public double? col05_hours { get; set; }
    }
}
