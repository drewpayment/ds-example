using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.Tax;
using Dominion.Domain.Entities.User;

namespace Dominion.Domain.EntityViews
{
    [NotMapped]
    public class EmployeeEmergencyContactEntityView : EmployeeEmergencyContact
    {
    }

    [NotMapped]
    public class EmployeeDependentEntityView : EmployeeDependent
    {
    }

    [NotMapped]
    public class EmployeeEntityView : Employee
    {
    }

    [NotMapped]
    public class EmployeeTaxEntityView : EmployeeTax
    {
    }
    [NotMapped]
    public class DriversLicenseEntityView : EmployeeDriversLicense
    {
        public bool DriversLicenseOnFile;
    }
    [NotMapped]
    public class CountryEntityView : Country
    {
    }
    [NotMapped]
    public class CountyEntityView : County
    {
    }

    [NotMapped]
    public class StateEntityView : State
    {
    }

    [NotMapped]
    public class ClientEntityView : Client
    {
    }

    [NotMapped]
    public class UserEntityView : User
    {
    }

    [NotMapped]
    public class JobProfileEntityView : JobProfile
    {
    }

    [NotMapped]
    public class ClientDivisionEntityView : ClientDivision
    {
    }

    [NotMapped]
    public class ClientDepartmentEntityView : ClientDepartment
    {
    }
}