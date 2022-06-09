using System;

namespace Dominion.Core.Dto.Client
{
    /// <summary>
    /// represents Discipline Level used to define company policies and notify when they have been reached.
    /// </summary>
    /// <remarks> contains isReadyToDelete(used to determine if the discipline level needs to be deleted) which is not saved in the data repository.</remarks>
    public partial class ClientDisciplineLevelDto
    {
        /// <summary>
        /// Uniquely Identifies Discipline Level.
        /// </summary>
        public virtual int      DisciplineLevelId { get; set; }

        /// <summary>
        /// Identifies the Client that is responsible for the Discipline Level.
        /// </summary>
        public virtual int      ClientId          { get; set; }

        /// <summary>
        /// Describes the purpose of the Discipline Level. Usually defined as an action to perform when a discipline level is reached.
        /// </summary>
        /// <example>name="warning"</example>
        public virtual string   Name              { get; set; }

        /// <summary>
        /// Describes the weight of the Discipline Level. used to calculate the order to process Discipline Levels.
        /// </summary>
        public virtual double   PointLevel        { get; set; }

        /// <summary>
        /// If checked true, a notification should be sent to inform someone that an employee has reached a certain discipline level.
        /// </summary>
        public virtual bool     IsNotify          { get; set; }

        /// <summary>
        /// maintains the order for the discipline level to help identify the direction the discipline levels are sorted in a collection.
        /// </summary>
        public virtual int      SortOrder         { get; set; }

        /// <summary>
        /// determines is a discipline level should be deleted from the data repository
        /// </summary>
        /// <remarks> This value is not stroed in the data repository. Used by client-side to communicate to client-side the action to perform when saving.</remarks>
        public virtual bool     isReadyToDelete   { get; set; }
    }
}
