using System.Collections.Generic;

namespace Dominion.Utility.Pdf
{
    public abstract class PdfFieldMap<TDto> : IPdfFieldMap<TDto>
    {
        public string FieldKey { get; private set; }

        protected PdfFieldMap(string key)
        {
            FieldKey = key;
        }

        public abstract string GetValue(TDto obj);

        public virtual void FillForm(TDto obj, IPdfFormFiller filler)
        {
            filler.SetField(FieldKey, GetValue(obj));
        }

        public virtual void AddFields(TDto obj, Dictionary<string, string> fields)
        {
            fields.Add(FieldKey, GetValue(obj));
        }
    }
}