using Dominion.Core.Dto.Sprocs;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.EF.Sprocs
{
    public class GetClockClientNoteListSproc : SprocBase<IEnumerable<GetClockClientNoteListResultDto>, GetClockClientNoteListArgsDto>
    {

        public GetClockClientNoteListSproc(string connStr, GetClockClientNoteListArgsDto args)
            : base(connStr, args)
        {
        }
        public override IEnumerable<GetClockClientNoteListResultDto> Execute()
        {
            return ExecuteSproc("spGetClockClientNoteList", reader =>
            {
                var result = new List<GetClockClientNoteListResultDto>();
                while (reader.Read())
                {
                    result.Add(new GetClockClientNoteListResultDto()
                    {
                        ClockClientNoteID = (int)reader["ClockClientNoteID"],
                        isActive = (bool)reader["isActive"],
                        Note = (string)reader["Note"]
                    });
                }
                return result;
            });
        }
    }
}
