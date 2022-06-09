using System.Collections.Generic;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.EntityDataStore.EntityMetadata;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;

namespace Dominion.Domain.EntityMetadata.Employee
{
    /// <summary>
    /// Metadata for the employee emergency contact entity.
    /// </summary>
    public class EmployeeEmergencyContactDataStoreMetadata : IEntityDataStoreMetadata
    {
        #region SINGLETON INSTANCE

        /// <summary>
        /// Holds the singleton value representing this object.
        /// </summary>
        private static EmployeeEmergencyContactDataStoreMetadata _instance = null;

        /// <summary>
        /// Get the singleton instance of this object.
        /// </summary>
        public static EmployeeEmergencyContactDataStoreMetadata Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeeEmergencyContactDataStoreMetadata();

                return _instance;
            }
        }

        #endregion

        #region VARIABLES

        /// <summary>
        /// IF the data store is a database this would be the table name.
        /// </summary>
        private string _dataStoreName;

        /// <summary>
        /// If the data store was a database this would be the list of column metadata.
        /// </summary>    
        private List<IEntityDataStoreFieldMetadata> _fieldMetaData;

        /// <summary>
        /// IF the data store is a database this would be the table name.
        /// </summary>
        public string DataStoreName
        {
            get { return _dataStoreName; }
        }

        /// <summary>
        /// If the data store was a database this would be the list of column metadata.
        /// </summary>
        public IEnumerable<IEntityDataStoreFieldMetadata> FieldMetaData
        {
            get { return _fieldMetaData; }
        }

        /// <summary>
        /// Property expression for the field.
        /// Useful for finding compile time issues and performing reflection.
        /// </summary>
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, int> EmployeeId;

        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string> FirstName;
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string> LastName;
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string> HomePhoneNumber;
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string> Relation;
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, int> EmployeeEmergencyContactId;
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string> CellPhoneNumber;
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, byte?> InsertApproved;
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, int> ClientId;
        public readonly EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string> EmailAddress;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor.
        /// Sets readonly values.
        /// </summary>
        internal EmployeeEmergencyContactDataStoreMetadata()
        {
            _dataStoreName = "EmployeeEmergencyContact";
            _fieldMetaData = new List<IEntityDataStoreFieldMetadata>();

            // EMPLOYEEID
            EmployeeId = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, int>(
                false, 
                false, 
                false, 
                "EmployeeID", 
                0, 
                10, 
                0, 
                x => x.EmployeeId);

            _fieldMetaData.Add(EmployeeId);

            // FIRSTNAME
            FirstName = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string>(
                false, 
                false, 
                false, 
                "FirstName", 
                25, 
                0, 
                0, 
                x => x.FirstName);

            _fieldMetaData.Add(FirstName);

            // LASTNAME
            LastName = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string>(
                false, 
                false, 
                false, 
                "LastName", 
                25, 
                0, 
                0, 
                x => x.LastName);

            _fieldMetaData.Add(LastName);

            // HOMEPHONENUMBER
            HomePhoneNumber = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string>(
                false, 
                false, 
                false, 
                "HomePhone", 
                15, 
                0, 
                0, 
                x => x.HomePhoneNumber);

            _fieldMetaData.Add(HomePhoneNumber);

            // RELATION
            Relation = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string>(
                false, 
                false, 
                false, 
                "Relation", 
                20, 
                0, 
                0, 
                x => x.Relation);

            _fieldMetaData.Add(Relation);

            // EMPLOYEEEMERGENCYCONTACTID
            EmployeeEmergencyContactId = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, int>(
                true, 
                false, 
                false, 
                "EmployeeEmergencyContactID", 
                0, 
                10, 
                0, 
                x => x.EmployeeEmergencyContactId);

            _fieldMetaData.Add(EmployeeEmergencyContactId);

            // CELLPHONENUMBER
            CellPhoneNumber = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string>(
                false, 
                true, 
                false, 
                "CellPhone", 
                15, 
                0, 
                0, 
                x => x.CellPhoneNumber);

            _fieldMetaData.Add(CellPhoneNumber);

            // INSERTAPPROVED
            InsertApproved = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, byte?>(
                false, 
                true, 
                false, 
                "InsertApproved", 
                0, 
                3, 
                0, 
                x => x.InsertApproved);

            _fieldMetaData.Add(InsertApproved);

            // CLIENTID
            ClientId = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, int>(
                false, 
                false, 
                false, 
                "ClientID", 
                0, 
                10, 
                0, 
                x => x.ClientId);

            _fieldMetaData.Add(ClientId);

            // EMAILADDRESS
            EmailAddress = new EntityDataStoreFieldMetadata<EmployeeEmergencyContact, string>(
                false, 
                true, 
                false, 
                "Email", 
                120, 
                0, 
                0, 
                x => x.EmailAddress);

            _fieldMetaData.Add(EmailAddress);
        }

        #endregion
    }
}