using System;
using System.IO;
using System.Linq;

using iTextSharp.text.pdf;

namespace Dominion.Utility.Pdf
{
    public abstract class iTextPdfForm : IPdf
    {
        protected abstract IPdfTemplateProvider FormTemplateProvider { get; }

        protected abstract void FillForm(IPdfFormFiller formFiller);

        public void AddToBuilder(IPdfBuilder builder, params int[] pages)
        {
            var iTextBuilder = builder as iTextPdfBuilder;
            if(iTextBuilder == null)
                throw new NotSupportedException("PDFs generated from different PDF libraries is not supported.");

            using (var templateStream = FormTemplateProvider.GetPdfStream())
            {
                var templateReader = new PdfReader(templateStream);
                // Prevents Adobe user prompt
                templateReader.RemoveUsageRights();

                using (var formStream = new MemoryStream())
                { 
                    // create stamper used to fill the form
                    // set flattening to true so the resulting PDF will not be editable
                    var stamper = new PdfStamper(templateReader, formStream) { FormFlattening = true };

                    // use the mapping definition to fill the form
                    FillForm(stamper.AsPdfFormFiller());

                    // clean up
                    stamper.Close();
                    templateReader.Close();
                    
                    // add pages to builder 
                    var tempReader = new PdfReader(formStream.ToArray());
                    if (pages.Any())
                    {
                        // add only selected pages
                        foreach (var p in pages)
                        {
                            var page = iTextBuilder.Copier.GetImportedPage(tempReader, p);
                            iTextBuilder.Copier.AddPage(page);
                        }
                    }
                    else
                    {
                        // add all pages
                        for (var p = 1; p <= tempReader.NumberOfPages; p++)
                        {
                            var page = iTextBuilder.Copier.GetImportedPage(tempReader, p);
                            iTextBuilder.Copier.AddPage(page);
                        }
                    }

                    iTextBuilder.Copier.FreeReader(tempReader);
                    tempReader.Close();
                }
            }
        }
    }
}
