using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

using Dominion.Core.Dto.Common;
using Dominion.Core.Dto.Interfaces;
using Dominion.Core.Dto.Utility.ValueObjects;
using Dominion.Domain.Entities.Aca;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.EntityViews;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeDependent :
        Entity<EmployeeDependent>,
        IHasModifiedOptionalData, 
        IEmployeeDependentEntity,
        IEmployeeOwnedEntity<EmployeeDependent>,
        IHasChangeHistoryEntityWithEnum<EmployeeDependentChangeHistory>,
        IHasFirstMiddleInitialLast
    {
        public virtual int          EmployeeDependentId              { get; set; }
        public virtual int          EmployeeId                       { get; set; }
        public virtual Employee     Employee                         { get; set; }
        public virtual string       FirstName                        { get; set; }
        public virtual string       LastName                         { get; set; }
        public virtual string       MiddleInitial                    { get; set; }
        public virtual string       SocialSecurityNumber             { get; set; }
        public virtual string       Relationship                     { get; set; }
        public virtual string       Gender                           { get; set; }
        public virtual string       Comments                         { get; set; }
        public virtual DateTime?    BirthDate                        { get; set; }
        public virtual InsertStatus InsertStatus                     { get; set; }
        public virtual int          ClientId                         { get; set; }
        public virtual Client       Client                           { get; set; }
        public virtual bool         TobaccoUser                      { get; set; }
        public virtual bool         HasADisability                   { get; set; }
        public virtual bool         IsAStudent                       { get; set; }
        public virtual int?         EmployeeDependentsRelationshipId { get; set; }
        public virtual EmployeeDependentRelationships EmployeeDependentRelationship { get; set; }

        public virtual ICollection<Aca1095CCoveredIndividual> Aca1095CCoveredIndividuals   { get; set; }
        public virtual EmployeeDependentPcp                   PrimaryCarePhysician         { get; set; }
        public virtual bool                                   IsInactive                   { get; set; }
        public virtual DateTime?                              InactiveDate                 { get; set; }

        [NotMapped]
        public PersonName PersonName
        {
            get { return new PersonName(this); }
        }

        #region IEmployeeOwnedEntity Members

        public Expression<Func<EmployeeDependent, EmployeeDependent>> GetEmployeeIdView()
        {
            return EmployeeIdView;
        }

        #endregion

        #region Filters

        /// <summary>
        /// Predicate definition used to limit based on a specific employee dependent.
        /// </summary>
        /// <param name="employeeId">The employe dependent id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeDependent, bool>> IsEmployeeDependent(int employeeDependentId)
        {
            return x => x.EmployeeDependentId == employeeDependentId;
        }

        /// <summary>
        /// Predicate definition used to limit based on a specific employee.
        /// </summary>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeDependent, bool>> ForEmployee(int employeeId)
        {
            return x => x.EmployeeId == employeeId;
        }

        /// <summary>
        /// Predicate definition used to limit based on is the record is approved.
        /// </summary>
        /// <param name="employeeId">The status wanted.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeDependent, bool>> ForInsertStatus(InsertStatus InsertStatus)
        {
            return x => x.InsertStatus == InsertStatus;
        }

        /// <summary>
        /// Just get the employee id.
        /// </summary>
        public static Expression<Func<EmployeeDependent, EmployeeDependent>> EmployeeIdView
        {
            get
            {
                return e =>
                    new EmployeeDependentEntityView
                    {
                        EmployeeId = e.EmployeeId
                    };
            }
        }

        #endregion

        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}