using System.Data.SqlClient;
using System.Text;

namespace Dominion.Utility.Query.AdoLogic
{
    public abstract class AdoLogicWithMessgesBase : AdoLogicBase
    {
        private readonly StringBuilder _sb;
        
        public override string Messages
        {
            get { return _sb.ToString(); }
        }

        protected AdoLogicWithMessgesBase()
        {
            _sb = new StringBuilder();
        }

        public override void MessageEventHandler(object sender, SqlInfoMessageEventArgs e)
        {
            _sb.AppendLine(e.Message);
        }
    }
}