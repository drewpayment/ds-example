using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class AlertDto
    {
        public int AlertId { get; set; }
        public DateTime DatePosted { get; set; }
        public string AlertText { get; set; }
        public string AlertLink { get; set; }
        public DateTime DateStartDisplay { get; set; }
        public DateTime DateEndDisplay { get; set; }
        public int AlertType { get; set; }
        public byte SecurityLevel { get; set; }
        public int? ClientId { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public byte? AlertCategoryId { get; set; }
        public string Title { get; set; }
        public bool IsExpired { get; set; }
        public int? ResourceId { get; set; }
        //custom
        public string DatePostedCustom { get; set; }
        public string DatePostedCustom2 { get; set; }
    }
}
