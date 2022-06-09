using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates
{
    public class OnShiftDemographicLayout
    {
        public string employeeNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string jobTitle { get; set; }
        /// <summary>
        /// Same as JobTitle...
        /// Is an attempt to fix an issue with csvHelper
        /// see https://github.com/JoshClose/CsvHelper/issues/515
        /// </summary>
        public string jobTitle2 { get; set; }
        public DateTime? hireDate { get; set; }
        public DateTime? terminationDate { get; set; }
        public string facilityId { get; set; }
        public string jobCode { get; set; }
        public string emailAddress { get; set; }
        public string mobilePhone { get; set; }
        public string homePhone { get; set; }
    }
}
