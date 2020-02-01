using System;

namespace Funny.Accents.Core.Compression.Interfaces
{
    public interface IArchiveResults
    {
        string ArchivedFile { get; }
        string ArchiveContainer { get; }
        DateTime? ArchivalDate { get; }
        bool ArchivalStatus { get; }
        string ArchivalCompressionLevel { get; }
    }/*End of IArchiveResults interface*/
}/*End of Funny.Accents.Core.Compression.Interfaces namespace*/
