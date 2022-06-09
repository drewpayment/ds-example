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
    public class EmployeeBenefitDataStoreMetadata : IEntityDataStoreMetadata
    {
        #region SINGLETON INSTANCE

        /// <summary>
        /// Holds the singleton value representing this object.
        /// </summary>
        private static EmployeeBenefitDataStoreMetadata _instance = null;

        /// <summary>
        /// Get the singleton instance of this object.
        /// </summary>
        public static EmployeeBenefitDataStoreMetadata Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeeBenefitDataStoreMetadata();

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
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, int> EmployeeId;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, bool> IsTobaccoUser;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, bool> IsEligible;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, DateTime?> EligibilityDate;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, int?> BenefitPackageId;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, byte?> DefaultSalaryMethod;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, DateTime> Modified;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, int> ModifiedBy;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, int?> ClientEmploymentClassId;
        public readonly EntityDataStoreFieldMetadata<EmployeeBenefitInfo, bool> IsRetirementEligible;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor.
        /// Sets readonly values.
        /// </summary>
        internal EmployeeBenefitDataStoreMetadata()
        {
            _dataStoreName = "bpEmployeeBenefitInfo";
            _fieldMetaData = new List<IEntityDataStoreFieldMetadata>();

            // EMPLOYEEID
            EmployeeId = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, int>(
                true, 
                false, 
                false, 
                "EmployeeId", 
                0, 
                10, 
                0, 
                x => x.EmployeeId);

            _fieldMetaData.Add(EmployeeId);


            // IsTobaccoUser
            IsTobaccoUser = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, bool>(
                false, 
                true, 
                false,
                "IsTobaccoUser", 
                0, 
                3, 
                0, 
                x => x.IsTobaccoUser);

            _fieldMetaData.Add(IsTobaccoUser);


            // IsEligible
            IsEligible = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, bool>(
                false, 
                true, 
                false,
                "IsEligible", 
                0, 
                3, 
                0, 
                x => x.IsEligible);

            _fieldMetaData.Add(IsEligible);


            // EligibilityDate
            EligibilityDate = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, DateTime?>(
                false, 
                false, 
                false,
                "EligibilityDate", 
                0, 
                0, 
                0, 
                x => x.EligibilityDate);

            _fieldMetaData.Add(EligibilityDate);


            // BenefitPackageId
            BenefitPackageId = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, int?>(
                false, 
                false, 
                false,
                "BenefitPackageId", 
                0, 
                10, 
                0, 
                x => x.BenefitPackageId);

            _fieldMetaData.Add(BenefitPackageId);


            // DefaultSalaryMethod
            DefaultSalaryMethod = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, byte?>(
                false, 
                false, 
                false,
                "DefaultSalaryMethodTypeId", 
                0, 
                3, 
                0, 
                x => (byte?)x.DefaultSalaryMethod);

            _fieldMetaData.Add(DefaultSalaryMethod);


            //Modified
            Modified = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, DateTime>(
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
            ModifiedBy = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, int>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "ModifiedBy",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.ModifiedBy);

            _fieldMetaData.Add(ModifiedBy);


            // ClientEmploymentClassId
            ClientEmploymentClassId = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, int?>(
                false, 
                false, 
                false,
                "ClientEmploymentClassId", 
                0, 
                10, 
                0, 
                x => x.ClientEmploymentClassId);

            _fieldMetaData.Add(ClientEmploymentClassId);


            // IsRetirementEligible
            IsRetirementEligible = new EntityDataStoreFieldMetadata<EmployeeBenefitInfo, bool>(
                false, 
                true, 
                false,
                "IsRetirementEligible", 
                0, 
                3, 
                0, 
                x => x.IsRetirementEligible);

            _fieldMetaData.Add(IsRetirementEligible);
        }

        #endregion
    }
}