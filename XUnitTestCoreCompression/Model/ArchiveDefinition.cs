using Funny.Accents.Core.Compression.Interfaces;
using System.IO.Compression;

namespace XUnitTestCoreCompression.Model
{
    class ArchiveDefinition : IArchiveDefinition
    {
        public string FileLocation { get; set; }
        public string CustomFileName { get; set; }
        public string ZipLocation { get; set; }
        public string ZipFileName { get; set; }
        public bool RemoveSourceFile { get; set; }
        public CompressionLevel CompressionLevel { get; set; }
    }
}
