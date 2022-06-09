using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Notification
{
    public class ProductDto
    {
        public Product   ProductId { get; set; }
        public string Name      { get; set; }
        public string IconName  { get; set; }
        public IEnumerable<NotificationTypeDto> NotificationTypes { get; set; }
    }
}
