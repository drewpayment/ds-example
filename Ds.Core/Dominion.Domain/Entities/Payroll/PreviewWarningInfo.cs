using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    public class PreviewWarningInfo : Entity<PreviewWarningInfo>
    {
        public virtual int PreviewWarningId { get; set; }
        public virtual int PayrollId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual string Description { get; set; }
        public virtual string LinkToFix { get; set; }
        public  virtual bool IsCritical { get; set; }
        public virtual int ClientId { get; set; }
        public virtual PreviewWarningTypeInfo PreviewWarningType { get; set; }
        public virtual PreviewWarningCategoryInfo PreviewWarningCategory { get; set; }

        public virtual Payroll Payroll { get; set; }
        public virtual Employee.Employee Employee { get; set; }
    }
}
