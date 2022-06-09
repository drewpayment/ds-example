using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.EntityDataStore.EntityMetadata;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;

namespace Dominion.Domain.EntityMetadata.Employee
{
    public class EmployeePayDataStoreMetadata : IEntityDataStoreMetadata
    {
        #region SINGLETON INSTANCE

        private static EmployeePayDataStoreMetadata _instance = null;

        public static EmployeePayDataStoreMetadata Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeePayDataStoreMetadata();

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

        #endregion

        #region PUBLIC PROPERTIES

        public readonly EntityDataStoreFieldMetadata<EmployeePay, int> EmployeePayId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, int> EmployeeId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, PayFrequencyType> PayFrequencyId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, PayType?> Type;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, double?> ContractAmount;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, DateTime?> ContractAmountEffectiveDate;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, double?> SalaryAmount;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, DateTime?> SalaryAmountEffectiveDate;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, double?> Hours;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, EmployeeStatusType> EmployeeStatusId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, int?> ClientShiftId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, int?> ClientTaxId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsFicaExempt;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsFutaExempt;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsSutaExempt;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsIncomeTaxExempt;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsC1099Exempt;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsSocSecExempt;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool?> IsTippedEmployee;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsHireActQualified;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, double?> TempAgencyPercent;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, double?> TempAgencyPercentOtOverride;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, double?> TempAgencyPercentDtOverride;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, int> ClientId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, DateTime> HireActStartDate;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, string> SalaryNote;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, string> ContractNote;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsExcludeFromAca;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, string> AcaNote;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsArpExempt;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> StateTaxExempt;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, int?> WotcReasonId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> IsCobraParticipant;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, int?> EmployeeTerminationReasonId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, RehireEligibleType?> RehireEligibleId;
        public readonly EntityDataStoreFieldMetadata<EmployeePay, bool> DeferEESocSecTax;

        #endregion

        #region CONSTRUCTOR

        private EmployeePayDataStoreMetadata()
        {
            // set the IEntityDataStoreMetadata values.
            _dataStoreName = "EmployeePay";
            _fieldMetaData = new List<IEntityDataStoreFieldMetadata>();

            EmployeePayId = new EntityDataStoreFieldMetadata<EmployeePay, int>(
                isPrimaryKey: true,
                required: true,
                isCurrency: false,
                fieldName: "EmployeePayID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.EmployeePayId);
            _fieldMetaData.Add(EmployeePayId);

            EmployeeId = new EntityDataStoreFieldMetadata<EmployeePay, int>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "EmployeeID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.EmployeeId);
            _fieldMetaData.Add(EmployeeId);

            PayFrequencyId = new EntityDataStoreFieldMetadata<EmployeePay, PayFrequencyType>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "PayFrequencyID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.PayFrequencyId);
            _fieldMetaData.Add(PayFrequencyId);

            Type = new EntityDataStoreFieldMetadata<EmployeePay, PayType?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "Type",
                maxLength: 0,
                precision: 3,
                scale: 0,
                propertyExpression: x => x.Type);
            _fieldMetaData.Add(Type);

            ContractAmount = new EntityDataStoreFieldMetadata<EmployeePay, double?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "ContractAmount",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.ContractAmount);
            _fieldMetaData.Add(ContractAmount);

            ContractAmountEffectiveDate = new EntityDataStoreFieldMetadata<EmployeePay, DateTime?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "ContractAmountEffectiveDate",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.ContractAmountEffectiveDate);
            _fieldMetaData.Add(ContractAmountEffectiveDate);

            SalaryAmount = new EntityDataStoreFieldMetadata<EmployeePay, double?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "SalaryAmount",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.SalaryAmount);
            _fieldMetaData.Add(SalaryAmount);

            SalaryAmountEffectiveDate = new EntityDataStoreFieldMetadata<EmployeePay, DateTime?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "SalaryAmountEffectiveDate",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.SalaryAmountEffectiveDate);
            _fieldMetaData.Add(SalaryAmountEffectiveDate);

            Hours = new EntityDataStoreFieldMetadata<EmployeePay, double?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "Hours",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.Hours);
            _fieldMetaData.Add(Hours);

            EmployeeStatusId = new EntityDataStoreFieldMetadata<EmployeePay, EmployeeStatusType>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "EmployeeStatusID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.EmployeeStatusId);
            _fieldMetaData.Add(EmployeeStatusId);

            ClientShiftId = new EntityDataStoreFieldMetadata<EmployeePay, int?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "ClientShiftID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.ClientShiftId);
            _fieldMetaData.Add(ClientShiftId);

            ClientTaxId = new EntityDataStoreFieldMetadata<EmployeePay, int?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "ClientTaxID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.ClientTaxId);
            _fieldMetaData.Add(ClientTaxId);

            IsFicaExempt = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "FICAExempt",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsFicaExempt);
            _fieldMetaData.Add(IsFicaExempt);

            IsFutaExempt = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "FUTAExempt",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsFutaExempt);
            _fieldMetaData.Add(IsFutaExempt);

            IsSutaExempt = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "SUTAExempt",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsSutaExempt);
            _fieldMetaData.Add(IsSutaExempt);

            IsIncomeTaxExempt = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "IncomeTaxExempt",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsIncomeTaxExempt);
            _fieldMetaData.Add(IsIncomeTaxExempt);

            IsC1099Exempt = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "1099Exempt",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsC1099Exempt);
            _fieldMetaData.Add(IsC1099Exempt);

            IsSocSecExempt = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "SocSecExempt",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsSocSecExempt);
            _fieldMetaData.Add(IsSocSecExempt);

            IsTippedEmployee = new EntityDataStoreFieldMetadata<EmployeePay, bool?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "TippedEmployee",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsTippedEmployee);
            _fieldMetaData.Add(IsTippedEmployee);

            IsHireActQualified = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "HireActQualified",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsHireActQualified);
            _fieldMetaData.Add(IsHireActQualified);

            TempAgencyPercent = new EntityDataStoreFieldMetadata<EmployeePay, double?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "TempAgencyPercent",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.TempAgencyPercent);
            _fieldMetaData.Add(TempAgencyPercent);

            TempAgencyPercentOtOverride = new EntityDataStoreFieldMetadata<EmployeePay, double?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "TempAgencyPercentOTOverride",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.TempAgencyPercentOtOverride);
            _fieldMetaData.Add(TempAgencyPercentOtOverride);

            TempAgencyPercentDtOverride = new EntityDataStoreFieldMetadata<EmployeePay, double?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "TempAgencyPercentDTOverride",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.TempAgencyPercentDtOverride);
            _fieldMetaData.Add(TempAgencyPercentDtOverride);

            ClientId = new EntityDataStoreFieldMetadata<EmployeePay, int>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "ClientID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.ClientId);
            _fieldMetaData.Add(ClientId);

            HireActStartDate = new EntityDataStoreFieldMetadata<EmployeePay, DateTime>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "HireActStartDate",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.HireActStartDate);
            _fieldMetaData.Add(HireActStartDate);

            SalaryNote = new EntityDataStoreFieldMetadata<EmployeePay, string>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "SalaryNote",
                maxLength: 500,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.SalaryNote);
            _fieldMetaData.Add(SalaryNote);

            ContractNote = new EntityDataStoreFieldMetadata<EmployeePay, string>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "ContractNote",
                maxLength: 500,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.ContractNote);
            _fieldMetaData.Add(ContractNote);

            IsExcludeFromAca = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "ExcludeFromACA",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsExcludeFromAca);
            _fieldMetaData.Add(IsExcludeFromAca);

            AcaNote = new EntityDataStoreFieldMetadata<EmployeePay, string>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "ACANote",
                maxLength: 500,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.AcaNote);
            _fieldMetaData.Add(AcaNote);

            IsArpExempt = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "IsARPExempt",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsArpExempt);
            _fieldMetaData.Add(IsArpExempt);

            StateTaxExempt = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "StateTaxExempt",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.StateTaxExempt);
            _fieldMetaData.Add(StateTaxExempt);

            WotcReasonId = new EntityDataStoreFieldMetadata<EmployeePay, int?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "WOTCReasonID",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.WotcReasonId);
            _fieldMetaData.Add(WotcReasonId);

            IsCobraParticipant = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "CobraParticipant",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.IsCobraParticipant);
            _fieldMetaData.Add(IsCobraParticipant);

            EmployeeTerminationReasonId = new EntityDataStoreFieldMetadata<EmployeePay, int?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "EmployeeTerminationReasonId",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => x.EmployeeTerminationReasonId);
            _fieldMetaData.Add(EmployeeTerminationReasonId);

            RehireEligibleId = new EntityDataStoreFieldMetadata<EmployeePay, RehireEligibleType?>(
                isPrimaryKey: false,
                required: false,
                isCurrency: false,
                fieldName: "RehireEligibleId",
                maxLength: 0,
                precision: 10,
                scale: 0,
                propertyExpression: x => (RehireEligibleType?)x.RehireEligibleId);
            _fieldMetaData.Add(RehireEligibleId);

            DeferEESocSecTax = new EntityDataStoreFieldMetadata<EmployeePay, bool>(
                isPrimaryKey: false,
                required: true,
                isCurrency: false,
                fieldName: "DeferEESocSecTax",
                maxLength: 0,
                precision: 0,
                scale: 0,
                propertyExpression: x => x.DeferEESocSecTax);
            _fieldMetaData.Add(DeferEESocSecTax);
        }

        #endregion

        public static IEnumerable<IEntityDataStoreFieldMetadata> EmployeePayIgnoredFields()
        {
            var list = new List<IEntityDataStoreFieldMetadata>();
            var metadata = EmployeeDataStoreMetadata.Instance;



            return list;
        }
    }
}
