﻿using System.IO;
using System.Text;

namespace Dominion.Utility.DataGeneration
{
    public sealed class StringWriterWithEncoding : StringWriter
    {
        public StringWriterWithEncoding(Encoding encoding)
        {
            Encoding = encoding;
        }
        public override Encoding Encoding { get; }

    }
}
