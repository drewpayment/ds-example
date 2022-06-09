using System;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Client
{
    /// <summary>
    /// Client entity validation rules.
    /// </summary>
    public class ClientValidator : EntityValidator<Entities.Clients.Client>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public ClientValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.ClientId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();
            RuleFor(x => x.ClientName).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.AddressLine1).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.City).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.StateId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
            RuleFor(x => x.CountryId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
            RuleFor(x => x.PostalCode).NotNull().WithRequiredContext();
            RuleFor(x => x.LastModifiedByDescription).NotNull().WithRequiredContext();


            // last modified date
            DateTime minDate = MIN_REALISTIC_DATE;
            DateTime maxDate = DateTime.Now.AddDays(1.0);

            RuleFor(x => x.LastModifiedDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.LastModifiedDate != null)
                .WithOutOfRangeContext();


            // check the other date properties for realistic values.
            minDate = MIN_REALISTIC_DATE;
            maxDate = MAX_REALISTIC_DATE;

            RuleFor(x => x.AllowAccessUntilDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.AllowAccessUntilDate != null)
                .WithOutOfRangeContext();
            RuleFor(x => x.FiscalStartDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.FiscalStartDate != null)
                .WithOutOfRangeContext();
            RuleFor(x => x.PowerOfAttorneyFederalDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.PowerOfAttorneyFederalDate != null)
                .WithOutOfRangeContext();
            RuleFor(x => x.PowerOfAttorneyStateDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.PowerOfAttorneyStateDate != null)
                .WithOutOfRangeContext();
            RuleFor(x => x.StartDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.StartDate != null)
                .WithOutOfRangeContext();
            RuleFor(x => x.TaxManagementDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.TaxManagementDate != null)
                .WithOutOfRangeContext();
            RuleFor(x => x.TermDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.TermDate != null)
                .WithOutOfRangeContext();
        }

    }
}