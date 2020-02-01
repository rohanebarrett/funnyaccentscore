using System.IO.Compression;

namespace Funny.Accents.Core.Compression.Interfaces
{
    public interface IArchiveDefinition
    {
        string FileLocation { get; set; }
        string CustomFileName { get; set; }
        string ZipLocation { get; set; }
        string ZipFileName { get; set; }
        bool RemoveSourceFile { get; set; }
        CompressionLevel CompressionLevel { get; set; }
    }/*End of IArchiveDefinition interface*/
}/*End of Funny.Accents.Core.Compression.Interfaces namespace*/
