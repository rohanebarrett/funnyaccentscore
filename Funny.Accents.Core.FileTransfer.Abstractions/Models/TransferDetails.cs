using System;
using Funny.Accents.Core.FileTransfer.Abstractions.Interfaces;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Models
{
    public class TransferDetails : ITransferDetails
    {
        public string DestinationDirectory { get; set; }
        public string DestinationFileName { get; set; }
        public string SourceDirectory { get; set; }
        public string SourceFileName { get; set; }
        public string WorkingDirectory { get; set; }
        public bool OverrideExisting { get; set; }
        public Func<string, string> CustomFileName { get; set; }
        public bool IsDirectoryUpload { get; set; }
    }/*End of TransferDetails class*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Models namespace*/
