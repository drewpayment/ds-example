using System;
using System.Collections.Generic;

namespace Dominion.Utility.Pdf
{
    public class ItemPdfFieldMap<TDto, TItem> : PdfFieldMap<TDto>
    {
        private readonly Func<TDto, TItem> _itemSelector; 
        private readonly PdfFormMapper<TItem> _itemFormMapper;

        public PdfFormMapper<TItem> ItemFormMapper
        {
            get { return _itemFormMapper; }
        }

        public ItemPdfFieldMap(Func<TDto, TItem> itemSelector)
            : base(null)
        {
            _itemSelector = itemSelector;
            _itemFormMapper = new PdfFormMapper<TItem>();
        }

        public override string GetValue(TDto obj)
        {
            throw new NotImplementedException();
        }

        public override void FillForm(TDto obj, IPdfFormFiller filler)
        {
            var item = _itemSelector(obj);

            if(item != null)
                _itemFormMapper.Map(item, filler);
        }

        public override void AddFields(TDto obj, Dictionary<string, string> fields)
        {
            var item = _itemSelector(obj);

            if(item != null)
            {
                var itemFields = _itemFormMapper.GetFieldValues(item);
                foreach(var f in itemFields)
                    fields.Add(f.Key, f.Value);
            }
        }
    }
}