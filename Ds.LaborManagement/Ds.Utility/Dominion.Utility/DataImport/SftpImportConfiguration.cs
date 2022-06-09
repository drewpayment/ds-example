using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Dominion.Utility.DataImport
{
    public class SftpImportConfiguration
    {
        public ConnectionInfo ConnectionInfo { get; set; }
        public int[] ClientIds { get; set; }
        public string RemoteDirectory { get; set; }
        public string LocalDirectory { get; set; }
        public string ExportType { get; set; }
        public string[] FileNames { get; set; }
        public string FileType { get; set; }
    }
}
