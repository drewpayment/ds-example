using Dominion.Core.Dto.Contact.Search;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewOwnerDto
    {
        public int ReviewTemplateId { get; set; }
        public int UserId { get; set; }
        public ContactSearchDto Contact { get; set; }
    }
}
