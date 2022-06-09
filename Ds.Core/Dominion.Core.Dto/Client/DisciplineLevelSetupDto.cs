using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Common;

namespace Dominion.Core.Dto.Client
{
    /// <summary>
    /// Contains the Data needed to Setup Discipline Levels.
    /// </summary>
    public class DisciplineLevelSetupDto
    {
        /// <summary>
        /// Collection of <see cref="ClientDisciplineLevelDto"/> to load.
        /// </summary>
        public virtual IEnumerable<ClientDisciplineLevelDto> DisciplineLevelDtos { get; set; }

        /// <summary>
        /// Determines the direction the Collection should be displayed. see <see cref="PointDirection"/>
        /// </summary>
        public virtual PointDirection PointDirection { get; set; } 
    }
}
