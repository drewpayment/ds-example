using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Web.DsMvc
{
    [Serializable]
    public class ModelMsgBase
    {
        public Dictionary<string, string> Errors { get; set; }

        public Dictionary<string, string> Infos { get; set; }

        public void AddError(string key, string msg)
        {
            Errors = Errors ?? new Dictionary<string, string>();

            if(!string.IsNullOrEmpty(msg))
                Errors.Add(key, msg);
        }

        public void AddInfo(string key, string msg)
        {
            Infos = Infos ?? new Dictionary<string, string>();
            Infos.Add(key, msg);
        }

        public bool HasError(string key = null)
        {
            //review: jay: use the new ? null check
            if (Errors != null)
            {
                if (key == null)
                    return Errors.Count > 0;

                return Errors.ContainsKey(key);
            }

            return false;
        }

        public bool HasInfos(string key = null)
        {
            //review: jay: use the new ? null check
            if (Infos != null)
            {
                if (key == null)
                    return Infos.Count > 0;

                return Infos.ContainsKey(key);
            }

            return false;
        }

        public bool HasAnyMsg()
        {
            return HasError() || HasInfos();
        }

        public void AddMessage(MsgLevels msgLevel, string key, string msg)
        {
            switch (msgLevel)
            {
                case MsgLevels.Info:
                    AddInfo(key, msg);
                    break;
                case MsgLevels.Error:
                    AddError(key, msg);
                    break;
                case MsgLevels.Warn:
                case MsgLevels.Debug:
                case MsgLevels.Fatal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(msgLevel), msgLevel, null);
            }
        }

        public IEnumerable<string> GetErrors()
        {
            return Errors?.Values.ToList() ?? new List<string>();
        }

        public string GetError(string key = null)
        {
            if (string.IsNullOrEmpty(key))
                return Errors?.FirstOrDefault().Value ?? string.Empty;

            return Errors?.ContainsKey(key) ?? false ? Errors[key] : null;
        }

        public string GetInfo(string key = null)
        {
            if (string.IsNullOrEmpty(key))
                return Infos?.FirstOrDefault().Value ?? string.Empty;

            return Infos?.ContainsKey(key ?? string.Empty) ?? false ? Infos[key] : null;
        }

        public ModelMsgBase ClearMessages()
        {
            Errors?.Clear();
            Infos?.Clear();
            return this;
        }

        //public string ErrorMsg { get; set; }

        //public string InfoMsg { get; set; }

    }

    
}
