using System;
using System.Collections.Generic;
using System.Linq;

using Dominion.Utility.Constants;
using Dominion.Utility.EntityDataStore.EntityMetadata;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;

namespace Dominion.Domain.EntityMetadata.Employee
{
    public class EmployeeDataStoreMetadata : IEntityDataStoreMetadata
    {
        #region SINGLETON INSTANCE

        private static EmployeeDataStoreMetadata _instance = null;

        public static EmployeeDataStoreMetadata Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeeDataStoreMetadata();

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

        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int> EmployeeId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> FirstName;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> LastName;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> MiddleInitial;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> AddressLine1;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> AddressLine2;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> City;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> StateId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> CountyId; 
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> PostalCode;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int> CountryId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> HomePhoneNumber;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> SocialSecurityNumber;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> Gender;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?> BirthDate;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> EmployeeNumber;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> JobTitle;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> JobClass;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> ClientDivisionId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> ClientDepartmentId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> ClientCostCenterId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> ClientGroupId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> ClientWorkersCompId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, bool?> IsW2Pension;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?> HireDate;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?> SeparationDate;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?> AnniversaryDate;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?> RehireDate;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?> EligibilityDate;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, bool> IsActive;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> EmailAddress;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> PayStubOption;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> Notes;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, byte> CostCenterType;


// public readonly EntityDataStoreFieldMetadata<Employee, DateTime> Modified;
        // public readonly EntityDataStoreFieldMetadata<Employee, string> ModifiedBy;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, bool?> IsNewHireDataSent;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int> ClientId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, byte?> MaritalStatusId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> EeocRaceId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> EeocJobCategoryId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, string> CellPhoneNumber;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> EeocLocationId;
        public readonly EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?> JobProfileId;
    
        #endregion

        #region CONSTRUCTOR

        internal EmployeeDataStoreMetadata()
        {
            _dataStoreName = "Employee";
            _fieldMetaData = new List<IEntityDataStoreFieldMetadata>();

            EmployeeId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int>(
                isPrimaryKey: true, 
                required: true, 
                isCurrency: false, 
                fieldName: "EmployeeID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.EmployeeId);

            _fieldMetaData.Add(EmployeeId);

            FirstName = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "FirstName", 
                maxLength: 25, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.FirstName);

            _fieldMetaData.Add(FirstName);

            LastName = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "LastName", 
                maxLength: 25, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.LastName);

            _fieldMetaData.Add(LastName);

            MiddleInitial = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "MiddleInitial", 
                maxLength: 1, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.MiddleInitial);

            _fieldMetaData.Add(MiddleInitial);

            AddressLine1 = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "Address1", 
                maxLength: 50, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.AddressLine1);

            _fieldMetaData.Add(AddressLine1);

            AddressLine2 = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "Address2", 
                maxLength: 50, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.AddressLine2);

            _fieldMetaData.Add(AddressLine2);

            City = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "City", 
                maxLength: 25, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.City);

            _fieldMetaData.Add(City);

            StateId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "StateID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.StateId);

            _fieldMetaData.Add(StateId);

            
            CountyId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "CountyID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.CountyId);

            _fieldMetaData.Add(CountyId);

            PostalCode = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "ZipCode", 
                maxLength: 10, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.PostalCode);

            _fieldMetaData.Add(PostalCode);

            CountryId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "CountryID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.CountryId);

            _fieldMetaData.Add(CountryId);

            HomePhoneNumber = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "HomePhone", 
                maxLength: 15, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.HomePhoneNumber);

            _fieldMetaData.Add(HomePhoneNumber);

            SocialSecurityNumber = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "SocialSecurityNumber", 
                maxLength: 11, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.SocialSecurityNumber);

            _fieldMetaData.Add(SocialSecurityNumber);

            Gender = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "Gender", 
                maxLength: 1, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.Gender);

            _fieldMetaData.Add(Gender);

            BirthDate = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "DateOfBirth", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.BirthDate);

            _fieldMetaData.Add(BirthDate);

            EmployeeNumber = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "EmployeeNumber", 
                maxLength: 10, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.EmployeeNumber);

            _fieldMetaData.Add(EmployeeNumber);

            JobTitle = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "JobTitle", 
                maxLength: 50, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.JobTitle);

            _fieldMetaData.Add(JobTitle);

            JobClass = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "JobClass", 
                maxLength: 50, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.JobClass);

            _fieldMetaData.Add(JobClass);

            ClientDivisionId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "ClientDivisionID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientDivisionId);

            _fieldMetaData.Add(ClientDivisionId);

            ClientDepartmentId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "ClientDepartmentID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientDepartmentId);

            _fieldMetaData.Add(ClientDepartmentId);

            ClientCostCenterId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "ClientCostCenterID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientCostCenterId);

            _fieldMetaData.Add(ClientCostCenterId);

            ClientGroupId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "ClientGroupID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientGroupId);

            _fieldMetaData.Add(ClientGroupId);

            ClientWorkersCompId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "ClientWorkersCompID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientWorkersCompId);

            _fieldMetaData.Add(ClientWorkersCompId);

            IsW2Pension = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, bool?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "IsW2Pension", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.IsW2Pension);

            _fieldMetaData.Add(IsW2Pension);

            HireDate = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "HireDate", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.HireDate);

            _fieldMetaData.Add(HireDate);

            SeparationDate = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "SeparationDate", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.SeparationDate);

            _fieldMetaData.Add(SeparationDate);

            AnniversaryDate = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "AnniversaryDate", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.AnniversaryDate);

            _fieldMetaData.Add(AnniversaryDate);

            RehireDate = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "RehireDate", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.RehireDate);

            _fieldMetaData.Add(RehireDate);

            EligibilityDate = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, DateTime?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "EligibilityDate", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.EligibilityDate);

            _fieldMetaData.Add(EligibilityDate);

            IsActive = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, bool>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "IsActive", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.IsActive);

            _fieldMetaData.Add(IsActive);

            EmailAddress = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "Email", 
                maxLength: 50, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.EmailAddress);

            _fieldMetaData.Add(EmailAddress);

            PayStubOption = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "PayStubOption", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.PayStubOption);

            _fieldMetaData.Add(PayStubOption);

            Notes = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "Notes", 
                maxLength: 4000, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.Notes);

            _fieldMetaData.Add(Notes);

            CostCenterType = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, byte>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "CostCenterType", 
                maxLength: 0, 
                precision: 3, 
                scale: 0, 
                propertyExpression: x => x.CostCenterType);

            _fieldMetaData.Add(CostCenterType);

            // Modified = new EntityDataStoreFieldMetadata<Employee, DateTime>(
            // isPrimaryKey:        false,
            // required:            true,
            // isCurrency:          false,
            // fieldName:           "Modified",
            // maxLength:           0,
            // precision:           0,
            // scale:               0,
            // propertyExpression:  x => x.Modified);

            // _fieldMetaData.Add(Modified);

            // ModifiedBy = new EntityDataStoreFieldMetadata<Employee, string>(
            // isPrimaryKey:        false,
            // required:            true,
            // isCurrency:          false,
            // fieldName:           "ModifiedBy",
            // maxLength:           25,
            // precision:           0,
            // scale:               0,
            // propertyExpression:  x => x.ModifiedBy);

            // _fieldMetaData.Add(ModifiedBy);
            IsNewHireDataSent = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, bool?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "NewHireDataSent", 
                maxLength: 0, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.IsNewHireDataSent);

            _fieldMetaData.Add(IsNewHireDataSent);

            ClientId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "ClientID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientId);

            _fieldMetaData.Add(ClientId);

            MaritalStatusId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, byte?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "MaritalStatusID", 
                maxLength: 0, 
                precision: 3, 
                scale: 0, 
                propertyExpression: x => x.MaritalStatusId);

            _fieldMetaData.Add(MaritalStatusId);

            EeocRaceId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "EEOCRaceID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.EeocRaceId);

            _fieldMetaData.Add(EeocRaceId);

            EeocJobCategoryId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "EEOCJobCategoryID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.EeocJobCategoryId);

            _fieldMetaData.Add(EeocJobCategoryId);

            CellPhoneNumber = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "CellPhone", 
                maxLength: 50, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.CellPhoneNumber);

            _fieldMetaData.Add(CellPhoneNumber);

            EeocLocationId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "EEOCLocationID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.EeocLocationId);

            _fieldMetaData.Add(EeocLocationId);

            JobProfileId = new EntityDataStoreFieldMetadata<Entities.Employee.Employee, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "JobProfileID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.JobProfileId);

            _fieldMetaData.Add(JobProfileId);

            
        }

        #endregion

        public object GetPropertyValueBasedOnDataStoreColumn(string column)
        {
            return null;
        }

        /// <summary>
        /// Get a list of fields to ignore for employee contact information.
        /// These are fields that aren't noted in when working with employee contact information.
        /// </summary>
        /// <returns>List of fields to ignore for employee contact/</returns>
        public static IEnumerable<IEntityDataStoreFieldMetadata> EmployeeContactIgnoredFields()
        {
            var list = new List<IEntityDataStoreFieldMetadata>();
            var metadata = EmployeeDataStoreMetadata.Instance;

            list.Add(metadata.ClientId);
            list.Add(metadata.SocialSecurityNumber);
            list.Add(metadata.Gender);
            list.Add(metadata.BirthDate);
            list.Add(metadata.EmployeeNumber);
            list.Add(metadata.JobTitle);
            list.Add(metadata.JobClass);
            list.Add(metadata.ClientDivisionId);
            list.Add(metadata.ClientDepartmentId);
            list.Add(metadata.ClientCostCenterId);
            list.Add(metadata.ClientGroupId);
            list.Add(metadata.ClientWorkersCompId);
            list.Add(metadata.IsW2Pension);
            list.Add(metadata.HireDate);
            list.Add(metadata.SeparationDate);
            list.Add(metadata.AnniversaryDate);
            list.Add(metadata.RehireDate);
            list.Add(metadata.EligibilityDate);
            list.Add(metadata.IsActive);
            list.Add(metadata.PayStubOption);
            list.Add(metadata.Notes);
            list.Add(metadata.CostCenterType);
            list.Add(metadata.IsNewHireDataSent);
            list.Add(metadata.EeocRaceId);
            list.Add(metadata.EeocJobCategoryId);
            list.Add(metadata.EeocLocationId);
            list.Add(metadata.JobProfileId);

            return list;
        }

        /// <summary>
        /// Get the fields that are needed for employee contact data.
        /// </summary>
        /// <returns>Fields for employee contact data.</returns>
        public static List<IEntityDataStoreFieldMetadata> EmployeeContactFields()
        {
            var toIgnore = EmployeeContactIgnoredFields();

            var toKeep = EmployeeDataStoreMetadata.Instance.FieldMetaData
                .Where(fmd => !toIgnore.Any(ignoredItems => ignoredItems.DataStoreFieldName == fmd.DataStoreFieldName));

            return toKeep.ToList();
        }
    }
}