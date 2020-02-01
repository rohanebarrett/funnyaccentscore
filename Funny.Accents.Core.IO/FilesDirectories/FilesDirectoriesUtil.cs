using System;
using System.Collections.Generic;
using System.IO;

namespace Funny.Accents.Core.Core.IO.FilesDirectories
{
    public class FilesDirectoriesUtil
    {
        private const string StringBlank = "";

        public static List<(bool processStatus, string filePath, Exception processException)>
            DeleteFileContaining(string targetDirectory, string searchValue)
        {
            var deletionList =
                new List<(bool processStatus, string filePath, Exception processException)>();

            var searchPattern = $"*{searchValue}*";
            var filesToDelete = Directory.EnumerateFiles(targetDirectory, searchPattern);
            foreach (var fileToDelete in filesToDelete)
            {
                try
                {
                    File.Delete(fileToDelete);
                    deletionList.Add((true, fileToDelete, null));
                }
                catch (Exception ex)
                {
                    deletionList.Add((false, fileToDelete, ex));
                }
            }

            return deletionList;
        }/*End of DeleteFileContaining method*/

        private static string FilePathCheck(string filePath)
        {
            return File.Exists(filePath) ? filePath : StringBlank;
        }

        private static string FileCheckFirstFound(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                var fileFound = FilePathCheck(filePath);
                if (string.IsNullOrWhiteSpace(fileFound)) { continue; }
                return fileFound;
            }
            return StringBlank;
        }

        //private static (bool exists, string path) DoesFileExists(string filePath)
        //{
        //    try
        //    {
        //        return File.Exists(filePath) ? (true,filePath) : (false, StringBlank);
        //    }
        //    catch (Exception)
        //    {
        //        return (false, StringBlank);
        //    }
        //}

        public static string ValidateFilePath(string filePath, bool checkApplicationDirectory = false)
        {
            return FileCheckFirstFound(checkApplicationDirectory
                ? new[] { filePath, Path.Combine(Directory.GetCurrentDirectory(), filePath) }
                : new[] { filePath });
        }
    }/*End of FilesDirectoriesUtil class*/
}/*End of CmkIoUtilities.FilesDirectories namespace*/
