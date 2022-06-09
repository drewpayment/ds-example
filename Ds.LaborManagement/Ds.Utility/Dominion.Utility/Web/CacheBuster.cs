using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Dominion.Utility.Containers;
using Dominion.Utility.ExtensionMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HPSF;
using org.pdfclown.documents.contents;
using Omu.ValueInjecter;

namespace Dominion.Utility.Web
{
    public class CacheBuster
    {
        public string ResolveFilename(string requestedFile)
        {
            var file = HttpContext.Current.Server.MapPath("~/WebCore/dist/rev-manifest.json");

            if (!File.Exists(file))
                return requestedFile;

            using (var r = new StreamReader(file))
            {
                var json = r.ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(json);
                JArray jArr = data["fileList"];

                if (!jArr.HasValues)
                    // attempt to build default filename if the object doesn't return anything
                    return requestedFile + ".css";

                var fileObj = jArr.Children()
                    .FirstOrDefault(x => x[requestedFile] != null);
                if (!object.ReferenceEquals(fileObj, null))
                    return fileObj.Value<string>(requestedFile);
            }

            return requestedFile;
        }

        public static string GetNgxStyle(string fileName, string appFolder = "ds-source", string currUrl = "")
        {
            var found = string.Empty;
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath($"~/WebCore/dist/{appFolder}"));

            if (!string.IsNullOrWhiteSpace(currUrl) && fileName == "main")
            {
                currUrl = currUrl.Trim().ToLower();
                var isEssOnboarding = currUrl.Contains("onboarding");
                if (isEssOnboarding)
                    fileName = "ess";
            }

            var files = directory.GetFiles("*" + fileName + "*.css");

            foreach (FileInfo file in files)
            {
                found = file.Name;
            }

            return found;
        }

        public string GetNgxSharedStylesheet()
        {
            var found = string.Empty;
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/WebCore/dist/ds-source"));
            var files = directory.GetFiles("ds-styles*.css");

            foreach (FileInfo file in files)
            {
                found = file.Name;
            }

            return found;
        }

        public static string GetNgxMobileSharedStylesheet()
        {
            var found = string.Empty;
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath($"~/WebCore/dist/ds-mobile"));
            var files = directory.GetFiles("ds-mobile-styles*.css");

            foreach (var file in files)
            {
                found = file.Name;
            }

            return found;
        }

        public static string GetNgxScript(string fileName, string appFolder = "ds-source")
        {
            var found = string.Empty;
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath($"~/WebCore/dist/{appFolder}"));
            var files = directory.GetFiles("*" + fileName + "*.js");

            foreach (FileInfo file in files)
            {
                return file.Name;
            }

            return found;
        }

        public static string ResolveScript(string requestedScript)
        {
            var file = HttpContext.Current.Server.MapPath("~/WebCore/dist/webpack-assets.json");

            if (File.Exists(file))
            {
                using (StreamReader r = new StreamReader(file))
                {
                    var json = r.ReadToEnd();
                    var fileText = JObject.Parse(json);
                    var results = fileText.Children().ToList();
                    var fileList = new List<CacheBustedItem>();
                    foreach (JToken res in results)
                    {
                        var hashJsonStr = res.GetProps().GetByName("Value").GetValue(res).ToString();
                        var hashItem = JsonConvert.DeserializeObject<CacheBustedScript>(hashJsonStr);
                        fileList.Add(new CacheBustedItem()
                        {
                            Key = res.GetProps().GetByName("Name").GetValue(res).ToString(),
                            Hash = hashItem.Js
                        });
                    }

                    return fileList.Where(x => x.Key == requestedScript.Replace(" ", ""))
                        .DefaultIfEmpty(new CacheBustedItem() { Hash = "" })
                        .First().Hash;
                }
            }

            return requestedScript;
        }
    }

    public class CacheBustedItem
    {
        public string Key { get; set; }
        public string Hash { get; set; }
    }

    public class CacheFileList
    {
        public List<CacheBustedItem> FileList { get; set; }
    }

    public class CacheBustedScript
    {
        public string Js { get; set; }
    }
}
