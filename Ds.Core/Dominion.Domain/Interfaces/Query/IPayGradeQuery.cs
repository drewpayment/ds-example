using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides the ability to perform various tasks with PayGrade related objects.
    /// </summary>
    public interface IPayGradeQuery : IQuery<PayGrade, IPayGradeQuery>
    {
        IPayGradeQuery ByName(string name);
        IPayGradeQuery ByCode(string Code);
        IPayGradeQuery ByCodeNotNullOrEmpty(string Code);
        IPayGradeQuery ByMinimum(decimal Minimum);
        IPayGradeQuery ByMiddle(decimal Middle);
        IPayGradeQuery ByMaximum(decimal Maximum);
        IPayGradeQuery ByType(PayGradeType type);
        IPayGradeQuery ByClientId(int clientId);
        IPayGradeQuery FindPayGrade(int payGradeId);
        IPayGradeQuery OrderByName();
        IPayGradeQuery ByNotThisId(int payGradeId);
    }
}
