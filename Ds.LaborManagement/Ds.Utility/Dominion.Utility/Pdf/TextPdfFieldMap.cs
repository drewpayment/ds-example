using System;

using Dominion.Utility.Constants;

namespace Dominion.Utility.Pdf
{
    public class TextPdfFieldMap<TDto> : PdfFieldMap<TDto>
    {
        private readonly Func<TDto, string> _fieldBuilder; 
        private string _defaultIfNull;

        public TextPdfFieldMap(string key, Func<TDto, string> fieldBuilder, string defaultIfNull = CommonConstants.EMPTY_STRING)
            : base(key)
        {
            _fieldBuilder = fieldBuilder;
            _defaultIfNull = defaultIfNull;
        }

        public TextPdfFieldMap<TDto> UseDefaultIfNull(string defaultValue = CommonConstants.EMPTY_STRING)
        {
            _defaultIfNull = defaultValue;
            return this;
        }

        public override string GetValue(TDto obj)
        {
            return _fieldBuilder(obj) ?? _defaultIfNull;
        }
    }
}