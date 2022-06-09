using System.Collections.Generic;
using System.Linq;

namespace Dominion.Domain.Entities.Tax
{
    /// <summary>
    /// Entity used to group Tax Types.
    /// </summary>
    public class TaxGroup
    {
        public string TaxGroupCode { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Container for various pre-defined tax groups.
    /// </summary>
    public static class TaxGroups
    {
        /// <summary>
        /// Code: "F" | Federal tax group. 
        /// </summary>
        public static readonly TaxGroup Federal = new TaxGroup {TaxGroupCode = "F", Description = "Federal"};

        /// <summary>
        /// Code: "S" | State tax group. 
        /// </summary>
        public static readonly TaxGroup State = new TaxGroup {TaxGroupCode = "S", Description = "State"};

        /// <summary>
        /// Code: "L" | Local tax group. 
        /// </summary>
        public static readonly TaxGroup Local = new TaxGroup {TaxGroupCode = "L", Description = "Local"};

        /// <summary>
        /// Code: "U" | Unemployment tax group. (Currently used only for SUTA)
        /// </summary>
        public static readonly TaxGroup Unemployment = new TaxGroup {TaxGroupCode = "U", Description = "Unemployment"};

        /// <summary>
        /// Code: "D" | Disability tax group. 
        /// </summary>
        public static readonly TaxGroup Disability = new TaxGroup {TaxGroupCode = "D", Description = "Disability"};

        /// <summary>
        /// Code: "E" | Employer Paid tax group
        /// </summary>
        public static readonly TaxGroup EmployerPaid = new TaxGroup {TaxGroupCode = "E", Description = "Employer Paid"};

        /// <summary>
        /// Collection of currently defined tax groups. 
        /// Currently contains Federal, State, Local, Unemployement & Disability groups.
        /// </summary>
        public static readonly IEnumerable<TaxGroup> All = new[] {Federal, State, Local, Unemployment, Disability};

        /// <summary>
        /// Returns the Tax Group that matches the specified code. An empty or null code is currently grouped as a
        /// Federal Tax.
        /// </summary>
        /// <param name="taxGroupCode">Code to get the tax group for.</param>
        /// <returns></returns>
        public static TaxGroup GetByCode(string taxGroupCode)
        {
            if (string.IsNullOrEmpty(taxGroupCode))
                return TaxGroups.Federal;

            return TaxGroups.All.FirstOrDefault(x => x.TaxGroupCode == taxGroupCode);
        }
    }
}