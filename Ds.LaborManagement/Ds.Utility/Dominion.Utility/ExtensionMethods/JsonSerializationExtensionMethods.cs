using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Dominion.Utility.ExtensionMethods
{
    public static class JsonSerializationExtensionMethods
    {
        /// <summary>
        /// This object represents an embedded file.
        /// If the embedded file is a json file, use this method to convert to the object you're expecting from it.
        /// </summary>
        /// <typeparam name="T">The object type your are expecting the json to represent.</typeparam>
        /// <returns></returns>
        public static T DeserializeJson<T>(this byte[] arr)
        {
            var obj = default(T);

            using(var ms = new MemoryStream(arr))
                obj = ms.DeserializeJson<T>();

            return obj;
        }

        /// <summary>
        /// Serialize an object to json.
        /// If the file already exists then the file will be deleted.
        /// NOTE:
        ///     Be careful with entities that have a lot of navigational properties.
        ///     This will call on all navigational properties.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="path">The path to serialize it to.</param>
        public static void SerializeJson(this object obj, string path, int? depth = null)
        {
            //this will delete the file if it exists
            File.Delete(path);

            //lowfix: jay: http://www.tecsupra.com/serializing-only-some-properties-of-an-object-to-json-using-newtonsoft-json-net/
            //setup this property select style of serializing
            using (var fs = File.Open(path, FileMode.CreateNew))
            {
                using (var sw = new StreamWriter(fs))
                {
                    using (var jw = new JsonTextWriter(sw))
                    {
                        var serializer = new JsonSerializer();
                        jw.Formatting = Formatting.Indented;
                        serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        serializer.MaxDepth = depth;
                        serializer.Serialize(jw, obj);
                    }
                }
            }
        }

        /// <summary>
        /// Serializes to an INDENTED json string.
        /// </summary>
        /// <param name="obj">The object you're serializing.</param>
        /// <returns>Serialize json string.</returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented); 
        }

        public static string ToJson<T>(this T obj, Formatting frmt = Formatting.None) where T : class
        {
            return JsonConvert.SerializeObject(obj, frmt); 
        }
        /// <summary>
        /// Deserialize a json file.
        /// </summary>
        /// <typeparam name="T">The object you're deserializing.</typeparam>
        /// <param name="path">The path to the json file.</param>
        /// <returns>The object as <see cref="T"/>.</returns>
        public static T DeserializeJson<T>(this string path)
        {
            var json = File.ReadAllText(path);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        /// <summary>
        /// Deserialize a json stream.
        /// </summary>
        /// <typeparam name="T">The object you're deserializing to.</typeparam>
        /// <param name="stream">The stream containing the Json.</param>
        /// <returns></returns>
        public static T DeserializeJson<T>(this Stream stream)
        {
            T data;
            var serializer = new JsonSerializer();

            using (var streamReader = new StreamReader(stream))
                data = (T)serializer.Deserialize(streamReader, typeof(T));

            return data;
        }

        public static T FromWebJson<T>(this string input)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(input);
        }

        public static string ToWebJson(this object input)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(input);
        }

    }
}
