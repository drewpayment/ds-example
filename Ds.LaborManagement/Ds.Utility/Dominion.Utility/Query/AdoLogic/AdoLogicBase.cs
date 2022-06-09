using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dominion.Utility.Configs;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Query.AdoLogic
{
    public abstract class AdoLogicBase : IAdoLogic
    {
        public int? CmdTimeoutSeconds { get; protected set; }
        public string CmdText { get; protected set; }
        public CommandType CmdType { get; protected set; }
        public virtual string Messages { get; protected set; }

        public bool UseMessageEventHandler { get; protected set; }
        public bool UseDataReaderLogic { get; protected  set; }
        public bool UseDataSetLogic { get; protected  set; }

        public SqlParameter[] Parameters { get; protected set; }

        public Action<SqlDataReader> DataReaderLogic { get; protected set; }
        
        public int NonQueryResult { get; set; }
        public object Data { get; set; }

        protected AdoLogicBase()
        {
        }

        public virtual void MessageEventHandler(object sender, SqlInfoMessageEventArgs e)
        {
        }

        //public static IOpResult<TResult> ExecuteAdoLogic<TResult>(IAdoLogic adoLogic, string connStr)
        //    where TResult : class
        //{
        //    var result = new OpResult<TResult>();

        //    result.TryCatch(() =>
        //    {
        //        var ado = new ExecSqlCmd(connStr);
        //        ado.Execute(adoLogic);
        //        result.Data = adoLogic.Data as TResult;
        //    });

        //    return result;
        //}
    }

    public static class AdoLogicBaseExMeth
    {
        public static IOpResult<T> RunAuthCtx<T>(this AdoLogicBase2<T> obj, string connStr = null) where T : class
        {
            return obj.Run(connStr ?? ConfigValues.AuthCtx);
        }

        public static IOpResult<T> RunAppCtx<T>(this AdoLogicBase2<T> obj, string connStr = null) where T : class
        {
            return obj.Run(connStr ?? ConfigValues.AppData);
        }

        public static IOpResult<T> Run<T>(this AdoLogicBase2<T> obj, string connStr) where T : class
        {
            var result = new OpResult<T>();

            result.TryCatch(() =>
            {
                var ado = new ExecSqlCmd(connStr ?? ConfigValues.AppData);
                ado.Execute(obj);
                result.Data = obj.Data as T;
            });

            return result;
        }

        public static IOpResult<TResult> ExecuteAdoLogic<TResult>(this IAdoLogic adoLogic, string connStr)
            where TResult : class
        {
            var result = new OpResult<TResult>();

            result.TryCatch(() =>
            {
                var ado = new ExecSqlCmd(connStr);
                ado.Execute(adoLogic);
                result.Data = adoLogic.Data as TResult;
            });

            return result;
        }

    }

    public abstract class AdoLogicBase2<T> : IAdoLogic
            where T : class
    {
        public int? CmdTimeoutSeconds { get; protected set; }
        public string CmdText { get; protected set; }
        public CommandType CmdType { get; protected set; }
        public virtual string Messages { get; protected set; }

        public bool UseMessageEventHandler { get; protected set; }
        public bool UseDataReaderLogic { get; protected  set; }
        public bool UseDataSetLogic { get; protected  set; }

        public SqlParameter[] Parameters { get; protected set; }

        public Action<SqlDataReader> DataReaderLogic { get; protected set; }
        
        public int NonQueryResult { get; set; }
            
        public object Data { get; set; }

        protected AdoLogicBase2()
        {
        }

        public virtual void MessageEventHandler(object sender, SqlInfoMessageEventArgs e)
        {
        }

        public IOpResult<T> Execute(string connStr)
        {
            var result = new OpResult<T>();

            result.TryCatch(() =>
            {
                var ado = new ExecSqlCmd(connStr);
                ado.Execute(this);
                result.Data = this.Data as T;
            });

            return result;
        }
    }

}