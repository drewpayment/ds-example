using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Msg.Specific;

namespace Dominion.Utility.Messaging
{
    using Dominion.Utility.Msg;
    using Dominion.Utility.Msg.Identifiers;

    /// <summary>
    /// Defines a message that wraps another message with additional diagnostic details for an item in an import stream.
    /// </summary>
    public class ItemImportMessage : IMsgSimple
    {
        private readonly int _index;

        private readonly IMsgSimple _internal;

        public ItemImportMessage(int index, IMsgSimple @internal)
        {
            this._index = index;
            this._internal = @internal;
        }

        public ItemImportMessage(int index, string messageString)
        {
            this._index = index;
            this._internal = new GenericMsg(messageString);
        }

        public int Index => _index;

        public string Msg => $"Item {this._index + 1}: {this._internal.Msg}";

        public int Code
        {
            get
            {
                return this._internal.Code;
            }
            set
            {
                this._internal.Code = value;
            }
        }

        public MsgLevels Level
        {
            get
            {
                return this._internal.Level;
            }
            set
            {
                this._internal.Level = value;
            }
        }

        public string LevelString => this._internal.LevelString;
    }
}
