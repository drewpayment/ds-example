using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientHRInfo : Entity<ClientHRInfo>
    {
        public virtual int ClientHRInfoId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string FieldName { get; set; }
        public virtual byte FieldTypeId { get; set; }
        public virtual string OtherInfo { get; set; }
        public virtual int? Sequence { get; set; }
        public virtual bool? IsEmployeeReportVisible { get; set; }
        public virtual bool? IsEmployeeEditable { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual ICollection<EmployeeHRInfo> EmployeeHRInfos { get; set; }

    }
}
