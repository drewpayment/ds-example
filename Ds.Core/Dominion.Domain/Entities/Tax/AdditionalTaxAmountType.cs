using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.DataModel.Entities.Base;
using Dominion.DataModel.Interfaces;
using Dominion.Utility.Messaging;
using Dominion.Utility.Containers;

namespace Dominion.DataModel.Entities
{

/// <summary>
	/// Feature Options available to Clients
	/// </summary>
	public enum AmountType : int
	{
		None							= 0,
		Percent_Of_Rate		= 1,
		Flat_Rate					= 2,
		Percent_Of_Pay    = 3
	}
	
	// Additional Amount Type For Employee Taxes
	public class AdditionalTaxAmountType : Entity<AdditionalTaxAmountType>
	{

		public virtual AmountType								AdditionalAmountTypeId		{ get; set; }
		public virtual string										Description								{ get; set; }


		#region IValidatableObject IMPLEMENTATION

				/// <summary>
		/// Validate all properties.
		/// </summary>
		/// <param name="validationContext">The validation context.</param>
		/// <returns>Set of validation errors, if any.</returns>
		public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
		{


			var validationErrors = base.Validate(validationContext).ToList();

			// ensure that the question text includes non-whitespace chars.
			if( String.IsNullOrWhiteSpace(this.Description) )
			{
				var properties = new PropertyList<AdditionalTaxAmountType>().Include( s => s.Description );

				validationErrors.Add( new ValidationStatusMessage(EntityExtensions.MISSING_REQUIRED_VALUE_MESSAGE, 
					ValidationStatusMessageType.Required, 
					new List<string> { properties.GetPropertyName(0) }) );
			}

			// check for valid FeatureOption enum value

			validationErrors.AddRange( this.HasValidEnumValue( x => x.AdditionalAmountTypeId ) );

			return validationErrors;

		}// Validate()

		#endregion // IValidatableObject IMPLEMENTATION
	}

}
