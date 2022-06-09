using System.Collections.Generic;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.EntityDataStore.EntityMetadata;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;
using Dominion.Domain.Entities.Onboarding;
using System;

namespace Dominion.Domain.EntityMetadata.Employee
{
    public class EmployeeW2ConsentDataStoreMetadata : IEntityDataStoreMetadata
    {
        #region SINGLETON INSTANCE

        private static EmployeeW2ConsentDataStoreMetadata _instance = null;

        public static EmployeeW2ConsentDataStoreMetadata Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeeW2ConsentDataStoreMetadata();

                return _instance;
            }
        }

        #endregion

        #region VARIABLES

        private string _dataStoreName;
        private List<IEntityDataStoreFieldMetadata> _fieldMetaData;

        public string DataStoreName { get { return _dataStoreName; } }
        public IEnumerable<IEntityDataStoreFieldMetadata> FieldMetaData { get { return _fieldMetaData; } }

        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, int> EmployeeW2ConsentId;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, int> EmployeeId;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, DateTime?> ConsentDate;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, DateTime?> WithdrawalDate;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, DateTime> TaxYear;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, string> PrimaryEmailAddress;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, string> SecondaryEmailAddress;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, DateTime> Modified;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, int> ModifiedBy;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, bool> IsEmailVerified;
        public readonly EntityDataStoreFieldMetadata<EmployeeW2Consent, int> ClientId;

        #endregion

        #region CONSTRUCTOR

        internal EmployeeW2ConsentDataStoreMetadata()
        {
            _dataStoreName = "EmployeeW2Consent";
            _fieldMetaData = new List<IEntityDataStoreFieldMetadata>();

            EmployeeW2ConsentId = new EntityDataStoreFieldMetadata<EmployeeW2Consent, int>(
                isPrimaryKey: true,
                required: true,
                isCurrency: false,
                fieldName: "EmployeeW2ConsentID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.EmployeeW2ConsentId);

            _fieldMetaData.Add(EmployeeW2ConsentId);

            EmployeeId = new EntityDataStoreFieldMetadata<EmployeeW2Consent, int>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "EmployeeID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.EmployeeId);

            _fieldMetaData.Add(EmployeeId);

            ConsentDate = new EntityDataStoreFieldMetadata<EmployeeW2Consent, DateTime?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "ConsentDate",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.ConsentDate);

            _fieldMetaData.Add(ConsentDate);

            WithdrawalDate = new EntityDataStoreFieldMetadata<EmployeeW2Consent, DateTime?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "WithdrawalDate",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.WithdrawalDate);

            _fieldMetaData.Add(WithdrawalDate);

            TaxYear = new EntityDataStoreFieldMetadata<EmployeeW2Consent, DateTime>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "TaxYear",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.TaxYear);

            _fieldMetaData.Add(TaxYear);

            PrimaryEmailAddress = new EntityDataStoreFieldMetadata<EmployeeW2Consent, string>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "PrimaryEmailAddress",
                maxLength: 50,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.PrimaryEmailAddress);

            _fieldMetaData.Add(PrimaryEmailAddress);

            SecondaryEmailAddress = new EntityDataStoreFieldMetadata<EmployeeW2Consent, string>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "SecondaryEmailAddress",
                maxLength: 50,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.SecondaryEmailAddress);

            _fieldMetaData.Add(SecondaryEmailAddress);

            Modified = new EntityDataStoreFieldMetadata<EmployeeW2Consent, DateTime>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "Modified",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.Modified);

            _fieldMetaData.Add(Modified);

            ModifiedBy = new EntityDataStoreFieldMetadata<EmployeeW2Consent, int>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "ModifiedBy",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.ModifiedBy);

            _fieldMetaData.Add(ModifiedBy);

            IsEmailVerified = new EntityDataStoreFieldMetadata<EmployeeW2Consent, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "EmailVerified",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsEmailVerified);

            _fieldMetaData.Add(IsEmailVerified);

            ClientId = new EntityDataStoreFieldMetadata<EmployeeW2Consent, int>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "ClientID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.ClientId);

            _fieldMetaData.Add(ClientId);

        }

        #endregion
    }
}
