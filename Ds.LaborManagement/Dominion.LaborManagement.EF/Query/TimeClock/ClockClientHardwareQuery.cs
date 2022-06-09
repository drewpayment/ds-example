using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Query;

namespace Dominion.Core.EF.Query
{
    public class ClockClientHardwareQuery : Query<ClockClientHardware, IClockClientHardwareQuery>, IClockClientHardwareQuery
    {
        public ClockClientHardwareQuery(IEnumerable<ClockClientHardware> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        public IClockClientHardwareQuery ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId.Equals(clientId));
            return this;
        }

        public IClockClientHardwareQuery ByClockClientHardwareId(int clockClientHardwareId)
        {
            FilterBy(x => x.ClockClientHardwareId.Equals(clockClientHardwareId));
            return this;
        }
    }
}