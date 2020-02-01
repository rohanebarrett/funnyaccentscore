using System;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Interfaces
{
    public interface ITransferDetails
    {
        string DestinationDirectory { get; set; }
        string DestinationFileName { get; set; }
        string SourceDirectory { get; set; }
        string SourceFileName { get; set; }
        string WorkingDirectory { get; set; }
        bool OverrideExisting { get; set; }
        Func<string, string> CustomFileName { get; set; }
        bool IsDirectoryUpload { get; set; }
    }/*End of ITransferDetails interface*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Interfaces namespace*/
