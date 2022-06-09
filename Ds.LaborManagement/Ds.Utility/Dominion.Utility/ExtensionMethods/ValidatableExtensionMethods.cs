using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dominion.Utility.Messaging;

namespace Dominion.Utility.ExtensionMethods
{
    public static class ValidatableExtensionMethods
    {
        public static void PopulateWithValidationResults(
            this IValidationStatusMessageList messages, 
            IEnumerable<ValidationResult> validationResults)
        {
            // Add validation errors to message list
            foreach (var vResult in validationResults)
            {
                IValidationStatusMessage vMessage;
                if (vResult is IValidationStatusMessage)
                    vMessage = vResult as IValidationStatusMessage;
                else
                    vMessage = new ValidationStatusMessage(vResult.ErrorMessage, vResult.MemberNames);

                messages.Add(vMessage);
            }
        }
    }
}