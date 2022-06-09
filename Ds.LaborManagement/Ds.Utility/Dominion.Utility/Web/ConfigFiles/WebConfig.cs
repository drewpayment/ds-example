using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Dominion.Utility.ExtensionMethods;
using System.IO;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Web.ConfigFiles
{
    public class WebConfigParser
    {
        #region Vars and Props

        /// <summary>
        /// The path to the web config file
        /// </summary>
        public string FilePath { get; set; }

        #endregion

        /// <summary>
        /// Get app settings whether or not they're in the web.config an external file
        /// </summary>
        /// <returns></returns>
        public WcSectionEleInfo GetWcSection(WcSection section)
        {
            var o = new WcSectionEleInfo();
            var info = WcSectionInfo.GetInfo(section);
            var doc = XDocument.Load(FilePath);
            var ele = doc.Root.XPathSelectElement($"/{WcEleNames.Root}/{info.EleName}");

            if (ele != null)
            {
                var exPath = $@"{Path.GetDirectoryName(FilePath)}\{CsFileNames.DcDir}\{info.CsFileName}";

                o.IsExternal = ele.HasAttribute(WcAttrNames.CS);
                o.Elements = o.IsExternal
                    ? GetWcConfigSource(exPath)
                    : ele.Elements();
            }

            return o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devConfigPath"></param>
        /// <returns></returns>
        public IEnumerable<XElement> GetWcConfigSource(string devConfigPath)
        {
            try
            {
                var doc = XDocument.Load(devConfigPath);
                return doc.Root.Elements();
            }
            catch (Exception)
            {
                return new List<XElement>();
            }
        }

    }

    public class WcSectionEleInfo : IWcSectionEleInfo
    {
        public bool IsExternal { get; set; }

        public IEnumerable<XElement> Elements { get; set; }

        public IOpResult<T> ExecuteRule<T>(IWcSectionRule<T> rule) where T : class
        {
            return rule.Execute(this);
        }
    }

    //public class WcSectionData
    //{
    //    bool IsExternal { get; set; }

    //    IEnumerable<XElement> Elements { get; set; }

    //    IOpResult<T> ExecuteRule<T>(IWcSectionRule<T> rule) where T : class
    //    {
    //        return rule.Execute(this.Elements);
    //    }
    //}

    public interface IWcSectionEleInfo
    {
        IEnumerable<XElement> Elements { get; set; }

        bool IsExternal { get; set; }

        IOpResult<T> ExecuteRule<T>(IWcSectionRule<T> rule) where T : class;
    }

    public interface IWcSectionRule<T> where T : class
    {
        //IOpResult<T> Execute(IEnumerable<XElement> elems);

        IOpResult<T> Execute(IWcSectionEleInfo sectionInfo);
    }

    public enum WcSection
    {
        AppSettings,
        ConnectoinStrings,
        SystemIdentityModel,
        SystemIdentityModelServices,
    }

    public class WcSectionInfo
    {
        /// <summary>
        /// The name of the element
        /// </summary>
        public string EleName { get; set; }

        /// <summary>
        /// The name of the external config source file.
        /// </summary>
        public string CsFileName { get; set; }

        public static WcSectionInfo GetInfo(WcSection section)
        {
            switch (section)
            {
                case WcSection.AppSettings:
                    return new WcSectionInfo()
                    {
                        EleName = WcEleNames.As,
                        CsFileName = CsFileNames.WcAS,
                    };
                case WcSection.ConnectoinStrings:
                    return new WcSectionInfo()
                    {
                        EleName = WcEleNames.Cs,
                        CsFileName = CsFileNames.WcCS,
                    };
                case WcSection.SystemIdentityModel:
                    return new WcSectionInfo()
                    {
                        EleName = WcEleNames.Sim,
                        CsFileName = CsFileNames.WcSim,
                    };
                case WcSection.SystemIdentityModelServices:
                    return new WcSectionInfo()
                    {
                        EleName = WcEleNames.Sims,
                        CsFileName = CsFileNames.WcSims,
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(section), section, null);
            }
        }
    }




}


//public interface IWcSection
//{
//    bool IsExternal { get; set; }

//    IEnumerable<XElement> Elements { get; set; }

//    IOpResult<T> ExecuteRule<T>(IWcSectionRule<T> rule) where T : class;
//}


///// <summary>
///// List of config sections with external files
///// </summary>
//public List<DevConfigSource> DevConfigSources { get; set; }


//public class DevConfigSource
//{
//    public string ElementName { get; set; }
//    public string ConfigSourcePath { get; set; }
//}

///// <summary>
///// Get elements that have 'configSource' in them
///// This means they have external config files
///// </summary>
///// <returns></returns>
//public IEnumerable<XElement> GetConfigSourceElements()
//{
//    var doc = XDocument.Load(FilePath);
//    return doc.Root.XPathSelectElements($"/{WcEleNames.WcRoot}/*[@{WcAttrNames.CS}]").ToList();
//}

///// <summary>
///// Parse the elements that have been found to have 'configSoruce'
///// </summary>
///// <returns></returns>
//public List<DevConfigSource> ParseConfigSources()
//{
//    var list = new List<DevConfigSource>();
//    foreach (XElement ele in GetConfigSourceElements())
//    {
//        var o = new DevConfigSource();
//        o.ElementName = ele.Name.LocalName;
//        o.ConfigSourcePath = ele.Attribute(WcAttrNames.CS).Value;
//        list.Add(o);
//    }

//    return list;
//}


/////// <summary>
/////// Get app settings whether or not they're in the web.config an external file
/////// </summary>
/////// <returns></returns>
////public IEnumerable<XElement> GetSectionElements(string elemName)
////{
////    var doc = XDocument.Load(FilePath);
////    var ele = doc.Root.XPathSelectElement($"/{WcEleNames.WcRoot}/{WcEleNames.WcAS}");
////    var hasConfigSource = ele.HasAttribute(WcAttrNames.CS);

////    if (hasConfigSource)
////    {
////        var exPath = $@"{Path.GetDirectoryName(FilePath)}\{ConfigFileNames.DcDir}\{ConfigFileNames.WcAS}";
////        return GetAppSettingsConfigSource(exPath);
////    }

////    return ele.Elements();
////}