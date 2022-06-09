using System;

namespace Dominion.Utility.Io
{
    public class TwoPartEmbeddedPath
    {
        public string NameSpace { get; private set; }
        public string FileName { get; private set; }

        public TwoPartEmbeddedPath(Type type, string fileName) 
            : this(type.Namespace, fileName)
        {
        }

        public TwoPartEmbeddedPath(string nameSpace, string fileName)
        {
            nameSpace = nameSpace.TrimEnd('.');
            nameSpace = nameSpace.Replace('-', '_');
            fileName = fileName.TrimStart('.');
            fileName = fileName.Replace('\\', '.');

            NameSpace = nameSpace;
            FileName = fileName;
        }

        public string ResourcePath
        {
            get
            {
                string resourceName = string.Format(
                    "{0}.{1}",
                    NameSpace,
                    FileName);

                return resourceName;
            }
        }
    }
}