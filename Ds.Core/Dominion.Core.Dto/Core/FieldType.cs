using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public enum FieldType : byte
    {
        Boolean = 1,
        List    = 2,
        Numeric = 3,
        Date    = 4,
        Text    = 5,
        MultipleSelection = 6,
        File                = 7,
    }

    public static class FieldTypeExtensions
    {
        /// <summary>
        /// To determine what string a question type should return see: <see href="http://techdocs.indeedeng.io/including-screener-questions/"/>
        /// </summary>
        /// <param name="type">Our system's representation of a field type</param>
        /// <returns>Indeed's representation of a field type.  This string must 
        /// match a question type in the referenced documentation.</returns>
        public static string FieldTypeToIndeedQuestionType(this FieldType type)
        {
            switch (type)
            {
                case FieldType.Boolean:
                    return "select";
                case FieldType.List:
                    return "select";
                case FieldType.MultipleSelection:
                    return "multiselect";
                case FieldType.Numeric:
                    return "text";
                case FieldType.Date:
                    return "date";
                case FieldType.Text:
                    return "text";
                case FieldType.File:
                    return "file";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
