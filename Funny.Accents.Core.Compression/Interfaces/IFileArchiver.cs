using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Funny.Accents.Core.Compression.Interfaces
{
    internal interface IFileArchiver
    {
        //void ArchiveFile(IArchiveDefinition archiveDefinition);
        IEnumerable<IArchiveResults> ArchiveFile(IEnumerable<IArchiveDefinition> archiveDefinition);
        IEnumerable<IArchiveResults> ArchiveDirectory(string dirToArchive, string zipPath,
            SearchOption searchOption = SearchOption.TopDirectoryOnly,
            Func<string, bool> validFiles = null,
            Func<string, string> customFileName = null,
            bool removeFiles = false,
            CompressionLevel compressionLevel = CompressionLevel.Optimal);
    }/*End of IFileArchiver inteface*/
}/*End of Funny.Accents.Core.Compression.Interfaces namespace*/
