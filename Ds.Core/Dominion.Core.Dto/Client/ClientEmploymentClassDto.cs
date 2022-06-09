using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    /// <summary>
    /// Defines a DTO that is used to represent a ClientEmploymentClass entity.
    /// </summary>
    public class ClientEmploymentClassDto
    {
        public int ClientEmploymentClassId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool IsEnabled { get; set; }
    }
}
