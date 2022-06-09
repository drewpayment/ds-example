using System.Collections.Generic;

namespace Dominion.Utility.Pdf
{
    public interface IPdfFieldMap<in TDto>
    {
        string FieldKey { get; }
        string GetValue(TDto obj);
        void FillForm(TDto obj, IPdfFormFiller filler);
        void AddFields(TDto obj, Dictionary<string, string> fields);
    }
}