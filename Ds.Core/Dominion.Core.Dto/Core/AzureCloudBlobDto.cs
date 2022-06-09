using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class AzureCloudBlobDto
    {
        /// <summary>
        /// Gets/Sets the blob URI.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets/Sets the name of the blob.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/Sets SAS signature to access the blob.
        /// </summary>
        public string Signature { get; set; }

        public string UrlWithSignature => $"{this.Uri}{this.Signature}";
    }
}
