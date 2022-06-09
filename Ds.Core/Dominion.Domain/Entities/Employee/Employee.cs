using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using Dominion.Core.Dto.Interfaces;
using Dominion.Core.Dto.Utility.ValueObjects;
using Dominion.Domain.Entities.Aca;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.EEOC;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Entities.Tax;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.EntityViews;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Utility.Query;
using Dominion.Core.Dto.TimeCard;
using Dominion.Core.Dto.Employee;

namespace Dominion.Domain.Entities.Employee
{
    public class Employee :
        Entity<Employee>, 
        IHasModifiedOptionalData, 
        IEmployeeOwnedEntity<Employee>, 
        IHasFirstMiddleInitialLast,
        IEmployeeReferenceDatesDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public State State { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int? CountyId { get; set; }
        public County County { get; set; }
        public string PostalCode { get; set; }
        public string HomePhoneNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string EmployeeNumber { get; set; }
        public string JobTitle { get; set; }
        public string JobClass { get; set; }
        public int? ClientGroupId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientWorkersCompId { get; set; }
        public bool? IsW2Pension { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public DateTime? RehireDate { get; set; }
        public DateTime? EligibilityDate { get; set; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }
        public int? PayStubOption { get; set; }
        public string Notes { get; set; }
        public byte CostCenterType { get; set; }
        public bool? IsNewHireDataSent { get; set; }
        public bool  IsInOnboarding { get; set; }
        public int ClientId { get; set; }
        public int? DirectSupervisorID { get; set; }
        public User.User DirectSupervisor { get; set; }

        [XmlIgnore]
        public virtual Client Client { get; set; }

        public byte? MaritalStatusId { get; set; }
        public int? EeocRaceId { get; set; }
        public int? EeocJobCategoryId { get; set; }
        public int? EeocLocationId { get; set; }
        public string CellPhoneNumber { get; set; }
        public int? JobProfileId { get; set; }
        public string PsdCode { get; set; }

        [XmlIgnore]
        public virtual ICollection<EmployeeDefaultShift> DefaultShifts { get; set; }

        [XmlIgnore]
        public virtual List<EmployeeDependent> EmployeeDependents { get; set; }

        [XmlIgnore]
        public virtual List<EmployeeEmergencyContact> EmergencyContacts { get; set; }

        [XmlIgnore]
        public virtual List<EmployeeTax> EmployeeTaxes { get; set; }
        public virtual ICollection<ApiAccountMapping> ApiAccountMapping { get; set; } // many-to-one;
        public virtual ICollection<ClockEmployeePunch> Punches { get; set; } 
        public virtual ICollection<ClockClientSchedule> Schedules { get; set; } 
        public virtual ICollection<ClockClientScheduleSelected> SelectedSchedules { get; set; }
        public virtual ICollection<ClockEmployeeSchedule> ScheduleOverrides { get; set; } 
        public virtual ICollection<ClockEmployeeBenefit> ScheduleBenefits { get; set; } 
        public virtual ICollection<PayrollPayData> PayrollPayData { get; set; }
        public virtual ICollection<PaycheckHistory> PaycheckHistory { get; set; }
        public virtual ICollection<EmployeeNotes> EmployeeNotes { get; set; }
        public virtual ClientCostCenter CostCenter { get; set; }
        public virtual ClientDepartment Department { get; set; }
        public virtual ClientGroup Group { get; set; }
        public virtual ClientDivision Division { get; set; }
        public virtual ClockEmployee ClockEmployee { get; set; }
        public virtual EEOCJobCategories EEOCJobCategories { get; set; }
        public virtual ICollection<EmployeePay> EmployeePayInfo { get; set; }
        public virtual ICollection<EmployeeDeduction> EmployeeDeductions { get; set; }
        public virtual ICollection<EmployeeClientRate> EmployeeClientRates { get; set; }
        public virtual JobProfile JobTitleInfo { get; set; }
        public virtual ICollection<Aca1095C> Aca1095Cs { get; set; } 
        public virtual ICollection<Aca1095CLineItem> Aca1095CLineItems { get; set; }
        public virtual EmployeePcp PrimaryCarePhysician { get; set; }
        public virtual ICollection<Aca1095CCoveredIndividual> Aca1095CCoveredIndividuals { get; set; }
        public virtual ICollection<AcaTransmissionRecord> AcaTransmissionRecords { get; set; }
        public virtual ICollection<OneTimeEarningSettings> OneTimeEarningSettings { get; set; }

        public virtual EmployeeDriversLicense DriversLicense { get; set; }

        public virtual EmployeeOnboarding EmployeeOnboarding { get; set; }
        public virtual EmployeeOnboardingW4Assist W4AssistantInfo { get; set; }
        public virtual EmployeeBenefitInfo BenefitInfo { get; set; }
        public virtual ICollection<PlanEmployeeAvailability> BenefitPlanAvailability { get; set; }
        public virtual ICollection<EmployeeOpenEnrollment> EmployeeOpenEnrollments { get; set; }
        public virtual EmployeePerformanceConfiguration EmployeePerformanceConfiguration { get; set; }
        public virtual ICollection<MeetingAttendee> MeetingsAttended { get; set; }
        public virtual ICollection<Review> PerformanceReviews { get; set; }
        public virtual ICollection<EmployeeAccrual> EmployeeAccruals { get; set; }
        //public virtual ICollection<LeaveManagementPendingAward> LeaveManagementPendingAwards { get; set; }
		public virtual ICollection<EmployeeGoal> EmployeeGoals { get; set; }
        public virtual ICollection<EmployeeHRInfo> EmployeeHRInfos { get; set; }
        public virtual ICollection<EmployeeAvatar> EmployeeAvatars { get; set; }

        /// <summary>
        /// This data is used when calculating the recommended bonus for an Employee.
        /// </summary>


        // IModifiableEntity Properties
        [XmlIgnore]
        public virtual DateTime? Modified { get; set; }

        [XmlIgnore]
        public virtual int? ModifiedBy { get; set; }

        [XmlIgnore]
        public virtual ICollection<User.User> UserAccounts { get; set; }

        public Employee()
        {
            EmployeeDependents = new List<EmployeeDependent>();
        }

        [NotMapped]
        public PersonName PersonNameView
        {
            get { return new PersonName(this); }
        }

        #region HELPER EXPRESSIONS

        public static Expression<Func<Employee, string>> FullNameExpression
        {
            get { return x => x.FirstName + " " + x.LastName; }
        }

        public static Expression<Func<Employee, string>> IsActiveAsTextExpression
        {
            get { return emp => emp.Client.IsActive ? "A" : "T"; }
        }

        public static Expression<Func<Employee, string>> ClientNameExpression
        {
            get { return emp => emp.Client.ClientName; }
        }

        #endregion // region HELPER EXPRESSIONS

        #region FILTERS

        /// <summary>
        /// Predicate definition used to limit based on a specific employee dependent.
        /// </summary>
        /// <param name="employeeId">The employe dependent id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<Employee, bool>> IsEmployee(int employeeId)
        {
            return x => x.EmployeeId == employeeId;
        }

        /// <summary>
        /// Predicate definition used to limit based on a specific employee.
        /// </summary>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<Employee, bool>> ForEmployee(int employeeId)
        {
            return x => x.EmployeeId == employeeId;
        }

        public static Expression<Func<Employee, bool>> FirstOrLastNameContains(string nameFilter)
        {
            if (false == string.IsNullOrWhiteSpace(nameFilter))
            {
                return x => (x.FirstName.Contains(nameFilter) || x.LastName.Contains(nameFilter));
            }

            return null;
        }

        public static Expression<Func<Employee, bool>> ClientNameContains(string clientNameFilter)
        {
            if (false == string.IsNullOrWhiteSpace(clientNameFilter))
            {
                return x => x.Client.ClientName.Contains(clientNameFilter);
            }

            return null;
        }

        public static Expression<Func<Employee, bool>> HasEmployeeDependents(bool? shouldHaveDependents)
        {
            if (shouldHaveDependents != null)
            {
                return x => x.EmployeeDependents.Any() == shouldHaveDependents.Value;
            }

            return null;
        }

        public static Expression<Func<Employee, bool>> HasEmployeeDependentCountOfAtLeast(int dependentCount)
        {
            return e => e.EmployeeDependents.Count() >= dependentCount;
        }

        #endregion // region FILTERS

        #region SORTING

        /// <summary>
        /// Creates an expression that will order a collection of employees by whether the employee's client is active ("A") or is terminated ("T")
        /// </summary>
        /// <param name="direction">Ascending - Will order active clients first. Descending - Will order active clients last.</param>
        /// <returns></returns>
        public static SortExpression<Employee, string> SortByClientActiveStatus(
            SortDirection direction = SortDirection.Ascending)
        {
            return new SortExpression<Employee, string>(Employee.IsActiveAsTextExpression, direction, 
                "SortEmployeeByClientActiveStatus");
        }

        public static SortExpression<Employee, string> SortByFullName(SortDirection direction = SortDirection.Ascending)
        {
            return new SortExpression<Employee, string>(Employee.FullNameExpression, direction, "SortEmployeeByFullName");
        }

        public static SortExpression<Employee, string> SortByClientName(
            SortDirection direction = SortDirection.Ascending)
        {
            return new SortExpression<Employee, string>(Employee.ClientNameExpression, direction, 
                "SortEmployeeByClientName");
        }

        #endregion // region SORTING

        /// <summary>
        /// View for getting the employee's contact information data.
        /// </summary>
        public static Expression<Func<Employee, Employee>> EmployeeContactInformationView
        {
            get
            {
                return e =>
                    new EmployeeEntityView
                    {
                        EmployeeId = e.EmployeeId,
                        EmployeeNumber = e.EmployeeNumber,
                        FirstName = e.FirstName, 
                        LastName = e.LastName,
                        MiddleInitial = e.MiddleInitial,
                        Gender = e.Gender,
                        BirthDate = e.BirthDate,
                        MaritalStatusId = e.MaritalStatusId.HasValue ? e.MaritalStatusId.Value : (byte)0,
                        SocialSecurityNumber = e.SocialSecurityNumber,
                        AddressLine1 = e.AddressLine1,
                        AddressLine2 = e.AddressLine2,
                        City = e.City,
                        StateId = e.StateId,
                        CountryId = e.CountryId,
                        PostalCode = e.PostalCode,
                        HomePhoneNumber = e.HomePhoneNumber,
                        CellPhoneNumber = e.CellPhoneNumber,
                        EmailAddress = e.EmailAddress,
                        CountyId = e.CountyId,
                        ClientId = e.ClientId,
                        EeocRaceId = e.EeocRaceId,
                        HireDate = e.HireDate,
                        JobProfileId = e.JobProfileId,
                        JobTitleInfo = e.JobProfileId == null ?
                            new JobProfileEntityView
                            {
                                JobProfileId = default(int),
                                Description = default(string)
                            } :
                            new JobProfileEntityView
                            {
                                JobProfileId = (int)e.JobProfileId,
                                Description = e.JobTitleInfo.Description
                            },
                        Division = e.ClientDivisionId == null ?
                            new ClientDivisionEntityView
                            {
                                ClientDivisionId = default(int),
                                Name = default(string)
                            } :
                            new ClientDivisionEntityView
                            {
                                ClientDivisionId = (int)e.ClientDivisionId,
                                Name = e.Division.Name
                            },
                        Department = e.ClientDepartmentId == null ?
                            new ClientDepartmentEntityView
                            {
                                ClientDepartmentId = default(int),
                                Name = default(string)
                            } :
                            new ClientDepartmentEntityView
                            {
                                ClientDepartmentId = (int)e.ClientDepartmentId,
                                Name = e.Department.Name
                            },
                        Client = new ClientEntityView
                        {
                            ClientId = e.Client.ClientId,
                            ClientName = e.Client.ClientName
                        },
                        State = new StateEntityView
                        {
                            StateId = e.State.StateId,
                            Name = e.State.Name
                        },
                        Country = new CountryEntityView
                        {
                            CountryId = e.Country.CountryId,
                            Name = e.Country.Name
                        },
                        County = e.CountyId == null ?
                        new CountyEntityView()
                        {
                            CountyId = default(int),
                            Name = default(string),
                            StateId = default(int),
                            Fips = default(string)
                        } :

                            new CountyEntityView()
                            {
                                CountyId = e.County.CountyId,
                                Name = e.County.Name,
                                StateId = e.County.StateId,
                                Fips = e.County.Fips
                            },
                        DriversLicense = e.DriversLicense == null ?
                        new DriversLicenseEntityView()
                        {
                            DriversLicenseNumber = default(string),
                            IssuingStateId = default(int),
                            ExpirationDate = default(DateTime?),
                            IssuingState = null
                        } :
                        new DriversLicenseEntityView()
                        {
                            DriversLicenseNumber = e.DriversLicense.DriversLicenseNumber,
                            IssuingStateId = e.DriversLicense.IssuingStateId,
                            ExpirationDate = e.DriversLicense.ExpirationDate,
                            IssuingState = e.DriversLicense.IssuingState
                        }

                    };
            }
        }

        #region IEmployeeOwnedEnity Interface Method

        /// <summary>
        /// Get the expression view to get the employee enity's id.
        /// We can use the principal data to verify.
        /// </summary>
        /// <returns></returns>
        public Expression<Func<Employee, Employee>> GetEmployeeIdView()
        {
            throw new NotImplementedException();
        }

        #endregion


        public virtual EEOCLocation EEOCLocation { get; set; }
        public virtual EeocRaceEthnicCategories EeocRaceEthnicCategories { get; set; }
    }
}