using System;


namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public class EmployeeStatusDto
    {
        public virtual EmployeeStatusType        EmployeeStatusId                 { get; set; }
        public virtual string     Description                     { get; set; }
        public virtual bool       IsActive                           { get; set; }
       
    }
}
