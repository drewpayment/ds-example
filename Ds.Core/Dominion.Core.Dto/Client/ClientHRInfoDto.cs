using Dominion.Core.Dto.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientHRInfoDto
    {
        public int ClientHRInfoId { get; set; }
        public int ClientId { get; set; }
        public string FieldName { get; set; }
        public byte FieldTypeId { get; set; }
        public string OtherInfo { get; set; }
        public short? Sequence { get; set; }
        public bool? IsEmployeeReportVisible { get; set; }
        public bool? IsEmployeeEditable { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public List<string> OptionsList => OtherInfo.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

        public List<EmployeeHRInfoDto> EmployeeHRInfos { get; set; }
    }
}
