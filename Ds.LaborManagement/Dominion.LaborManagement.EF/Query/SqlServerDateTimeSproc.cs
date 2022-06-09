using System;
using System.Linq;
using System.Linq.Expressions;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    internal class SqlServerDateTimeSproc : SprocBase<SqlServerTimeDto, SqlServerDateTimeSproc.Args>
    {
        /// <summary>
        /// Name of sproc: "[dbo].[spGetSqlServerDateTime]"
        /// </summary>
        public const string SprocName = "[dbo].[spGetSqlServerDateTime]";
       
        public SqlServerDateTimeSproc(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override SqlServerTimeDto Execute()
        {
            var dto = ExecuteSproc(SprocName, dr =>
            {
                var result = dr.AsEnumerable<ResultDto>().FirstOrDefault();

                var resultDto = Mapper.Instance.Map(result);

                return resultDto;
            });

            return dto;
        }

        /// <summary>
        /// Internal class for creating arguments for the stored procedure call. 
        /// This stored procedure does not have any parameteres
        /// </summary>
        internal class Args : SprocParametersBase
        {
            //call has no Args 
        }

        /// <summary>
        /// Mapper that converts ResultDto into Dto
        /// </summary>
        internal class Mapper : ExpressionMapper<ResultDto, SqlServerTimeDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();
            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, SqlServerTimeDto>> MapExpression
            {
                get
                {
                    return x => new SqlServerTimeDto()
                    {
                        ServerTime = x.ServerTime
                    };
                }
            }
        }

        /// <summary>
        /// Internal class that provides a 1-to-1 mapping for the SP result dataset
        /// </summary>
        internal class ResultDto
        {
            public DateTime ServerTime { get; set; }
        }
    }
}