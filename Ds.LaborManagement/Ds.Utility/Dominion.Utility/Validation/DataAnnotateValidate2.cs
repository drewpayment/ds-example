﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Validation
{
    public class DataAnnotateValidate2
    {

        public static IEnumerable<SinglePropValidationDto> Validate(object obj, string propName, string displayName = null, int id = int.MinValue)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var vResults = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, vResults, true);

            var list = new List<SinglePropValidationDto>();

            if (vResults.Any(x => x.MemberNames.Contains(propName)))
            {
                foreach (var vr in vResults)
                {
                    if (vr.MemberNames.Contains(propName))
                    {
                        var d = new SinglePropValidationDto();
                        d.Id = id;
                        d.DisplayName = displayName;
                        d.PropertyName = propName;
                        d.Message = vr.ErrorMessage;
                        d.HasError = true;

                        list.Add(d);
                    }
                }
            }
            else
            {
                var d = new SinglePropValidationDto();
                d.Id = id;
                d.DisplayName = displayName;
                d.PropertyName = propName;
                list.Add(d);
            }

            return list;
        }


        public static IOpResult<IEnumerable<string>> ValidateCondensed(object obj, string separator)
        {
            var r = new OpResult<IEnumerable<string>>();
            var results = Validate(obj).MergeInto(r);

            var list = new List<string>();
            foreach (var item in CondenseMessages(results.Data))
            {
                var msg = $"{item.Msg.TrimEnd('.')}: {item.Fields.ToSeparatedList(separator)}";
                list.Add(msg);
            }

            r.Data = list;
            return r;
        }

        /// <summary>
        /// Validate the object and condense the error messages.
        /// Condensing the messages means we take the validation results and get a distinct list of errors and the fields associated with those errors.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IOpResult<IEnumerable<CondensedErrorMsg>> ValidateCondensed(object obj)
        {
            var r = new OpResult<IEnumerable<CondensedErrorMsg>>();
            var results = Validate(obj).MergeInto(r);
            r.Data = CondenseMessages(results.Data);
            return r;
        }

        /// <summary>
        /// Validate the object and return the default results.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IOpResult<ICollection<ValidationResult>> Validate(object obj)
        {
            var r = new OpResult<ICollection<ValidationResult>>();
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            r.Data = new List<ValidationResult>();

            Validator.TryValidateObject(obj, context, r.Data, true);

            r.EvaluateSuccess(!r.Data.Any());
            return r;
        }


        /// <summary>
        /// Take the validation results and get a distinct list of errors and the fields associated with those errors.
        /// </summary>
        /// <param name="results">Data Annotation validation results.</param>
        /// <returns></returns>
        public static IEnumerable<CondensedErrorMsg> CondenseMessages(ICollection<ValidationResult> results)
        {
            var data = results
                .Select(d => d.ErrorMessage).Distinct()
                .Select(d => results.Where(x => x.ErrorMessage == d))
                .Select(d => new CondensedErrorMsg
                {
                    Msg = d.First().ErrorMessage,
                    Fields = d.SelectMany(x => x.MemberNames),
                });

            return data;
        }

        /// <summary>
        /// Represents the validation results but in a condensed / distinct format.
        /// </summary>
        public class CondensedErrorMsg
        {
            public string Msg { get; set; }
            public IEnumerable<string> Fields { get; set; }
        }

    }
}