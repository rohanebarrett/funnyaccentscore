using System;
using Funny.Accents.Core.Compression.Interfaces;

namespace Funny.Accents.Core.Compression.Model
{
    struct ArchiveResults : IArchiveResults
    {
        public string ArchivedFile { get; }
        public string ArchiveContainer { get; }
        public DateTime? ArchivalDate { get; }
        public bool ArchivalStatus { get; }
        public string ArchivalCompressionLevel { get; }

        public ArchiveResults(string archivedFile, string archiveContainer,
            DateTime? archivalDate, bool archivalStatus,
            string archivalCompressionLevel)
        {
            ArchivedFile = archivedFile;
            ArchiveContainer = archiveContainer;
            ArchivalDate = archivalDate;
            ArchivalStatus = archivalStatus;
            ArchivalCompressionLevel = archivalCompressionLevel;
        }
    }/*End of ArchiveResults struct*/
}/*End of Funny.Accents.Core.Compression.Model namespace*/
