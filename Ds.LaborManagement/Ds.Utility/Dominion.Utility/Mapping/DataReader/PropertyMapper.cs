using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Mapping.DataReader
{
    /// <summary>
    /// Maps a <see cref="IDataReader"/> to a specified type.  
    /// </summary>
    /// <remarks>
    /// Modified From: http://www.codeproject.com/Articles/674419/A-propertymapping-Extension-for-DataReaders
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public static class PropertyMapper
    {
        #region IDataReader Extension Methods

        // REMARK: Commented out for now due to performance issue with PropertyMapper's caching mechanism.  
        // Replaced w/ combination of QuickDataReaderMapper & DataReaderExtensions

        ///// <summary>
        ///// ExtensionMethod that creates a List(of Target) from the IDataReader
        ///// </summary>
        ///// <typeparam name="TDest"></typeparam>
        ///// <param name="reader">IDataReader</param>
        ///// <param name="mustMapAllProperties"></param>
        ///// <returns>Generic List</returns>
        //public static List<TDest> ToList<TDest>(this IDataReader reader, bool mustMapAllProperties = true)
        //{
        //    var culture = CultureInfo.CurrentCulture;
        //    return ToList<TDest>(reader, culture, mustMapAllProperties);
        //}

        ///// <summary>
        ///// ExtensionMethod that creates a List(of Target) from the IDataReader
        ///// </summary>
        ///// <typeparam name="TDest"></typeparam>
        ///// <param name="reader">IDataReader</param>
        ///// <param name="culture"></param>
        ///// <param name="mustMapAllProperties"></param>
        ///// <returns>Generic List</returns>
        //public static List<TDest> ToList<TDest>(this IDataReader reader, CultureInfo culture, bool mustMapAllProperties = true)
        //{
        //    var list = new List<TDest>();

        //    if (!reader.IsClosed && reader.Read())
        //    {
        //        var creator = ReaderMapperCache<TDest>.GetCreator(reader, culture, mustMapAllProperties);
        //        do
        //        {
        //            list.Add(creator(reader));
        //        }
        //        while (reader.Read());
        //    }

        //    return list;
        //}

        ///// <summary>
        ///// ExtensionMethod that creates a LinkedList(of Target) from the IDataReader
        ///// </summary>
        ///// <typeparam name="TDest"></typeparam>
        ///// <param name="reader">IDataReader</param>
        ///// <param name="mustMapAllProperties"></param>
        ///// <returns>Generic List</returns>
        //public static LinkedList<TDest> ToLinkedList<TDest>(this IDataReader reader, bool mustMapAllProperties = true)
        //{
        //    var culture = CultureInfo.CurrentCulture;
        //    return ToLinkedList<TDest>(reader, culture, mustMapAllProperties);
        //}

        ///// <summary>
        ///// ExtensionMethod that creates a LinkedList(of Target) from the IDataReader
        ///// </summary>
        ///// <typeparam name="TDest"></typeparam>
        ///// <param name="reader">IDataReader</param>
        ///// <param name="culture"></param>
        ///// <param name="mustMapAllProperties"></param>
        ///// <returns>Generic List</returns>
        //public static LinkedList<TDest> ToLinkedList<TDest>(this IDataReader reader, CultureInfo culture, bool mustMapAllProperties = true)
        //{
        //    var list = new LinkedList<TDest>();

        //    if (!reader.IsClosed && reader.Read())
        //    {
        //        var creator = ReaderMapperCache<TDest>.GetCreator(reader, culture, mustMapAllProperties);
        //        do
        //        {
        //            list.AddLast(creator(reader));
        //        }
        //        while (reader.Read());
        //    }

        //    return list;
        //}

        ///// <summary>
        ///// ExtensionMethod that creates an IEnumerable from the IDatareader
        ///// </summary>
        ///// <param name="reader">IDataReader</param>
        ///// <param name="mustMapAllProperties"></param>
        ///// <returns>IEnumerable</returns>
        //public static IEnumerable<TDest> AsEnumerable<TDest>(this IDataReader reader, bool mustMapAllProperties = true)
        //{
        //    var culture = CultureInfo.CurrentCulture;
        //    if (!reader.IsClosed && reader.Read())
        //    {
        //        var creator = ReaderMapperCache<TDest>.GetCreator(reader, culture, mustMapAllProperties);
        //        do
        //        {
        //            yield return creator(reader);
        //        }
        //        while (reader.Read());
        //    }
        //}

        ///// <summary>
        ///// ExtensionMethod that creates an IEnumerable from the IDatareader
        ///// </summary>
        ///// <param name="reader">IDataReader</param>
        ///// <param name="culture"></param>
        ///// <param name="mustMapAllProperties"></param>
        ///// <returns>IEnumerable</returns>
        //public static IEnumerable<TDest> AsEnumerable<TDest>(this IDataReader reader, CultureInfo culture, bool mustMapAllProperties = true)
        //{
        //    if (!reader.IsClosed && reader.Read())
        //    {
        //        var creator = ReaderMapperCache<TDest>.GetCreator(reader, culture, mustMapAllProperties);
        //        do
        //        {
        //            yield return creator(reader);
        //        }
        //        while (reader.Read());
        //    }
        //}

        ///// <summary>
        ///// ExtensionMethod that creates a Dictionary from the IDatareader using specified key selector function
        ///// </summary>
        //public static Dictionary<TKey, TDest> ToDictionary<TKey, TDest>(this IDataReader reader, Func<TDest, TKey> keySelector, bool mustMapAllProperties = true)
        //{
        //    return ToDictionary(reader, keySelector, CultureInfo.CurrentCulture, mustMapAllProperties);
        //}

        ///// <summary>
        ///// ExtensionMethod that creates a Dictionary from the IDatareader using specified key selector function
        ///// </summary>
        //public static Dictionary<TKey, TDest> ToDictionary<TKey, TDest>(IDataReader reader, Func<TDest, TKey> keySelector, CultureInfo culture, bool mustMapAllProperties = true)
        //{
        //    var dict = new Dictionary<TKey, TDest>();
        //    if (!reader.IsClosed && reader.Read())
        //    {
        //        var creator = ReaderMapperCache<TDest>.GetCreator(reader, culture, mustMapAllProperties);
        //        do
        //        {
        //            var item = creator(reader);
        //            dict.Add(keySelector(item), item);
        //        }
        //        while (reader.Read());
        //    }

        //    return dict;
        //}
    
        #endregion

        #region Public Utils

        /// <summary>
        /// Creates a delegate that creates an instance of <see cref="TDest"/> from the supplied DataRecord
        /// </summary>
        /// <param name="recordInstance">An instance of a DataRecord</param>
        /// <param name="culture"></param>
        /// <param name="mustMapAllProperties"></param>
        /// <returns>A Delegate that creates a new instance of Target with the values set from the supplied DataRecord</returns>
        /// <remarks></remarks>
        public static Func<IDataRecord, TDest> GetInstanceCreator<TDest>(IDataRecord recordInstance, CultureInfo culture, bool mustMapAllProperties)
        {
            var bindings       = new List<MemberBinding>();
            var sourceType     = typeof(IDataRecord);
            var sourceInstance = Expression.Parameter(sourceType, "SourceInstance");
            var targetType     = typeof(TDest);
            var schemaTable    = ((IDataReader)recordInstance).GetSchemaTable();

            Expression body;

            //The actual names for Tuples are System.Tuple`1,System.Tuple`2 etc where the number stands for the number of Parameters 
            //This crashes whenever Microsoft creates a class in the System Namespace called Tuple`duple 
            if (targetType.FullName.StartsWith("System.Tuple`"))
            {
                var constructors = targetType.GetConstructors();
                if (constructors.Count() != 1)
                {
                    throw new ArgumentException("Tuple must have one Constructor");
                }

                var constructor = constructors[0];

                var parameters = constructor.GetParameters();
                if (parameters.Length > 7)
                {
                    throw new NotSupportedException("Nested Tuples are not supported");
                }

                var targetValueExpressions = new Expression[parameters.Length];
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    if (i >= recordInstance.FieldCount)
                    {
                        if (mustMapAllProperties)
                        {
                            throw new ArgumentException("Tuple has more fields than the DataReader");
                        }

                        targetValueExpressions[i] = Expression.Default(parameterType);
                    }
                    else
                    {
                        targetValueExpressions[i] = GetTargetValueExpression(
                            recordInstance, 
                            culture, 
                            sourceType, 
                            sourceInstance, 
                            schemaTable, 
                            i, 
                            parameterType);
                    }
                }

                body = Expression.New(constructor, targetValueExpressions);
            }
            else if (targetType.IsElementaryType())
            {
                //Find out if SourceType is an elementary Type

                //If you try to map an elementary type, e.g. ToList<Int32>, there is no name to map on. So to avoid error we map to the first field in the datareader
                //If this is wrong, it is the query that's wrong.
                const int i = 0;
                var targetValueExpression = GetTargetValueExpression(
                    recordInstance, 
                    culture, 
                    sourceType, 
                    sourceInstance, 
                    schemaTable, 
                    i, 
                    targetType);

                var targetExpression = Expression.Variable(targetType, "Target");
                var assignExpression = Expression.Assign(targetExpression, targetValueExpression);
                body = Expression.Block(new[] { targetExpression }, assignExpression);
            }
            else
            {
                //Loop through the Properties in the Target and the Fields in the Record to check which ones are matching
                foreach (var targetMember in targetType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    var member = targetMember;
                    Action work = delegate
                        {
                            for (var i = 0; i < recordInstance.FieldCount; i++)
                            {
                                //Check if the RecordFieldName matches the TargetMember
                                if (MemberMatchesName(member, recordInstance.GetName(i)))
                                {
                                    var targetValueExpression = GetTargetValueExpression(
                                        recordInstance, 
                                        culture, 
                                        sourceType, 
                                        sourceInstance, 
                                        schemaTable, 
                                        i, 
                                        member.FieldType);

                                    //Create a binding to the target member
                                    var bindExpression = Expression.Bind(
                                        member, 
                                        targetValueExpression);
                                    bindings.Add(bindExpression);
                                    return;
                                }
                            }

                            //If we reach this code the targetmember did not get mapped
                            if (mustMapAllProperties)
                            {
                                throw new ArgumentException(
                                    string.Format(
                                        "TargetField {0} is not matched by any field in the DataReader", 
                                        member.Name));
                            }
                        };
                    work();
                }

                foreach (var targetMember in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (targetMember.CanWrite)
                    {
                        var member = targetMember;
                        Action work = delegate
                            {
                                for (var i = 0; i < recordInstance.FieldCount; i++)
                                {
                                    //Check if the RecordFieldName matches the TargetMember
                                    if (MemberMatchesName(member, recordInstance.GetName(i)))
                                    {
                                        var targetValueExpression = GetTargetValueExpression(
                                            recordInstance, 
                                            culture, 
                                            sourceType, 
                                            sourceInstance, 
                                            schemaTable, 
                                            i, 
                                            member.PropertyType);

                                        //Create a binding to the target member
                                        var bindExpression = Expression.Bind(
                                            member, 
                                            targetValueExpression);
                                        bindings.Add(bindExpression);
                                        return;
                                    }
                                }

                                //If we reach this code the targetmember did not get mapped
                                if (mustMapAllProperties)
                                {
                                    throw new ArgumentException(
                                        string.Format(
                                            "TargetProperty {0} is not matched by any Field in the DataReader", 
                                            member.Name));
                                }
                            };
                        work();
                    }
                }

                //Create a memberInitExpression that Creates a new instance of Target using bindings to the DataRecord
                body = Expression.MemberInit(Expression.New(targetType), bindings);
            }

            //Compile the Expression to a Delegate
            return Expression.Lambda<Func<IDataRecord, TDest>>(body, sourceInstance).Compile();
        }

        #endregion

        #region Private Utils

        #region Converter

        /// <summary>
        /// Gets an expression representing the Source converted to the TargetType
        /// </summary>
        /// <param name="sourceType">The Type of the Source</param>
        /// <param name="sourceExpression">An Expression representing the Source value</param>
        /// <param name="targetType">The Type of the Target</param>
        /// <param name="culture"></param>
        /// <returns>Expression</returns>
        public static Expression GetConversionExpression(Type sourceType, Expression sourceExpression, Type targetType, CultureInfo culture)
        {
            //Just assign the RecordField
            if (sourceType.Equals(targetType))
                return sourceExpression;

            if (sourceType.Equals<string>())
                return GetParseExpression(sourceExpression, targetType, culture);
            
            //There are no casts from primitive types to String.
            //And Expression.Convert Method (Expression, Type, MethodInfo) only works with static methods.
            if (targetType.Equals<string>())
                return Expression.Call(sourceExpression, sourceType.GetMethod("ToString", Type.EmptyTypes));
            
            if (targetType.Equals<bool>())
            {
                var toBooleanMethod = typeof(Convert).GetMethod("ToBoolean", new[] { sourceType });
                return Expression.Call(toBooleanMethod, sourceExpression);
            }

            //Using Expression.Convert works wherever you can make an explicit or implicit cast.
            //But it casts OR unboxes an object, therefore the double cast. First unbox to the SourceType and then cast to the TargetType
            //It also doesn't convert a numerical type to a String or date, this will throw an exception.
            return Expression.Convert(sourceExpression, targetType);
        }

        /// <summary>
        /// Creates an expression that parses a string to an enum
        /// </summary>
        /// <param name="sourceExpression">The Source to parse</param>
        /// <param name="targetType">The Type of enum</param>
        /// <returns>MethodCallExpression</returns>
        public static MethodCallExpression GetEnumParseExpression(Expression sourceExpression, Type targetType)
        {
            //Get the MethodInfo for parsing an Enum
            var enumParseMethod            = typeof(Enum).GetMethod("Parse", new[] { typeof(Type), typeof(string), typeof(bool) });
            var targetMemberTypeExpression = Expression.Constant(targetType);
            var ignoreCase                 = Expression.Constant(true, typeof(bool));

            //Create an expression the calls the Parse method
            var callExpression = Expression.Call(enumParseMethod, new[] { targetMemberTypeExpression, sourceExpression, ignoreCase });

            return callExpression;
        }

        /// <summary>
        /// Creates an Expression that parses a string to Char or bool
        /// </summary>
        /// <param name="sourceExpression"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static MethodCallExpression GetGenericParseExpression(Expression sourceExpression, Type targetType)
        {
            var parseMethod    = targetType.GetMethod("Parse", new[] { typeof(string) });
            var callExpression = Expression.Call(parseMethod, new[] { sourceExpression });
            return callExpression;
        }

        /// <summary>
        /// Creates an Expression that parses a string to a number
        /// </summary>
        /// <param name="sourceExpression"></param>
        /// <param name="targetType"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static MethodCallExpression GetNumberParseExpression(Expression sourceExpression, Type targetType,  CultureInfo culture)
        {
            var parseMethod        = targetType.GetMethod("Parse", new[] { typeof(string), typeof(NumberFormatInfo) });
            var providerExpression = Expression.Constant(culture.NumberFormat, typeof(NumberFormatInfo));
            var callExpression     = Expression.Call(parseMethod, new[] { sourceExpression, providerExpression });

            return callExpression;
        }

        /// <summary>
        /// Creates an Expression that parses a string to a DateTime
        /// </summary>
        /// <param name="sourceExpression"></param>
        /// <param name="targetType"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static MethodCallExpression GetDateTimeParseExpression(Expression sourceExpression, Type targetType, CultureInfo culture)
        {
            var parseMethod = targetType.GetMethod("Parse", new[] { typeof(string), typeof(DateTimeFormatInfo) });
            var providerExpression = Expression.Constant(culture.DateTimeFormat, typeof(DateTimeFormatInfo));
            var callExpression = Expression.Call(parseMethod, new[] { sourceExpression, providerExpression });

            return callExpression;
        }

        /// <summary>
        /// Creates an Expression that parses a string
        /// </summary>
        /// <param name="sourceExpression"></param>
        /// <param name="targetType"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static Expression GetParseExpression(Expression sourceExpression, Type targetType, CultureInfo culture)
        {
            var underlyingType = targetType.GetUnderlyingType();
            if (underlyingType.IsEnum)
            {
                var parsedEnumExpression = GetEnumParseExpression(sourceExpression, underlyingType);

                //Enum.Parse returns an object that needs to be unboxed
                return Expression.Unbox(parsedEnumExpression, targetType);
            }

            Expression parseExpression;
            switch (underlyingType.FullName)
            {
                case "System.Byte":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.SByte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                    parseExpression = GetNumberParseExpression(sourceExpression, underlyingType, culture);
                    break;
                case "System.DateTime":
                    parseExpression = GetDateTimeParseExpression(sourceExpression, underlyingType, culture);
                    break;
                case "System.bool":
                case "System.Boolean":
                case "System.Char":
                    parseExpression = GetGenericParseExpression(sourceExpression, underlyingType);
                    break;
                default:
                    throw new ArgumentException(string.Format("Conversion from {0} to {1} is not supported", "String", targetType));
            }

            //Convert to nullable if necessary
            var method = typeof(string).GetMethod("IsNullOrEmpty", new [] {typeof(string)});
            var nullCheckConvert = Expression.Condition(Expression.Call(method, new [] {sourceExpression}), Expression.Default(targetType), Expression.Convert(parseExpression, targetType));

            return Nullable.GetUnderlyingType(targetType) == null ? parseExpression : nullCheckConvert;

            
        }

        #endregion

        #region Mapper

        public static bool IsElementaryType(this Type t)
        {
            return ElementaryTypes.Contains(t);
        }

        public readonly static HashSet<Type> ElementaryTypes = LoadElementaryTypes();

        public static HashSet<Type> LoadElementaryTypes()
        {
            var typeSet = new HashSet<Type>
                {
                    typeof(string), 
                    typeof(byte), 
                    typeof(sbyte), 
                    typeof(short), 
                    typeof(int), 
                    typeof(long), 
                    typeof(ushort), 
                    typeof(uint), 
                    typeof(ulong), 
                    typeof(float), 
                    typeof(double), 
                    typeof(decimal), 
                    typeof(DateTime), 
                    typeof(Guid), 
                    typeof(bool), 
                    typeof(TimeSpan), 
                    typeof(byte?), 
                    typeof(sbyte?), 
                    typeof(short?), 
                    typeof(int?), 
                    typeof(long?), 
                    typeof(ushort?), 
                    typeof(uint?), 
                    typeof(ulong?), 
                    typeof(float?), 
                    typeof(double?), 
                    typeof(decimal?), 
                    typeof(DateTime?), 
                    typeof(Guid?), 
                    typeof(bool?), 
                    typeof(TimeSpan?)
                };
            return typeSet;
        }

        /// <summary>
        /// Gets an Expression that checks if the current RecordField is null
        /// </summary>
        /// <param name="sourceType">The Type of the Record</param>
        /// <param name="sourceInstance">The Record instance</param>
        /// <param name="i">The index of the parameter</param>
        /// <returns>MethodCallExpression</returns>
        public static MethodCallExpression GetNullCheckExpression(Type sourceType, Expression sourceInstance, int i)
        {
            var getNullValueMethod = sourceType.GetMethod("IsDBNull", new[] { typeof(int) });
            var nullCheckExpression = Expression.Call(sourceInstance, getNullValueMethod, Expression.Constant(i, typeof(int)));
            return nullCheckExpression;
        }

        /// <summary>
        /// Gets an Expression that represents the getter method for the RecordField
        /// </summary>
        /// <param name="sourceType">The Type of the Record</param>
        /// <param name="sourceInstance">The Record instance</param>
        /// <param name="i">The index of the parameter</param>
        /// <param name="recordFieldType">The Type of the RecordField</param>
        /// <returns></returns>
        public static Expression GetRecordFieldExpression(Type sourceType, Expression sourceInstance, int i, Type recordFieldType)
        {
            MethodInfo getValueMethod;

            switch (recordFieldType.FullName)
            {
                case "System.bool":
                case "System.Boolean":
                    getValueMethod = sourceType.GetMethod("GetBoolean", new[] { typeof(int) });
                    break;
                case "System.Byte":
                    getValueMethod = sourceType.GetMethod("GetByte", new[] { typeof(int) });
                    break;
                case "System.Char":
                    getValueMethod = sourceType.GetMethod("GetChar", new[] { typeof(int) });
                    break;
                case "System.DateTime":
                    getValueMethod = sourceType.GetMethod("GetDateTime", new[] { typeof(int) });
                    break;
                case "System.Decimal":
                    getValueMethod = sourceType.GetMethod("GetDecimal", new[] { typeof(int) });
                    break;
                case "System.Double":
                    getValueMethod = sourceType.GetMethod("GetDouble", new[] { typeof(int) });
                    break;
                case "System.Single":
                    getValueMethod = sourceType.GetMethod("GetFloat", new[] { typeof(int) });
                    break;
                case "System.Guid":
                    getValueMethod = sourceType.GetMethod("GetGuid", new[] { typeof(int) });
                    break;
                case "System.Int16":
                    getValueMethod = sourceType.GetMethod("GetInt16", new[] { typeof(int) });
                    break;
                case "System.Int32":
                    getValueMethod = sourceType.GetMethod("GetInt32", new[] { typeof(int) });
                    break;
                case "System.Int64":
                    getValueMethod = sourceType.GetMethod("GetInt64", new[] { typeof(int) });
                    break;
                case "System.String":
                    getValueMethod = sourceType.GetMethod("GetString", new[] { typeof(int) });
                    break;
                default:
                    getValueMethod = sourceType.GetMethod("GetValue", new[] { typeof(int) });
                    break;
            }

            var recordFieldExpression = Expression.Call(sourceInstance, getValueMethod, Expression.Constant(i, typeof(int)));
            return recordFieldExpression;
        }

        /// <summary>
        /// Returns The FieldNameAttribute if existing
        /// </summary>
        /// <param name="member">MemberInfo</param>
        /// <returns>String</returns>
        public static string GetFieldNameAttribute(MemberInfo member)
        {
            return member.GetCustomAttributes(typeof(FieldNameAttribute), true).Any() 
                ? ((FieldNameAttribute)member.GetCustomAttributes(typeof(FieldNameAttribute), true)[0]).FieldName 
                : string.Empty;
        }

        /// <summary>
        /// Checks if the Field name matches the Member name or Members FieldNameAttribute
        /// </summary>
        /// <param name="member">The Member of the Instance to check</param>
        /// <param name="name">The Name to compare with</param>
        /// <returns>True if Fields match</returns>
        /// <remarks>FieldNameAttribute takes precedence over TargetMembers name.</remarks>
        public static bool MemberMatchesName(MemberInfo member, string name)
        {
            var fieldnameAttribute = GetFieldNameAttribute(member);
            return string.Equals(fieldnameAttribute, name, StringComparison.CurrentCultureIgnoreCase) || string.Equals(member.Name, name, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Returns an Expression representing the value to set the TargetProperty to
        /// </summary>
        /// <remarks>Prepares the parameters to call the other overload</remarks>
        public static Expression GetTargetValueExpression(
            IDataRecord recordInstance, 
            CultureInfo culture, 
            Type sourceType, 
            Expression sourceInstance, 
            DataTable schemaTable, 
            int i, 
            Type targetMemberType)
        {
            var recordFieldType                = recordInstance.GetFieldType(i);
            var allowDbNull                    = Convert.ToBoolean(schemaTable.Rows[i]["AllowDBNull"]);
            var recordFieldExpression          = GetRecordFieldExpression(sourceType, sourceInstance, i, recordFieldType);
            var convertedRecordFieldExpression = GetConversionExpression(recordFieldType, recordFieldExpression, targetMemberType, culture);
            var nullCheckExpression            = GetNullCheckExpression(sourceType, sourceInstance, i);

            //Create an expression that assigns the converted value to the target
            return allowDbNull
                ? Expression.Condition(
                    nullCheckExpression,
                    Expression.Default(targetMemberType),
                    convertedRecordFieldExpression,
                    targetMemberType)
                : convertedRecordFieldExpression;
        }

        #endregion

        #endregion

        #region Util Classes

        private class DataField
        {
            private readonly Type _fieldType;
            private readonly string _fieldName;

            public DataField(string fieldName, Type fieldType)
            {
                this._fieldName = fieldName;
                this._fieldType = fieldType;
            }
        }

        private class DataFieldList : List<DataField>
        {
            public CultureInfo Culture              { get; set; }
            public bool        MustMapAllProperties { get; set; }

            public DataFieldList(CultureInfo culture, bool mustMapAllProperties)
            {
                this.Culture = culture;
                this.MustMapAllProperties = mustMapAllProperties;
            }

            public override bool Equals(object obj)
            {
                var otherList = obj as DataFieldList;
                return otherList != null
                       && (this.MustMapAllProperties.Equals(otherList.MustMapAllProperties)
                           && this.Culture.Equals(otherList.Culture) && this.SequenceEqual(otherList));
            }

            public override int GetHashCode()
            {
                var hash = this.Culture.GetHashCode() ^ Convert.ToInt32(this.MustMapAllProperties);
                var i = 0;

                foreach (var item in this)
                {
                    hash = hash * 31 + item.GetHashCode();
                    i += 1;
                    if (i < 7)
                    {
                        hash = unchecked(hash ^ hash >> 32);
                        i = 0;
                    }
                }

                return unchecked(hash ^ hash >> 32);
            }
        }

        private sealed class ReaderMapperCache<TDest> : Dictionary<int, Func<IDataRecord, TDest>> 
        {
            private ReaderMapperCache()
            {
            }

            private static readonly object SyncRoot = new object();

            private static readonly Dictionary<DataFieldList, Func<IDataRecord, TDest>> Creators = new Dictionary<DataFieldList, Func<IDataRecord, TDest>>();

            public static Func<IDataRecord, TDest> GetCreator(IDataRecord recordInstance, CultureInfo culture, bool mustMapAllProperties)
            {
                var list = GetDataFieldList(recordInstance, culture, mustMapAllProperties);
                if (!Creators.ContainsKey(list))
                {
                    lock (SyncRoot)
                    {
                        if (!Creators.ContainsKey(list))
                        {
                            Creators.Add(list, GetInstanceCreator<TDest>(recordInstance, culture, mustMapAllProperties));
                        }
                    }
                }

                return Creators[list];
            }

            private static DataFieldList GetDataFieldList(IDataRecord recordInstance, CultureInfo culture, bool mustMapAllProperties)
            {
                var list = new DataFieldList(culture, mustMapAllProperties);
                for (var i = 0; i < recordInstance.FieldCount; i++)
                {
                    var field = new DataField(recordInstance.GetName(i), recordInstance.GetFieldType(i));
                    list.Add(field);
                }

                return list;
            }
        }
        #endregion
    }
}