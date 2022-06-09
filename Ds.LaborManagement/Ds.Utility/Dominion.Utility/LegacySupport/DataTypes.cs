using System;

namespace Dominion.Utility.LegacySupport
{
    public enum LegacyDataTypes
    {
        UnKnown = 0, 
        String = 1, 
        Integer = 2, 
        Date = 3, 
        Double = 4, 
        Currency = 5, 
        Boolean = 6, 
    }

    public class DataTypes
    {
        /// <summary>
        /// This maps to the legacy enum 'DataTypes'.
        /// This is stored in the db and used to apply effect date changes.
        /// </summary>
        /// <param name="isCurrency">TRUE if the type represents a currency value.</param>
        /// <param name="dataType">The c# data type.</param>
        /// <param name="tableName">Table name where the value is stored.</param>
        /// <param name="fieldName">Field/column name where the value is stored.</param>
        /// <returns></returns>
        public static string GetLegacyDataTypeValue(bool isCurrency, Type dataType, string tableName, string fieldName)
        {
            // check for currency
            if (isCurrency)
            {
                return ((int) LegacyDataTypes.Currency).ToString(); // decimal is best use for currency by the way
            }

            // check for fields that have 'custom' data types.
            // note: return values that start with "__" are parsed as "__<table name>.<field name>".
            switch (fieldName)
            {
                case "AccountType":
                    return "**0,Savings;1,Checking";

                case "AdditionalAmountType":
                    return "**0,Additional;1,Override";

                case "ClientCostCenterID":
                    return "__ClientCostCenter.Description";

                case "ClientDepartmentID":
                    return "__ClientDepartment.Name";

                case "ClientDivisionID":
                    return "__ClientDivision.Name";

                case "ClientGroupID":
                    return "__ClientGroup.Description";

                case "ClientPlanID":
                    return "__ClientPlan.Description";

                case "ClientShiftID":
                    return "__ClientShift.Description";

                case "ClientVendorID":
                    return "__ClientVendor.Name";

                case "ClientWorkersCompID":
                    return "__ClientWorkersComp.Description";

                case "CountryID":
                    return "__Country.Name";

                case "DeductionAmountTypeID":
                    return "__EmployeeDeductionAmountType.Description";

                case "EmployeeStatusID":
                    return "__EmployeeStatus.EmployeeStatus";

                case "MaritalStatusID":
                    if( tableName.ToLower() == "employee" ) return "__MaritalStatus.MaritalStatus";
                    return "__FilingStatus.FilingStatus";

                case "MaxType":
                    return "__EmployeeDeductionMaxType.Description";

                case "PayFrequencyID":
                    return "__PayFrequency.PayFrequency";

                case "StateID":
                    return "__State.Name";

                case "CountyID":
                    return "__County.Name";

                case "IssuingStateID":
                    return "__State.Name";

                case "Type":
                    return "**1,Hourly;2,Salary";
            }

            // use the data type to determine the legacy type value.
            switch (Type.GetTypeCode(dataType))
            {
                case TypeCode.String:
                    return ((int) LegacyDataTypes.String).ToString();

                case TypeCode.Int32:
                    return ((int) LegacyDataTypes.Integer).ToString();

                case TypeCode.DateTime:
                    return ((int) LegacyDataTypes.Date).ToString();

                case TypeCode.Double:
                    return ((int) LegacyDataTypes.Double).ToString();

                    // ********
                    // 5 is currency ... don't have a currency type (see beginning of method)
                    // ********
                case TypeCode.Boolean://Changed booleans to return string.  If you sennd back boolean it shows a checkbox in change requests
                    return ((int) LegacyDataTypes.String).ToString();

                case TypeCode.Byte:
                    return ((int) LegacyDataTypes.Integer).ToString();

                default:
                    return ((int) LegacyDataTypes.UnKnown).ToString();
            }
        }
    }
}