using System;

namespace Dominion.Utility.Mapping.DataReader
{
    /// <summary>
    /// Used by <see cref="PropertyMapper"/> to define field names that do not match the property name.
    /// </summary>
    /// <remarks>
    /// Modified From: http://www.codeproject.com/Articles/674419/A-propertymapping-Extension-for-DataReaders
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FieldNameAttribute : Attribute
    {
        private readonly string _fieldName;

        public string FieldName
        {
            get { return this._fieldName; }
        }

        public FieldNameAttribute(string fieldName)
        {
            this._fieldName = fieldName;
        }
    }
}