using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Reporting
{
    public partial class SavedReportField : Entity<SavedReportField>, IHasModifiedOptionalData
    {
        public virtual int SavedReportFieldId { get; set; }
        public virtual int SavedReportId { get; set; }
        public virtual SavedReport Report { get; set; }
        public virtual int ReportFieldId { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual bool IsSelect { get; set; }
        public virtual bool IsGroup { get; set; }
        public virtual bool IsWhere { get; set; }
        public virtual bool IsOrder { get; set; }
        public virtual bool IsCustom { get; set; }
        public virtual string WhereValue { get; set; }
        public virtual string WhereBooleanOperator { get; set; }
        public virtual string WhereComparisonOperator { get; set; }
        public virtual int? ListFieldId { get; set; }
        public virtual string ListFieldName { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }

        public SavedReportField()
        {
        }
    }
}