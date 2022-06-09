using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominion.Utility.Pdf
{
    /// <summary>
    /// Base PDF form mapper. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PdfFormMapper<T> : IPdfFormMapper<T>
    {
        private readonly List<IPdfFieldMap<T>> _fieldMaps; 

        public PdfFormMapper()
        {
            _fieldMaps = new List<IPdfFieldMap<T>>(); 
        }

        public void Map(T obj, IPdfFormFiller filler)
        {
            if (obj != null)
            {
                foreach (var field in _fieldMaps)
                {
                    field.FillForm(obj, filler);
                }
            }
        }

        public IDictionary<string, string> GetFieldValues(T obj)
        {
            var fields = new Dictionary<string, string>();

            if (obj != null)
            {
                foreach (var field in _fieldMaps)
                {
                    field.AddFields(obj, fields);
                }
            }

            return fields;
        }

        public TextPdfFieldMap<T> MapToText(Func<T, string> builder, string field)
        {
            return AddFieldMap(new TextPdfFieldMap<T>(field, builder));
        }

        public PropertyTextTransformPdfFieldMap<T, TProperty> MapFromProperty<TProperty>(Func<T, TProperty> property, string field)
        {
            return AddFieldMap(new PropertyTextTransformPdfFieldMap<T, TProperty>(field, property));
        }

        public CheckBoxPdfFieldMap<T> MapToCheckBox(Func<T, bool?> builder, string field, string truthyValue, string falsyValue = null, bool defaultSelection = false)
        {
            return AddFieldMap(new CheckBoxPdfFieldMap<T>(field, builder, truthyValue, falsyValue, defaultSelection));
        }

        public ItemPdfFieldMap<T, TItem> MapItem<TItem>(Func<T, TItem> itemSelector, Action<PdfFormMapper<TItem>> config)
        {
            var fieldMap = new ItemPdfFieldMap<T, TItem>(itemSelector);
            var mapper = fieldMap.ItemFormMapper;
            config(mapper);

            return AddFieldMap(fieldMap);
        }

        public ItemPdfFieldMap<T, TItem> MapItem<TItem>(Func<T, IEnumerable<TItem>> itemSelector, int itemIndex, Action<PdfFormMapper<TItem>> config)
        {
            return MapItem(x => itemSelector(x) == null ? default(TItem) : itemSelector(x).ElementAtOrDefault(itemIndex), config);
        }

        private TMap AddFieldMap<TMap>(TMap fieldMap) where TMap : IPdfFieldMap<T>
        {
            _fieldMaps.Add(fieldMap);
            return fieldMap;
        }
    }
}