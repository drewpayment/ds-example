﻿//using System.IO;
//using Newtonsoft.Json;

//namespace Dominion.Utility.DataExport.ExMeths
//{
//    public static class JsonSerializationExtensionMethods
//    {
//        /// <summary>
//        /// This object represents an embedded file.
//        /// If the embedded file is a json file, use this method to convert to the object you're expecting from it.
//        /// </summary>
//        /// <typeparam name="T">The object type your are expecting the json to represent.</typeparam>
//        /// <returns></returns>
//        public static T DeserializeJson<T>(this byte[] arr)
//        {
//            var obj = default(T);

//            using(var ms = new MemoryStream(arr))
//                obj = ms.DeserializeJson<T>();

//            return obj;
//        }

//        /// <summary>
//        /// Serialize an object to json.
//        /// If the file already exists then the file will be deleted.
//        /// NOTE:
//        ///     Be careful with entities that have a lot of navigational properties.
//        ///     This will call on all navigational properties.
//        /// </summary>
//        /// <param name="obj">The object to serialize.</param>
//        /// <param name="path">The path to serialize it to.</param>
//        public static void SerializeJson(this object obj, string path)
//        {
//            //this will delete the file if it exists
//            File.Delete(path);

//            //lowfix: jay: http://www.tecsupra.com/serializing-only-some-properties-of-an-object-to-json-using-newtonsoft-json-net/
//            //setup this property select style of serializing
//            using (var fs = File.Open(path, FileMode.CreateNew))
//            {
//                using (var sw = new StreamWriter(fs))
//                {
//                    using (var jw = new JsonTextWriter(sw))
//                    {
//                        var serializer = new JsonSerializer();
//                        jw.Formatting = Formatting.Indented;
//                        serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
//                        serializer.Serialize(jw, obj);
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Deserialize a json file.
//        /// </summary>
//        /// <typeparam name="T">The object you're deserializing.</typeparam>
//        /// <param name="path">The path to the json file.</param>
//        /// <returns>The object as <see cref="T"/>.</returns>
//        public static T DeserializeJson<T>(this string path)
//        {
//            var json = File.ReadAllText(path);
//            var obj = JsonConvert.DeserializeObject<T>(json);
//            return obj;
//        }

//        /// <summary>
//        /// Deserialize a json stream.
//        /// </summary>
//        /// <typeparam name="T">The object you're deserializing to.</typeparam>
//        /// <param name="stream">The stream containing the Json.</param>
//        /// <returns></returns>
//        public static T DeserializeJson<T>(this Stream stream)
//        {
//            T data;
//            var serializer = new JsonSerializer();

//            using (var streamReader = new StreamReader(stream))
//                data = (T)serializer.Deserialize(streamReader, typeof(T));

//            return data;
//        }

//        /// <summary>
//        /// Get a json string by default indented.
//        /// Really just a short cut to the JsonConvert call.
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <param name="format"></param>
//        /// <returns></returns>
//        public static string ToJson(this object obj, Formatting format = Formatting.Indented)
//        {
//            return JsonConvert.SerializeObject(obj, Formatting.Indented);
//        }
//    }
//}