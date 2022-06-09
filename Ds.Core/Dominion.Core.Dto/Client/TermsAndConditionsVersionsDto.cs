using Dominion.Core.Dto.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Dominion.Authentication.Dto
{
    public class TermsAndConditionsVersionsDto : IDisposable
    {
        public int TermsAndConditionsVersionID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? LastEffectiveDate { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public StreamContent File { get; set; }

        public void Dispose()
        {
            File.Dispose();
        }
    }
}