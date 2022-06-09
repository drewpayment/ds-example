using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Core.Dto.InterfaceFiles.PositivePay
{
    /// <summary>
    /// File Name: AGA2.CSV
    /// File Format Type: Huntington AGA2
    ///
    /// The file needs to be in CSV format
    /// No header row is needed
    /// Column A - client account number taken from the Company> Payroll> Bank Information Page
    /// Column B - Date, this is the payroll check date MMDDYY format
    /// Column C - Check Number
    /// Column D - Amount of the check
    /// Column E - Payee's name 
    /// Column F - Code, this is a text code required by the bank and it always need to be IS
    /// </summary>
    public class HuntingtonAGA2Layout
    {
        public static readonly string Delimiter = ",";
        public static readonly string DelimiterReplacement = " ";

        private string _PayeeFirstName;
        private string _PayeeLastName;

        /// <summary>
        /// Payee's firstname.
        /// </summary>
        public string PayeeFirstName
        {
            get => _PayeeFirstName?.Trim() ?? string.Empty;
            set => _PayeeFirstName = value;
        }

        /// <summary>
        /// Payee's lastname.
        /// </summary>
        public string PayeeLastName
        {
            get => _PayeeLastName?.Trim() ?? string.Empty;
            set => _PayeeLastName = value;
        }

        private string _PayeeFullName => PayeeFirstName + (string.IsNullOrEmpty(PayeeLastName) ? string.Empty : $" {PayeeLastName}");

        private string ReplaceDelimiter(string value) => value.Replace(Delimiter, DelimiterReplacement);



        /// <summary>
        /// Client account number, taken from the Company> Payroll> Bank Information Page.
        /// </summary>
        public string ClientAchBankAccountNumber { get; set; }

        /// <summary>
        /// Date, this is the payroll check date MMDDYY format
        /// </summary>
        public DateTime PayrollCheckDate { get; set; }

        /// <summary>
        /// Check Number
        /// </summary>
        public string CheckNumber { get; set; }

        /// <summary>
        /// Amount of the check
        /// </summary>
        public decimal CheckAmount { get; set; }

        /// <summary>
        /// Payee's name.
        /// Formatted as <c>"firstname lastname"</c>. If lastname is empty, then just <c>"firstname"</c>.
        /// </summary>
        public string PayeeName => ReplaceDelimiter(_PayeeFullName);
        
        /// <summary>
        /// Code, this is a text code required by the bank and it always need to be IS
        /// </summary>
        public string BankRequiredCode { get; set; }
    }
}
