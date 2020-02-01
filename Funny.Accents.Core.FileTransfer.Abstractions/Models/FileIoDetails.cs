using Funny.Accents.Core.FileTransfer.Abstractions.Interfaces;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Models
{
    public class FileIoDetails : IFileIoDetails
    {
        public string DirectoryName { get; set; }
        public string FileName { get; set; }
        public string FileFilter { get; set; }
    }/*End of FileIoDetails class*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Models namespace*/
