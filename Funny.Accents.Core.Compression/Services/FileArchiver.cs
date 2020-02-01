using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Funny.Accents.Core.Compression.Interfaces;
using Funny.Accents.Core.Compression.Model;

namespace Funny.Accents.Core.Compression.Services
{
    public class FileArchiver : IFileArchiver
    {
        public IEnumerable<IArchiveResults> ArchiveFile(
            IEnumerable<IArchiveDefinition> archiveDefinition)
        {
            var arcResults = new List<IArchiveResults>();

            foreach (var archDef in archiveDefinition)
            {
                var zipFile = Path.Combine(
                    archDef.ZipLocation, archDef.ZipFileName);

                /*Check if zip file exists*/
                if (!File.Exists(zipFile))
                {
                    using (File.Create(zipFile)) { }
                }

                using (var zipToOpen
                    = new FileStream(zipFile, FileMode.Open))
                {
                    using (var archive
                        = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        /*Add the file to the archive*/
                        var fileInfo = new FileInfo(archDef.FileLocation);
                        archive.CreateEntryFromFile(
                            fileInfo.FullName, fileInfo.Name, archDef.CompressionLevel);

                        var archived = archive.Entries
                            .Any(entry => entry.Name.Equals(fileInfo.Name));

                        arcResults.Add(new ArchiveResults(
                            fileInfo.FullName, zipFile,
                            archived ? DateTime.Now : (DateTime?)null,
                            archived,
                            archDef.CompressionLevel.ToString()));

                        /*Remove the source file if specified*/
                        if (archDef.RemoveSourceFile && archived)
                        {
                            fileInfo.Delete();
                        }
                    }
                }
            }/*Add each file selected to the zip file*/

            return arcResults;
        }/*End of ArchiveFile method*/

        public IEnumerable<IArchiveResults> ArchiveDirectory(string dirToArchive,
            string zipPath,
            SearchOption searchOption = SearchOption.TopDirectoryOnly,
            Func<string, bool> validFiles = null,
            Func<string, string> customFileName = null,
            bool removeFiles = false,
            CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            /*Indicate source directory does not exist*/
            if (!Directory.Exists(dirToArchive))
            {
                throw new ArgumentException(
                    "The directory specified does not exist", dirToArchive);
            }

            /*Check if delegates/callbacks were supplied*/
            var valFile = validFiles ?? (file => true);
            var custFileName = customFileName ?? (fileName => fileName);

            /*Get the files in the directory that should be archived*/
            var files = Directory.EnumerateFiles(dirToArchive, "*.*", searchOption)
                .Where(valFile.Invoke)
                .ToArray()
                .Select(file => new ArchiveDefinition
                {
                    FileLocation = custFileName.Invoke(file),
                    ZipFileName = Path.GetFileName(zipPath),
                    ZipLocation = Path.GetDirectoryName(zipPath),
                    RemoveSourceFile = removeFiles,
                    CompressionLevel = compressionLevel
                });

            /*Archive the files*/
            return ArchiveFile(files);
        }/*End of ArchiveDirectory method*/
    }/*End of FileArchiver class*/
}/*End of Funny.Accents.Core.Compression.Services namespace*/
