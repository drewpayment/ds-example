using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public enum RehireEligibleType
    {
        [Description("Yes")]
        Yes = 1,
        [Description("No")]
        No = 2,
        [Description("Contact HR")]
        ContactHR = 3,
        [Description("Not Specified")]
        NotSpecified = 4,
    }

    public static class RehireEligibleTypeExtensions
    {
        public static RehireEligibleType? ToNullable(RehireEligibleType input) {
            if (input == RehireEligibleType.NotSpecified) {
                return null;
            } else {
                return input;
            }
        }

        public static RehireEligibleType? ParseInputToNullable(int? input) => ToNullable(ParseInput(input));
        public static RehireEligibleType? ParseInputStringToNullable(string input) => ToNullable(ParseInputString(input));

        public static RehireEligibleType ParseInput(int? input)
        {
            if (input == null)
                return RehireEligibleType.NotSpecified;

            if (input.HasValue && Enum.IsDefined(typeof(RehireEligibleType), input.Value))
            {
                return (RehireEligibleType)input.Value;
            }

            // catchall for when invalid input...
            return RehireEligibleType.NotSpecified;
        }

        public static RehireEligibleType ParseInputString(string input)
        {
            int.TryParse(input, out int value);
            return ParseInput(value);
        }

        public static RehireEligibleType? FromRadioBooleansToNullable(bool? rehireYes, bool? rehireNo, bool? rehireContactHr) => ToNullable(FromRadioBooleans(rehireYes, rehireNo, rehireContactHr));

        public static RehireEligibleType FromRadioBooleans(bool? rehireYes, bool? rehireNo, bool? rehireContactHr)
        {
            if (!rehireYes.HasValue && !rehireNo.HasValue && !rehireContactHr.HasValue) {
                return RehireEligibleType.NotSpecified;
            }

            bool _rehireYes       = (rehireYes.HasValue       && rehireYes.Value);
            bool _rehireNo        = (rehireNo.HasValue        && rehireNo.Value);
            bool _rehireContactHr = (rehireContactHr.HasValue && rehireContactHr.Value);

            if (_rehireYes && !_rehireNo && !_rehireContactHr) {
                return RehireEligibleType.Yes;
            } else if (!_rehireYes && _rehireNo && !_rehireContactHr) {
                return RehireEligibleType.No;
            } else if (!_rehireYes && !_rehireNo && _rehireContactHr) {
                return RehireEligibleType.ContactHR;
            } else {
                return RehireEligibleType.NotSpecified;
            }
        }

        public static void ToRadioBooleans( RehireEligibleType? value, 
                                            System.Web.UI.WebControls.RadioButton rehireYes, 
                                            System.Web.UI.WebControls.RadioButton rehireNo, 
                                            System.Web.UI.WebControls.RadioButton rehireContactHr)
        {
            rehireYes.Checked       = (value.HasValue && value.Value == RehireEligibleType.Yes);
            rehireNo.Checked        = (value.HasValue && value.Value == RehireEligibleType.No);
            rehireContactHr.Checked = (value.HasValue && value.Value == RehireEligibleType.ContactHR);
        }
    }
}
