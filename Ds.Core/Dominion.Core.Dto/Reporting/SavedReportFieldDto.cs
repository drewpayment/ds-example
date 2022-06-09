using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public class SavedReportFieldDto
    {
        public  int SavedReportFieldId { get; set; }
        public  int SavedReportId { get; set; }
        public SavedReportDto Report { get; set; }
        public int ReportFieldId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsSelect { get; set; }
        public bool IsGroup { get; set; }
        public bool IsWhere { get; set; }
        public bool IsOrder { get; set; }
        public bool IsCustom { get; set; }
        public string WhereValue { get; set; }
        public string WhereBooleanOperator { get; set; }
        public string WhereComparisonOperator { get; set; }
        public int? ListFieldId { get; set; }
        public string ListFieldName { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
