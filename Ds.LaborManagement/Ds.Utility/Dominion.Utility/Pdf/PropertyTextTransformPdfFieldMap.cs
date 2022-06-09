using System;

using Dominion.Utility.Constants;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Pdf
{
    public class PropertyTextTransformPdfFieldMap<TDto, TProperty> : PdfFieldMap<TDto>
    {
        private readonly Func<TDto, TProperty> _property; 
        private readonly bool _isNullable;
        
        private Func<TProperty, string> _propertyTransform;  

        private string _defaultIfNull;

        public PropertyTextTransformPdfFieldMap(string key, Func<TDto, TProperty> property)
            : base(key)
        {
            _property          = property;
            _isNullable        = typeof(TProperty) == typeof(string) || typeof(TProperty).IsNullableType();
            _defaultIfNull     = CommonConstants.EMPTY_STRING;
            _propertyTransform = p => p.ToString();
        }

        public PropertyTextTransformPdfFieldMap<TDto, TProperty> UseDefaultIfNull(string defaultValue = CommonConstants.EMPTY_STRING)
        {
            _defaultIfNull = defaultValue;
            return this;
        }

        public PropertyTextTransformPdfFieldMap<TDto, TProperty> ToFormattedText(string format)
        {
            var formatToken = "{0:" + format + "}";
            _propertyTransform = p => string.Format(formatToken, p);
            return this;
        }

        public override string GetValue(TDto obj)
        {
            var propVal = _property(obj);
            return _isNullable && propVal == null ? _defaultIfNull : _propertyTransform(propVal);;
        }
    }
}