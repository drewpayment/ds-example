//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dominion.Utility.Msg.Identifiers;

//namespace Dominion.Utility.Msg.Specific
//{
//    public class NotSupportedExcelFileMsg : MsgBase<NotSupportedExcelFileMsg>
//    {
//        public string FileDescription { get; set; }

//        public NotSupportedExcelFileMsg(MsgLevels msgLevel, int code) 
//            : base(msgLevel, int.MinValue)
//        {
//        }

//        protected override string BuildMsg()
//        {
//            return "Not a valid excel format."
//        }
//    }
//}
