using System.Collections.Generic;
using System.Linq;

namespace Dominion.Core.Dto.Common
{
    public class PointDirection
    {
        public enum DirectionEnum
        {
            Ascending = 1,
            Descending = 2
        }

        public DirectionEnum Direction { get; set; }

        public PointDirection()
        {
            Direction = DirectionEnum.Ascending;
        }

        public static PointDirection SetPointDirection(IEnumerable<double> pointValues)
        {
            var pointDirection = new PointDirection();
            //account for 0
            if (!pointValues.Any())
            {
                pointDirection.Direction = DirectionEnum.Ascending;  
                return pointDirection;
            }
            
            //Add sorting?

            //account for 1 or more
            pointDirection.Direction = (pointValues.First() <= pointValues.Last())
                ? DirectionEnum.Ascending
                : DirectionEnum.Descending;

            return pointDirection;
        }

        public bool IsAscending => Direction == DirectionEnum.Ascending;
        public bool IsDescending => Direction == DirectionEnum.Descending;

    }
}
