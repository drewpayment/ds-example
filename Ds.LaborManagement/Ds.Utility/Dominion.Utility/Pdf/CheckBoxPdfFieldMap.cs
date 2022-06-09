using System;

namespace Dominion.Utility.Pdf
{
    public class CheckBoxPdfFieldMap<TDto> : PdfFieldMap<TDto>
    {
        private readonly Func<TDto, bool?> _predicate; 
        private readonly string           _truthySetter;
        private readonly string           _falsySetter;
        private readonly bool             _defaultSelection;

        public CheckBoxPdfFieldMap(string key, Func<TDto, bool?> predicate, string truthySetter, string falsySetter = null, bool defaultSelection = false)
            : base(key)
        {
            _predicate        = predicate;
            _truthySetter     = truthySetter;
            _falsySetter      = falsySetter ?? "false";
            _defaultSelection = defaultSelection;
        }

        public override string GetValue(TDto obj)
        {
            var val = _predicate(obj);

            return val.GetValueOrDefault(_defaultSelection) ? _truthySetter : _falsySetter;
        }
    }
}