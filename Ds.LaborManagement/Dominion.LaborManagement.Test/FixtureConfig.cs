using Dominion.Core.Services.Mapping;
using Dominion.Testing.Util.Common;
using Dominion.Utility.Io;

using NUnit.Framework;

namespace Dominion.LaborManagement.Test
{
    /// <summary>
    /// see: http://www.nunit.org/index.php?p=setupFixture&r=2.4
    /// </summary>
    [SetUpFixture]
    public class FixtureConfig
    {
        /// <summary>
        /// This will be run for ALL tests in the namespace.
        /// </summary>
        [SetUp]
        public void RunBeforeAnyTests()
        {
            Config.UseSharedTestingAppConfig();
            CoreAutoMapperManager.ConfigureMappings();
        }

        public static EmbeddedFileResource ForThisAssembly(string projectFilePath)
        {
            var res = new EmbeddedFileResource(typeof(FixtureConfig), projectFilePath);
            return res;
        }
    }
}
