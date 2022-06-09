using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.EntityDataStore.EntityMetadata;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;

namespace Dominion.Domain.EntityMetadata.Benefit
{
    /// <summary>
    /// Metadata for the employee benefit info entity.
    /// </summary>
    public class EmployeeAccrualDataStoreMetadata : IEntityDataStoreMetadata
    {
        #region SINGLETON INSTANCE

        /// <summary>
        /// Holds the singleton value representing this object.
        /// </summary>
        private static EmployeeAccrualDataStoreMetadata _instance = null;

        /// <summary>
        /// Get the singleton instance of this object.
        /// </summary>
        public static EmployeeAccrualDataStoreMetadata Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeeAccrualDataStoreMetadata();

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
        public readonly EntityDataStoreFieldMetadata<EmployeeAccrual, int> EmployeeAccrualId;
        public readonly EntityDataStoreFieldMetadata<EmployeeAccrual, int> EmployeeId;
        public readonly EntityDataStoreFieldMetadata<EmployeeAccrual, int> ClientAccrualId;
        public readonly EntityDataStoreFieldMetadata<EmployeeAccrual, bool> IsAllowScheduledAwards;
        public readonly EntityDataStoreFieldMetadata<EmployeeAccrual, DateTime?> WaitingPeriodDate;
        public readonly EntityDataStoreFieldMetadata<EmployeeAccrual, bool> IsActive;
        public readonly EntityDataStoreFieldMetadata<EmployeeAccrual, DateTime?> Modified;
        public readonly EntityDataStoreFieldMetadata<EmployeeAccrual, int?> ModifiedBy;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor.
        /// Sets readonly values.
        /// </summary>
        internal EmployeeAccrualDataStoreMetadata()
        {
            _dataStoreName = "EmployeeAccrual";
            _fieldMetaData = new List<IEntityDataStoreFieldMetadata>();

            // EMPLOYEEACCRUALID
            EmployeeAccrualId = new EntityDataStoreFieldMetadata<EmployeeAccrual, int>(
                true,
                false,
                false,
                "EmployeeAccrualId",
                0,
                10,
                0,
                x => x.EmployeeAccrualId);

            _fieldMetaData.Add(EmployeeAccrualId);

            // EMPLOYEEID
            EmployeeId = new EntityDataStoreFieldMetadata<EmployeeAccrual, int>(
                false, 
                false, 
                false, 
                "EmployeeId", 
                0, 
                10, 
                0, 
                x => x.EmployeeId);

            _fieldMetaData.Add(EmployeeId);

            // CLIENTACCRUALID
            ClientAccrualId = new EntityDataStoreFieldMetadata<EmployeeAccrual, int>(
                false,
                false,
                false,
                "ClientAccrualId",
                0,
                10,
                0,
                x => x.ClientAccrualId);

            _fieldMetaData.Add(ClientAccrualId);


            // IsAllowScheduledAwards
            IsAllowScheduledAwards = new EntityDataStoreFieldMetadata<EmployeeAccrual, bool>(
                false, 
                true, 
                false,
                "AllowScheduledAwards", 
                0, 
                3, 
                0, 
                x => x.IsAllowScheduledAwards);

            _fieldMetaData.Add(IsAllowScheduledAwards);


            // IsActive
            IsActive = new EntityDataStoreFieldMetadata<EmployeeAccrual, bool>(
                false,
                true,
                false,
                "IsActive",
                0,
                3,
                0,
                x => x.IsActive);

            _fieldMetaData.Add(IsActive);


            // WaitingPeriodDate
            WaitingPeriodDate = new EntityDataStoreFieldMetadata<EmployeeAccrual, DateTime?>(
                false, 
                false, 
                false,
                "WaitingPeriodDate", 
                0, 
                0, 
                0, 
                x => x.WaitingPeriodDate);

            _fieldMetaData.Add(WaitingPeriodDate);

            //Modified
            Modified = new EntityDataStoreFieldMetadata<EmployeeAccrual, DateTime?>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "Modified",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.Modified);

            _fieldMetaData.Add(Modified);


            // Modified By
            ModifiedBy = new EntityDataStoreFieldMetadata<EmployeeAccrual, int?>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "ModifiedBy",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.ModifiedBy);

            _fieldMetaData.Add(ModifiedBy);

        }

        #endregion
    }
}