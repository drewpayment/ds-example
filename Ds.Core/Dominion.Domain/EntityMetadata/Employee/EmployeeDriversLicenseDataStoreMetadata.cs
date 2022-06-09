using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.EntityDataStore.EntityMetadata;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;

namespace Dominion.Domain.EntityMetadata.Employee
{
   public class EmployeeDriversLicenseDataStoreMetadata : IEntityDataStoreMetadata
	{
		#region SINGLETON INSTANCE

		private static EmployeeDriversLicenseDataStoreMetadata _instance = null;

		public static EmployeeDriversLicenseDataStoreMetadata Instance
		{
			get
			{
				if(_instance == null)
					_instance = new EmployeeDriversLicenseDataStoreMetadata();
				
				return _instance;
			}
		}

		#endregion

		#region VARIABLES

		private string _dataStoreName;
		private List<IEntityDataStoreFieldMetadata> _fieldMetaData;

		public string DataStoreName { get{ return _dataStoreName;}  }
		public IEnumerable<IEntityDataStoreFieldMetadata> FieldMetaData { get{ return _fieldMetaData;} }	

		public readonly EntityDataStoreFieldMetadata<EmployeeDriversLicense, int> EmployeeId;
		public readonly EntityDataStoreFieldMetadata<EmployeeDriversLicense, int?> IssuingStateId;
		public readonly EntityDataStoreFieldMetadata<EmployeeDriversLicense, string> DriversLicenseNumber;
		public readonly EntityDataStoreFieldMetadata<EmployeeDriversLicense, DateTime?> ExpirationDate;
		public readonly EntityDataStoreFieldMetadata<EmployeeDriversLicense, DateTime> Modified;
		public readonly EntityDataStoreFieldMetadata<EmployeeDriversLicense, int> ModifiedBy;
		public readonly EntityDataStoreFieldMetadata<EmployeeDriversLicense, int> ClientId;
		
		#endregion

		#region CONSTRUCTOR

		internal EmployeeDriversLicenseDataStoreMetadata()
		{
			_dataStoreName    = "EmployeeDriversLicense";
			_fieldMetaData    = new List<IEntityDataStoreFieldMetadata>();

			EmployeeId = new EntityDataStoreFieldMetadata<EmployeeDriversLicense, int>(
				isPrimaryKey:        true,
				required:            true,
				isCurrency:          false,
				fieldName:           "EmployeeID",
				maxLength:           0,
				precision:           10,
				scale:               0,
				propertyExpression:  x => x.EmployeeId);
			
			_fieldMetaData.Add(EmployeeId);

			IssuingStateId = new EntityDataStoreFieldMetadata<EmployeeDriversLicense, int?>(
				isPrimaryKey:        false,
				required:            true,
				isCurrency:          false,
				fieldName:           "IssuingStateID",
				maxLength:           0,
				precision:           10,
				scale:               0,
				propertyExpression:  x => x.IssuingStateId);
			
			_fieldMetaData.Add(IssuingStateId);

			DriversLicenseNumber = new EntityDataStoreFieldMetadata<EmployeeDriversLicense, string>(
				isPrimaryKey:        false,
				required:            true,
				isCurrency:          false,
				fieldName:           "DriversLicenseNumber",
				maxLength:           16,
				precision:           0,
				scale:               0,
				propertyExpression:  x => x.DriversLicenseNumber);
			
			_fieldMetaData.Add(DriversLicenseNumber);

			ExpirationDate = new EntityDataStoreFieldMetadata<EmployeeDriversLicense, DateTime?>(
				isPrimaryKey:        false,
				required:            false,
				isCurrency:          false,
				fieldName:           "ExpirationDate",
				maxLength:           0,
				precision:           0,
				scale:               0,
				propertyExpression:  x => x.ExpirationDate);
			
			_fieldMetaData.Add(ExpirationDate);

			Modified = new EntityDataStoreFieldMetadata<EmployeeDriversLicense, DateTime>(
				isPrimaryKey:        false,
				required:            true,
				isCurrency:          false,
				fieldName:           "Modified",
				maxLength:           0,
				precision:           0,
				scale:               0,
				propertyExpression:  x => x.Modified);
			
			_fieldMetaData.Add(Modified);

			ModifiedBy = new EntityDataStoreFieldMetadata<EmployeeDriversLicense, int>(
				isPrimaryKey:        false,
				required:            true,
				isCurrency:          false,
				fieldName:           "ModifiedBy",
				maxLength:           0,
				precision:           10,
				scale:               0,
				propertyExpression:  x => x.ModifiedBy);
			
			_fieldMetaData.Add(ModifiedBy);

			ClientId = new EntityDataStoreFieldMetadata<EmployeeDriversLicense, int>(
				isPrimaryKey:        false,
				required:            true,
				isCurrency:          false,
				fieldName:           "ClientID",
				maxLength:           0,
				precision:           10,
				scale:               0,
				propertyExpression:  x => x.ClientId);
			
			_fieldMetaData.Add(ClientId);

		}

         /// <summary>
        /// Get a list of fields to ignore for employee contact information.
        /// These are fields that aren't noted in when working with employee contact information.
        /// </summary>
        /// <returns>List of fields to ignore for employee contact/</returns>
        public static IEnumerable<IEntityDataStoreFieldMetadata> EmployeeDriversLicenseIgnoredFields()
        {
            var list = new List<IEntityDataStoreFieldMetadata>();
            var metadata = EmployeeDriversLicenseDataStoreMetadata.Instance;

            list.Add(metadata.ClientId);
            list.Add(metadata.EmployeeId);
            list.Add(metadata.Modified);
            list.Add(metadata.ModifiedBy);
           
            return list;
        }

         /// <summary>
        /// Get the fields that are needed for employee contact data.
        /// </summary>
        /// <returns>Fields for employee contact data.</returns>
        public static List<IEntityDataStoreFieldMetadata> EmployeeDriversLicenseFields()
        {
            var toIgnore = EmployeeDriversLicenseIgnoredFields();

            var toKeep = EmployeeDriversLicenseDataStoreMetadata.Instance.FieldMetaData
                .Where(fmd => !toIgnore.Any(ignoredItems => ignoredItems.DataStoreFieldName == fmd.DataStoreFieldName));

            return toKeep.ToList();
        }
		#endregion
	}
}
