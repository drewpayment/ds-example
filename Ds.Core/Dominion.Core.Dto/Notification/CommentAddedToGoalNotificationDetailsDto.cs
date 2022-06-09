using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    public class CommentAddedToGoalNotificationDetailsDto
    {
        public int    GoalId           { get; set; }
        public string Title            { get; set; }
        public int    RemarkId         { get; set; }
        public string Remark           { get; set; }
        public int    AddedBy          { get; set; }
        public string AddedByFirstName { get; set; }
        public string AddedByLastName  { get; set; }
    }
}
