using System;
using System.Collections.Generic;

using Dominion.Core.Dto.Common;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.EntityDataStore.EntityMetadata;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;

namespace Dominion.Domain.EntityMetadata.Employee
{
    public class EmployeeDependentDataStoreMetadata : IEntityDataStoreMetadata
    {
        #region SINGLETON INSTANCE

        private static EmployeeDependentDataStoreMetadata _instance = null;

        public static EmployeeDependentDataStoreMetadata Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeeDependentDataStoreMetadata();

                return _instance;
            }
        }

        #endregion

        #region VARIABLES

        private string _dataStoreName;
        private List<IEntityDataStoreFieldMetadata> _fieldMetaData;

        public string DataStoreName
        {
            get { return _dataStoreName; }
        }

        public IEnumerable<IEntityDataStoreFieldMetadata> FieldMetaData
        {
            get { return _fieldMetaData; }
        }

        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, int> EmployeeDependentId;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, int> EmployeeId;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, string> FirstName;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, string> LastName;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, string> MiddleInitial;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, string> SocialSecurityNumber;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, string> Relationship;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, string> Gender;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, string> Comments;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, DateTime?> BirthDate;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, InsertStatus> InsertStatus;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, int> ClientId;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, bool> IsAStudent;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, bool> HasADisability;
        public readonly EntityDataStoreFieldMetadata<EmployeeDependent, bool> TobaccoUser; 

        #endregion

        #region CONSTRUCTOR

        internal EmployeeDependentDataStoreMetadata()
        {
            _dataStoreName = "EmployeeDependent";
            _fieldMetaData = new List<IEntityDataStoreFieldMetadata>();

            // EMPLOYEEDEPENDENTID
            EmployeeDependentId = new EntityDataStoreFieldMetadata<EmployeeDependent, int>(
                isPrimaryKey: true, 
                required: false, 
                isCurrency: false, 
                fieldName: "EmployeeDependentID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.EmployeeDependentId);

            _fieldMetaData.Add(EmployeeDependentId);

            // EMPLOYEEID
            EmployeeId = new EntityDataStoreFieldMetadata<EmployeeDependent, int>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "EmployeeID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.EmployeeId);

            _fieldMetaData.Add(EmployeeId);

            // FIRSTNAME
            FirstName = new EntityDataStoreFieldMetadata<EmployeeDependent, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "FirstName", 
                maxLength: 25, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.FirstName);

            _fieldMetaData.Add(FirstName);

            // LASTNAME
            LastName = new EntityDataStoreFieldMetadata<EmployeeDependent, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "LastName", 
                maxLength: 25, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.LastName);

            _fieldMetaData.Add(LastName);

            // MIDDLEINITIAL
            MiddleInitial = new EntityDataStoreFieldMetadata<EmployeeDependent, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "MiddleInitial", 
                maxLength: 1, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.MiddleInitial);

            _fieldMetaData.Add(MiddleInitial);

            // SOCIALSECURITYNUMBER
            SocialSecurityNumber = new EntityDataStoreFieldMetadata<EmployeeDependent, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "SocialSecurityNumber", 
                maxLength: 11, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.SocialSecurityNumber);

            _fieldMetaData.Add(SocialSecurityNumber);

            // RELATIONSHIP
            Relationship = new EntityDataStoreFieldMetadata<EmployeeDependent, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "Relationship", 
                maxLength: 15, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.Relationship);

            _fieldMetaData.Add(Relationship);

            // GENDER
            Gender = new EntityDataStoreFieldMetadata<EmployeeDependent, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "Gender", 
                maxLength: 1, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.Gender);

            _fieldMetaData.Add(Gender);

            // COMMENTS
            Comments = new EntityDataStoreFieldMetadata<EmployeeDependent, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "Comments", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.Comments);

            _fieldMetaData.Add(Comments);

            // BIRTHDATE
            BirthDate = new EntityDataStoreFieldMetadata<EmployeeDependent, DateTime?>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "DateOfBirth", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.BirthDate);

            _fieldMetaData.Add(BirthDate);

            // INSERTAPPROVED
            InsertStatus = new EntityDataStoreFieldMetadata<EmployeeDependent, InsertStatus>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "InsertApproved", 
                maxLength: 0, 
                precision: 3, 
                scale: 0, 
                propertyExpression: x => x.InsertStatus);

            _fieldMetaData.Add(InsertStatus);

            // CLIENTID
            ClientId = new EntityDataStoreFieldMetadata<EmployeeDependent, int>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "ClientID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientId);

            _fieldMetaData.Add(ClientId);

              // IsAStudent (DB saved as a boolean, but needs to display 'True/False' on change requests)
            IsAStudent = new EntityDataStoreFieldMetadata<EmployeeDependent, bool>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "IsAStudent", 
                maxLength: 5, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.IsAStudent);

            _fieldMetaData.Add(IsAStudent);

             HasADisability = new EntityDataStoreFieldMetadata<EmployeeDependent, bool>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "HasADisability", 
                maxLength: 5, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.HasADisability);

            _fieldMetaData.Add(HasADisability);

             TobaccoUser = new EntityDataStoreFieldMetadata<EmployeeDependent, bool>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "TobaccoUser", 
                maxLength: 5, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.TobaccoUser);

            _fieldMetaData.Add(TobaccoUser);

        }

        #endregion
    }
}