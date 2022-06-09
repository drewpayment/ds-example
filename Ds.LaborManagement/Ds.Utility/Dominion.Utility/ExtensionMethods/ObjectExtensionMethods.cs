using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.ExtensionMethods
{
    public static class ObjectExtensionMethods
    {
        /// <summary>
        /// Returns true if the specified type is nullable.
        /// </summary>
        /// <typeparam name="T">Type to check for nullability.</typeparam>
        /// <param name="obj">Object of the type to check for nullability.</param>
        /// <returns></returns>
        public static bool IsTypeNullable<T>(this T obj)
        {
            return TypeExtensionMethods.IsNullableType<T>();
        }

        /// <summary>
        /// Attempts to convert an object to the desired type.  Handles <see cref="Nullable{T}"/> types.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="obj">Object to convert.</param>
        /// <param name="defaultValue">Optional. Default value to return if the object is null.</param>
        /// <returns>Object converted to the specified type.</returns>
        /// <remarks>
        /// Derived From: http://stackoverflow.com/questions/3531318/convert-changetype-fails-on-nullable-types
        /// </remarks>
        public static T ConvertTo<T>(this object obj, T defaultValue = default(T))
        {
            var type = TypeExtensionMethods.GetUnderlyingType<T>();
            return obj != null ? (T)Convert.ChangeType(obj, type) : defaultValue;
        }

        /// <summary>
        /// Attempts to convert an object to the desired type.  Handles <see cref="Nullable{T}"/> types.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        /// <param name="type">Type to convert object to.</param>
        /// <returns>Object converted to the specified type.</returns>
        /// <remarks>
        /// Derived From: http://stackoverflow.com/questions/3531318/convert-changetype-fails-on-nullable-types
        /// </remarks>
        public static object ConvertTo(this object obj, Type type)
        {
            var safeType = type.GetUnderlyingType();
            return obj != null ? Convert.ChangeType(obj, safeType) : type.GetDefaultValue();
        }

        /// <summary>
        /// Returns the ToString() value of the object if the object isn't null.
        /// If the object is null it returns null.
        /// </summary>
        /// <param name="obj">The object in question.</param>
        /// <returns></returns>
        public static string ToStringWithNullCheck(this object obj)
        {
            if (obj == null)
                return null;
            else
                return obj.ToString();
        }

        /// <summary>
        /// XML Serialize an object only debug mode.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="path">File path for xml.</param>
        [Conditional("DEBUG")]
        public static void SerializeToXMLDebug<T>(this T obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (T));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, obj);
            textWriter.Close();
        }

        /// <summary>
        /// XML Serialize an object only debug mode.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="path">File path for xml.</param>
        public static void SerializeToXML<T>(this T obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (T));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, obj);
            textWriter.Close();
        }

        /// <summary>
        /// Serializes to object to an XML string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SerializeToXML<T>(this T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (T));
            MemoryStream memoryStream = new MemoryStream();

            serializer.Serialize(memoryStream, obj);

            memoryStream.Position = 0;
            StreamReader reader = new StreamReader(memoryStream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Perform a deep copy on an object.
        /// A deep copy doesn't copy references. 
        /// All reference types will be new references, not sharing the source's reference.
        /// </summary>
        /// <typeparam name="T">The type your're copying.</typeparam>
        /// <param name="objSource">The object you're copying.</param>
        /// <returns></returns>
        public static T DeepCopyObject<T>(this object objSource)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, objSource);
                stream.Position = 0;
                return (T) formatter.Deserialize(stream);
            }
        }


        public static IOpResult CompareObjectValues<T>(this T one, T two) where T : class
        {
            var opResult = new OpResult.OpResult();
            var dtoProperties = typeof(T).GetProperties();

            // loop through all the dto properties
            foreach (var propInfo in dtoProperties)
            {
                // check to see if we found a matching property in the entity
                // compare the dto to the entity value to determine if they're equal
                var oneValue = propInfo.GetValue(one);
                var twoValue = propInfo.GetValue(two);

                // both null: considered equal
                if (oneValue == null && twoValue == null)
                    continue;

                // if one of the two values were null or they're not equal return false
                if (oneValue == null || twoValue == null || !oneValue.Equals(twoValue))
                {
                    var msg = new ObjectComparisonErrorMsg<T>(oneValue, twoValue, propInfo.Name);
                    opResult.AddMessage(msg);
                }
            }

            return opResult.SetSuccessBasedOnMessageCount();
        }


        public static void IfNotNull<T>(this T obj, Action<T> action)
        {
            if(obj != null)
                action(obj);
        }

        /// <summary>
        /// Get a bool value from an object that may be null.
        /// This was built to provide 2 different objects that contain a bool to return a bool value.
        /// One dilemma was one had a nullable bool the other didn't.
        /// Another was the entire object could be null, but it had a bool property.
        /// This handles either condition.
        /// </summary>
        /// <param name="obj">The object containing the bool property.</param>
        /// <param name="objPropertyValue">Delegate that can return the bool value from the property.</param>
        /// <param name="nullReturnValue">The value you want if all is null; object or property.</param>
        /// <returns>True or False.</returns>
        public static bool IfObjNullOrBoolProperty(this object obj, Func<bool?> objPropertyValue, bool nullReturnValue)
        {
            if(obj == null || !objPropertyValue().HasValue)
                return nullReturnValue;
   
            return objPropertyValue().Value;
        } 

        /// <summary>
        /// Defaults to true if object is null.
        /// Otherwise it uses the property sent.
        /// </summary>
        /// <param name="obj">The object containing the bool property.</param>
        /// <param name="objPropertyValue">Delegate that can return the bool value from the property.</param>
        /// <param name="nullReturnValue">The value you want if all is null; object or property.</param>
        /// <returns>True or False.</returns>
        public static bool TrueIfNull(this object obj, Func<bool?> func)
        {
            return obj.IfObjNullOrBoolProperty(func, true);
        }   


        /// <summary>
        /// Provide an alternative value is a object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="altResult"></param>
        /// <returns></returns>
        public static T IsNull<T>(this T obj, Func<T> altResult)
            where T : class
        {
            if(obj != null)
            {
                var v = obj;
                return v;
            }
            else
            {
                var v = altResult();
                return v;
            }       
        }


        public static string ObjectToHtml<T>(this IEnumerable<T> list, string title)
        {
            StringBuilder myBuilder = new StringBuilder();

            //Open tags and write the top portion.
            myBuilder.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
            myBuilder.AppendLine("<head>");
            myBuilder.AppendLine("<title>");
            myBuilder.AppendLine(title);
            myBuilder.AppendLine("");
            myBuilder.AppendLine("</title>");
            myBuilder.AppendLine("</head>");
            myBuilder.AppendLine("<body>");
            myBuilder.AppendLine("<table border='1px' cellpadding='5' cellspacing='0' ");
            myBuilder.AppendLine("style='border: solid 1px Silver; font-size: x-small;'>");

            //Add the headings row.
            myBuilder.AppendLine("<tr align='left' valign='top'>");

            //ADD THE HEADER
            var propertyArray = typeof(T).GetProperties();
            foreach (var prop in propertyArray)
            {
                string info = string.Format("{0} - {1}", prop.Name, prop.PropertyType.Name);
                myBuilder.AppendLine("<td align='left' valign='top'>");
                myBuilder.AppendLine(info);
                myBuilder.AppendLine("</td>");
            }

            //ADD THE VALUES
            for (int i = 0; i < list.Count(); i++)
            {
                myBuilder.AppendLine("<tr align='left' valign='top'>");

                foreach (var prop in propertyArray)
                {
                    object value = prop.GetValue(list.ElementAt(i), null);
                    //result.AppendFormat("<td>{0}</td>", value ?? String.Empty);

                    value = (value == null) ? DBNull.Value : value; //in case value is null

                    myBuilder.AppendLine("<td align='left' valign='top'>");
                    myBuilder.AppendLine(value.ToString());
                    myBuilder.AppendLine("</td>");

                }
                myBuilder.AppendLine("</tr>");
                //result.AppendLine("</tr>");
            }

            //Close tags.
            myBuilder.AppendLine("</table>");
            myBuilder.AppendLine("</body>");
            myBuilder.AppendLine("</html>");

            //Get the string for return.
            string myHtmlFile = myBuilder.ToString();

            return myHtmlFile;
        }

    }
}