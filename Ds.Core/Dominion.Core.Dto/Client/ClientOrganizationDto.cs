using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientOrganizationDto
    {
        public int ClientOrganizationId { get; set; }
        public string ClientOrganizationName { get; set; }
        public int SelectedOrganizationId { get; set; }
        public List<ClientOrganizationClientDto> ClientOrganizationClient { get; set; }
        public bool IsNew { get; set; }
    }
}
