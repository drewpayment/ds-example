using Omu.ValueInjecter;

namespace Dominion.Utility.Dto
{
    /// <summary>
    /// Value injection scheme that involves prefixing properties with a token prefix.
    /// </summary>
    public class VariablePrefixedPropertyConventionInjection : ConventionInjection
    {
        #region Variables and Properties

        /// <summary>
        /// The prefix.
        /// </summary>
        private string _prefix;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public VariablePrefixedPropertyConventionInjection()
        {
        }

        /// <summary>
        /// Used to specify the prefix and create instance.
        /// This will then be passed in to the InjectFrom method and used for value matching during the mapping process.
        /// </summary>
        /// <param name="prefix"></param>
        public VariablePrefixedPropertyConventionInjection(string prefix)
        {
            _prefix = prefix;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The rule.
        /// </summary>
        /// <param name="c">Method override that contains the custom rules for value matching.</param>
        /// <returns></returns>
        protected override bool Match(ConventionInfo c)
        {
            // check for a target prop that starts with the prefix.
            if (c.TargetProp.Name.StartsWith(_prefix))
            {
                // remove the prefix from the target prop name and to check against the source prop.
                // at this point we already matched the prefix, now we need to find the right target prop to fill with the source.
                var derivedName = c.TargetProp.Name.Replace(_prefix, string.Empty);
                var result = c.SourceProp.Name == derivedName;

                // if true we found our match, return true signaling the 
                // value injector to use the source value on the target.
                if (result)
                    return true;
            }

            // not a match
            return false;
        }

      #endregion
    }
}