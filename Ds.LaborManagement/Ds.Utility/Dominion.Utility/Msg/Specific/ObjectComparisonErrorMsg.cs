using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Msg.Identifiers;
using Newtonsoft.Json.Serialization;

namespace Dominion.Utility.Msg.Specific
{
    public class ObjectComparisonErrorMsg<T> : MsgBase<BasicExceptionMsg>
        where T : class
    {
        public Type TypeToCompare { get; set; }
        public object OneValue  { get; set; }
        public object TwoValue { get; set; }
        public string PropertyName { get; set; }

        public ObjectComparisonErrorMsg(object one, object two, string propName)
            : base(MsgLevels.Fatal, MsgCodes.Information)
        {
            TypeToCompare = typeof(T);
            OneValue = one;
            TwoValue = two;
            PropertyName = propName;
        }

        protected override string BuildMsg()
        {
            return BuildPropertyValuesDoNotMatchMessage();
        }

        private string BuildPropertyValuesDoNotMatchMessage()
        {
            var msg = string.Format(
                "[Type:]{0}{4}[Property:]{1}{4}[Values:] [{2}] - [{3}]", 
                /*0*/TypeToCompare.Name, 
                /*1*/PropertyName, 
                /*2*/(OneValue != null) ? OneValue.ToString() : "null", 
                /*3*/(TwoValue != null) ? TwoValue.ToString() : "null", 
                /*4*/Environment.NewLine);

            return msg;
        }
    }
}
