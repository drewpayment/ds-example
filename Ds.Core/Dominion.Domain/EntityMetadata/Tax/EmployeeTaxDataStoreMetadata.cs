using System.Collections.Generic;
using Dominion.Domain.Entities.Tax;
using Dominion.Utility.EntityDataStore.EntityMetadata;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;

namespace Dominion.Domain.EntityMetadata.Tax
{
    /// <summary>
    /// This class defines property metadata for the EmployeeTax entity.
    /// </summary>
    public class EmployeeTaxDataStoreMetadata : IEntityDataStoreMetadata
    {
        #region SINGLETON INSTANCE

        private static EmployeeTaxDataStoreMetadata _instance = null;

        /// <summary>
        /// Get the singleton instance of this class.
        /// </summary>
        public static EmployeeTaxDataStoreMetadata Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeeTaxDataStoreMetadata();

                return _instance;
            }
        }

        #endregion

        #region IEntityDataStoreMetadata IMPLEMENTATION

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

        #endregion // IEntityDataStoreMetadata IMPLEMENTATION

        #region PUBLIC PROPERTIES

        public readonly EntityDataStoreFieldMetadata<EmployeeTax, int> EmployeeTaxId;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, int> EmployeeId;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, int?> ClientTaxId;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, int> FilingStatusId;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, FilingStatusInfo> FilingStatusInfo;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, byte> NumberOfExemptions;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, byte> NumberOfDependents;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, byte> AdditionalTaxAmountTypeId;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, double> AdditionalTaxPercent;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, double> AdditionalTaxAmount;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, bool> IsResident;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, bool> IsActive;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, string> Description;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, int> ClientId;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, int?> ResidentId;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, decimal> TaxCredit;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, decimal> OtherTaxableIncome;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, decimal> WageDeduction;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, bool> HasMoreThanOneJob;
        public readonly EntityDataStoreFieldMetadata<EmployeeTax, bool> Using2020FederalW4Setup;

        #endregion // PUBLIC PROPERTIES

        #region CONSTRUCTOR

        /// <summary>
        /// Private constructor to be used by the Instance property.
        /// </summary>
        private EmployeeTaxDataStoreMetadata()
        {
            // set the IEntityDataStoreMetadata values.
            _dataStoreName = "EmployeeTax";
            _fieldMetaData = new List<IEntityDataStoreFieldMetadata>();

            EmployeeTaxId = new EntityDataStoreFieldMetadata<EmployeeTax, int>(
                isPrimaryKey: true, 
                required: true, 
                isCurrency: false, 
                fieldName: "EmployeeTaxID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.EmployeeTaxId);
            _fieldMetaData.Add(EmployeeTaxId);

            EmployeeId = new EntityDataStoreFieldMetadata<EmployeeTax, int>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "EmployeeID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.EmployeeId);
            _fieldMetaData.Add(EmployeeId);

            ClientTaxId = new EntityDataStoreFieldMetadata<EmployeeTax, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "ClientTaxID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientTaxId);
            _fieldMetaData.Add(ClientTaxId);

            FilingStatusId = new EntityDataStoreFieldMetadata<EmployeeTax, int>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "MaritalStatusID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => (int) x.FilingStatus);
            _fieldMetaData.Add(FilingStatusId);

            NumberOfExemptions = new EntityDataStoreFieldMetadata<EmployeeTax, byte>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "Exemptions", 
                maxLength: 0, 
                precision: 3, 
                scale: 0, 
                propertyExpression: x => x.NumberOfExemptions);
            _fieldMetaData.Add(NumberOfExemptions);

            NumberOfDependents = new EntityDataStoreFieldMetadata<EmployeeTax, byte>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "Dependent", 
                maxLength: 0, 
                precision: 3, 
                scale: 0, 
                propertyExpression: x => x.NumberOfDependents);
            _fieldMetaData.Add(NumberOfDependents);

            AdditionalTaxAmountTypeId = new EntityDataStoreFieldMetadata<EmployeeTax, byte>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "AdditionalAmountType", 
                maxLength: 0, 
                precision: 3, 
                scale: 0, 
                propertyExpression: x => x.AdditionalTaxAmountTypeId);
            _fieldMetaData.Add(AdditionalTaxAmountTypeId);

            AdditionalTaxPercent = new EntityDataStoreFieldMetadata<EmployeeTax, double>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "AdditionalPercent", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.AdditionalTaxPercent);
            _fieldMetaData.Add(AdditionalTaxPercent);

            AdditionalTaxAmount = new EntityDataStoreFieldMetadata<EmployeeTax, double>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "AdditionalAmount", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.AdditionalTaxAmount);
            _fieldMetaData.Add(AdditionalTaxAmount);

            IsResident = new EntityDataStoreFieldMetadata<EmployeeTax, bool>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "IsResident", 
                maxLength: 0, 
                precision: 3, 
                scale: 0, 
                propertyExpression: x => x.IsResident);
            _fieldMetaData.Add(IsResident);

            IsActive = new EntityDataStoreFieldMetadata<EmployeeTax, bool>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "IsActive", 
                maxLength: 0, 
                precision: 3, 
                scale: 0, 
                propertyExpression: x => x.IsActive);
            _fieldMetaData.Add(IsActive);

            Description = new EntityDataStoreFieldMetadata<EmployeeTax, string>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "Description", 
                maxLength: 50, 
                precision: 0, 
                scale: 0, 
                propertyExpression: x => x.Description);
            _fieldMetaData.Add(Description);

            ClientId = new EntityDataStoreFieldMetadata<EmployeeTax, int>(
                isPrimaryKey: false, 
                required: true, 
                isCurrency: false, 
                fieldName: "ClientID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ClientId);
            _fieldMetaData.Add(ClientId);

            ResidentId = new EntityDataStoreFieldMetadata<EmployeeTax, int?>(
                isPrimaryKey: false, 
                required: false, 
                isCurrency: false, 
                fieldName: "ResidentID", 
                maxLength: 0, 
                precision: 10, 
                scale: 0, 
                propertyExpression: x => x.ResidentId);
            _fieldMetaData.Add(ResidentId);

            TaxCredit = new EntityDataStoreFieldMetadata<EmployeeTax, decimal>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "TaxCredit",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.TaxCredit);
            _fieldMetaData.Add(TaxCredit);

            OtherTaxableIncome = new EntityDataStoreFieldMetadata<EmployeeTax, decimal>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "OtherTaxableIncome",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.OtherTaxableIncome);
            _fieldMetaData.Add(OtherTaxableIncome);

            WageDeduction = new EntityDataStoreFieldMetadata<EmployeeTax, decimal>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "WageDeduction",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.WageDeduction);
            _fieldMetaData.Add(WageDeduction);

            HasMoreThanOneJob = new EntityDataStoreFieldMetadata<EmployeeTax, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "HasMoreThanOneJob",
                maxLength: 0,
                precision: 3,
                scale: 0,
                propertyExpression: x => x.HasMoreThanOneJob);
            _fieldMetaData.Add(HasMoreThanOneJob);

            Using2020FederalW4Setup = new EntityDataStoreFieldMetadata<EmployeeTax, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "Using2020FederalW4Setup",
                maxLength: 0,
                precision: 3,
                scale: 0,
                propertyExpression: x => x.Using2020FederalW4Setup);
            _fieldMetaData.Add(Using2020FederalW4Setup);
        }

        // EmployeeTaxDataStoreMetadata()
        #endregion // CONSTRUCTOR
    } // class EmployeeTaxDataStoreMetadata
}