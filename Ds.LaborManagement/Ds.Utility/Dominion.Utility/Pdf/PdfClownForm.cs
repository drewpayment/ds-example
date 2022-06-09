using System;
using System.Linq;

using org.pdfclown.tools;

namespace Dominion.Utility.Pdf
{
    public abstract class PdfClownForm : IPdf
    {
        protected abstract IPdfTemplateProvider FormTemplateProvider { get; }

        protected abstract void FillForm(IPdfFormFiller formFiller);

        void IPdf.AddToBuilder(IPdfBuilder builder, params int[] pages)
        {
            var pdfClownBuilder = builder as PdfClownBuilder;
            if(pdfClownBuilder == null)
                throw new NotSupportedException("PDFs generated from different PDF libraries is not supported.");

            using (var stream = FormTemplateProvider.GetPdfStream())
            using (var formFile = new org.pdfclown.files.File(stream))
            {
                FillForm(formFile.Document.Form.AsPdfFormFiller());

                var flattener = new FormFlattener();
                flattener.Flatten(formFile.Document);

                var pagesToAdd = formFile.Document.Pages.Where(p => pages == null || pages.Contains(p.Number)).ToList();
                pdfClownBuilder.Manager.Add(pagesToAdd);
            }
        }
    }
}
