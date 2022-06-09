using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using org.pdfclown.documents.interaction.forms;

namespace Dominion.Utility.Pdf
{
    public class PdfClownFormFiller : IPdfFormFiller
    {
        private readonly Form _form;
        
        public PdfClownFormFiller(Form form)
        {
            _form = form;
        }

        void IPdfFormFiller.SetField(string key, string value)
        {
            _form.Fields[key].Value = value;
        }
    }
}
