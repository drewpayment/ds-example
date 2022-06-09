using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Client
{
    /// <summary>
    /// Standard Validation Rules for a ClientFeature entity.
    /// </summary>
    public class ClientFeatureValidator : EntityValidator<ClientAccountFeature>
    {
        /// <summary>
        /// Instantiates a new ClientFeatureValidator.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public ClientFeatureValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }


        /// <summary>
        /// Defines the rules for a ClientFeature entity.
        /// </summary>
        protected override void Rules()
        {
            // Keys
            RuleFor(x => x.ClientId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => x.Client == null)
                .WithRequiredContext();

            RuleFor(x => x.ClientId)
                .Equal(x => x.Client.ClientId)
                .When(x => x.Client != null)
                .WithIncompatibleContext(new[] {"Client"});

            RuleFor(x => x.AccountFeature)
                .Enum()
                .WithOutOfRangeContext();

            // Required Properties
            RuleFor(x => x.ModifiedBy).NotEmpty().WithRequiredContext().NotNull().WithRequiredContext();
        }

    }
}