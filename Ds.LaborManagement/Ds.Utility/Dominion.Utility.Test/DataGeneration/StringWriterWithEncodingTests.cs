using System.Text;
using Dominion.Utility.DataGeneration;
using NUnit.Framework;
using Dominion.Utility;
using Dominion.Utility.Pdf.DocxToPdf;
using System.IO;
using Dominion.Testing.Util.Common;
using System.Web.Configuration;

namespace Dominion.Utility.Test.DataGeneration
{
    [TestFixture]
    public class StringWriterWithEncodingTests
    {
        /// <summary>
        /// This will be run for ALL tests in the namespace.
        /// </summary>
        [SetUp]
        public void RunBeforeAnyTests()
        {
            Config.UseSharedTestingAppConfig();
        }

        [Test]
        public void Test_StringWriter_Has_UTF8_Encoding()
        {
            var writer = new StringWriterWithEncoding(Encoding.UTF8);
            var result = writer.Encoding;
            Assert.AreEqual(Encoding.UTF8, result);
        }

        [Test]
        public void Test_StringWriter_Has_UTF32_Encoding()
        {

            var writer = new StringWriterWithEncoding(Encoding.UTF32);
            var result = writer.Encoding;
            Assert.AreEqual(Encoding.UTF32, result);
        }

        [Test]
        public void Test_Docx_To_PDFWriter()
        {
            IDocxToPdfConverter converter = new DocxToPdfConverter();

            string inputFile = WebConfigurationManager.AppSettings["InputFile"];
            byte[] pdf = converter.Convert(File.ReadAllBytes(inputFile));
            File.WriteAllBytes(inputFile.Replace(".docx",".pdf"), pdf);
            Assert.AreNotEqual(0, pdf.Length);
        }
    }
}
