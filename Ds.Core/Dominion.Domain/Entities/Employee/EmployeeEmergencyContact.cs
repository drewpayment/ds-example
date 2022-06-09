using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

using Dominion.Core.Dto.Interfaces;
using Dominion.Core.Dto.Utility.ValueObjects;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.EntityViews;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    /// Defines if record has been approved for actual use.
    /// The term 'Insert' refers to the idea that it's allowed to be considered a usable record  from the database.
    /// </summary>
    public enum InsertStatusApproved : byte
    {
        /// <summary>
        /// This record should not be considered a usable record.
        /// </summary>
        Pending = 0, 

        /// <summary>
        /// This is a usable record.
        /// </summary>
        Approved = 1
    }


    public class EmployeeEmergencyContact :
        Entity<EmployeeEmergencyContact>,
        IHasModifiedOptionalData, 
        IEmployeeOwnedEntity<EmployeeEmergencyContact>
    {
        public virtual int EmployeeEmergencyContactId { get; set; }

        public virtual int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        
        public virtual int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual string HomePhoneNumber { get; set; }
        public virtual string CellPhoneNumber { get; set; }

        public virtual string EmailAddress { get; set; }
        public virtual string Relation { get; set; }
        public virtual byte? InsertApproved { get; set; }

        [NotMapped]
        public PersonName PersonName
        {
            get { return new PersonName(this.FirstName,"",this.LastName); }
        }

        [NotMapped]
        public string Relationship
        {
            get { return Relation; }
            set { Relation = value; }
        }

        #region IEmployeeOwnedEntity Members

        public Expression<Func<EmployeeEmergencyContact, EmployeeEmergencyContact>> GetEmployeeIdView()
        {
            return EmployeeIdView;
        }

        #endregion

        #region Views

        /// <summary>
        /// Just get the employee id.
        /// </summary>
        public static Expression<Func<EmployeeEmergencyContact, EmployeeEmergencyContact>> EmployeeIdView
        {
            get
            {
                return e =>
                    new EmployeeEmergencyContactEntityView
                    {
                        EmployeeId = e.EmployeeId
                    };
            }
        }

        /// <summary>
        /// View for building lists of emergency contacts for a list only showing first and last name.
        /// Id is included for correlation purposes.
        /// </summary>
        public static Expression<Func<EmployeeEmergencyContact, EmployeeEmergencyContact>> FirstLastListEntityView
        {
            get
            {
                return e =>
                    new EmployeeEmergencyContactEntityView
                    {
                        EmployeeEmergencyContactId = e.EmployeeEmergencyContactId, 
                        FirstName = e.FirstName, 
                        LastName = e.LastName
                    };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Expression<Func<EmployeeEmergencyContact, EmployeeEmergencyContact>> EmployeeEmergencyContactFullEntityView
        {
            get
            {
                return e =>
                    new EmployeeEmergencyContactEntityView
                    {
                        EmployeeId = e.EmployeeId, 
                        FirstName = e.FirstName, 
                        LastName = e.LastName, 
                        HomePhoneNumber = e.HomePhoneNumber, 
                        Relation = e.Relation, 
                        Modified = e.Modified, 
                        ModifiedBy = e.ModifiedBy, 
                        EmployeeEmergencyContactId = e.EmployeeEmergencyContactId, 
                        CellPhoneNumber = e.CellPhoneNumber, 
                        InsertApproved = e.InsertApproved, 
                        ClientId = e.ClientId, 
                        EmailAddress = e.EmailAddress, 
                    };
            }
        }




        /// <summary>
        /// 
        /// </summary>
        public static Expression<Func<EmployeeEmergencyContact, EmployeeEmergencyContact>> EmployeeEmergencyContactOnboardingView
        {
            get
            {
                return e =>
                    new EmployeeEmergencyContactEntityView
                    {
                        EmployeeId = e.EmployeeId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Relation = e.Relation,
                        EmployeeEmergencyContactId = e.EmployeeEmergencyContactId,
                        CellPhoneNumber = e.CellPhoneNumber,
                        ClientId = e.ClientId,
                        EmailAddress = e.EmailAddress,
                        
                    };
            }
        }



        #endregion

        #region Filters

        /// <summary>
        /// Predicate definition used to limit based on a specific employee.
        /// </summary>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeEmergencyContact, bool>> ForEmployee(int employeeId)
        {
            return x => x.EmployeeId == employeeId;
        }

        /// <summary>
        /// Predicate definition used to limit based on a specific emergnecy contact.
        /// </summary>
        /// <param name="emergencyContactId">The entity's id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeEmergencyContact, bool>> ForEmergencyContact(
            int employeeEmergencyContactId)
        {
            return x => x.EmployeeEmergencyContactId == employeeEmergencyContactId;
        }

        /// <summary>
        /// Predicate definition used to limit based on a specific employee emergency contact.
        /// </summary>
        /// <param name="employeeId">The employee emergency contact id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeEmergencyContact, bool>> IsEmployeeEmergencyContact(
            int employeeEmergencyContactId)
        {
            return x => x.EmployeeEmergencyContactId == employeeEmergencyContactId;
        }

        #endregion

        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}