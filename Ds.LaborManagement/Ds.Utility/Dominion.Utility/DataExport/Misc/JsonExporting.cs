using System.IO;
using Newtonsoft.Json;

namespace Dominion.Utility.DataExport.Misc
{
    public class JsonExporting
    {
        public void SerializeJson(object obj, string path)
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
                        serializer.Serialize(jw, obj);
                    }
                }
            }
        }

    }
}
