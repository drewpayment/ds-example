using System.IO;
using System.Reflection;

namespace Dominion.Utility.Io
{
    public static class EmbeddedResourceLoader
    {
        public static EmbeddedResourceLoader<T> Load<T>(T assemblyObject, string resourceName)
        {
            return new EmbeddedResourceLoader<T>(resourceName);
        }
    }

    public class EmbeddedResourceLoader<T>
    {
        private readonly Assembly _sourceAssembly;
        private readonly string _resourceName;

        public EmbeddedResourceLoader(string resourceName)
        {
            _sourceAssembly = typeof(T).Assembly;
            _resourceName = resourceName;
        }

        public Stream GetStream()
        {
            var resources = _sourceAssembly.GetManifestResourceNames();

            var stream = _sourceAssembly.GetManifestResourceStream(_resourceName);
            return stream;
        }
    }
}
