using Renci.SshNet;

namespace Dominion.Utility.DataExport
{
    public class SftpExportConfiguration
    {
        public ConnectionInfo ConnectionInfo { get; set; }
        public int[] ClientIds { get; set; }
        public string RemoteDirectory { get; set; }
        public string ExportType { get; set; }
        public string[] FileNames { get; set; }
        public string FileType { get; set; }
        public string Server { get; set; }
    }
}
