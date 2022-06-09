using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominion.Utility.Security
{
    /// <summary>
    /// Helper class used to remove ActionType access based on a set of removal rules.
    /// </summary>
    public class ActionRemover
    {
        #region VARIABLES & CONSTRUCTORS

        private readonly string _actionDesignation;
        private readonly List<Func<bool>> _removers;

        /// <summary>
        /// Instantiates a new ActionRemover instance.
        /// </summary>
        /// <param name="actionDesignation">Action designation this remover will apply removal rules to.</param>
        private ActionRemover(string actionDesignation)
        {
            _actionDesignation = actionDesignation;
            _removers = new List<Func<bool>>();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Adds the specified value as a restrictive flag to the list of removers.  
        /// </summary>
        /// <param name="remover">If true, the action will be removed.</param>
        /// <returns></returns>
        public ActionRemover RemoveWhen(Func<bool> remover)
        {
            _removers.Add(remover);
            return this;
        }

        /// <summary>
        /// Adds the specified value as a passive flag to the list of removers.
        /// </summary>
        /// <param name="allower">If true, the action will NOT be removed.</param>
        /// <returns></returns>
        public ActionRemover RemoveWhenNot(Func<bool> allower)
        {
            _removers.Add(() => !allower());
            return this;
        }

        /// <summary>
        /// Indication if any of the removal flags being tracked for the given action are marked to remove the action.
        /// </summary>
        /// <returns></returns>
        public bool ShouldRemove()
        {
            return _removers.Any(remover => remover());
        }

        /// <summary>
        /// Removes the action being tracked from the provided list of actions if a removal flag is marked to remove.
        /// </summary>
        /// <param name="actions">Actions to remove from.</param>
        public void RemoveActionFrom(Dictionary<string, ActionType> actions)
        {
            if (actions.ContainsKey(_actionDesignation) && ShouldRemove())
            {
                actions.Remove(_actionDesignation);
            }
                
        }

        #endregion

        #region STATIC MEMBERS

        /// <summary>
        /// Creates a new ActionRemover instance for an action with the provided designation.
        /// </summary>
        /// <param name="actionDesignation">Action designation to apply the removal rules to.</param>
        /// <returns></returns>
        public static ActionRemover For(string actionDesignation)
        {
            return new ActionRemover(actionDesignation);
        }

        /// <summary>
        /// Creates a new ActionRemover instance for the provided ActionType.
        /// </summary>
        /// <param name="actionType">ActionType to apply the removal rules to.</param>
        /// <returns></returns>
        public static ActionRemover For(ActionType actionType)
        {
            return new ActionRemover(actionType.Designation);
        }

        #endregion
    }
}