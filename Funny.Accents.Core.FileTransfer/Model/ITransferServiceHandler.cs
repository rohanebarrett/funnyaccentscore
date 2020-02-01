using Funny.Accents.Core.Types.Results;
using System;
using System.Collections.Generic;

namespace Funny.Accents.Core.FileTransfer.Model
{
    public interface ITransferConnection
    {
        IProcessResult<T> Connect<T>(Func<bool, T> statusCheck);
        IProcessResult<T> CloseConnection<T>(Func<bool, T> statusCheck);
    }/*End of ITransferConnection interface*/

    public interface ITransferFile
    {
        IProcessResult<T> UploadFile<T>(string remoteFilePath,
            string remoteFileNameParam,
            string localFilePathParam, string localFileNameParam,
            Func<bool, T> statusCheck = null,
            bool overrideFile = false,
            Func<string, string> customFileName = null,
            bool closeConnection = false);
        IProcessResult<T> DownloadFile<T>(string remoteFilePath,
            string remoteFileNameParam,
            string localFilePathParam, string localFileNameParam,
            Func<bool, T> statusCheck = null,
            bool overrideFile = false,
            Func<string, string> customFileName = null,
            bool closeConnection = false);
    }/*End of ITransferFile interface*/

    public interface ITransferFileIo
    {
        //IProcessResult<T, List<string>> ListDirectoryContent<T>(string remoteDirectoryParam,
        //    Func<bool, T> statusCheck = null,
        //    Func<SftpFile,bool> extensionFilter = null,
        //    bool closeConnection = false);

        IProcessResult<T, List<string>> ListDirectoryContent<T>(string remoteDirectoryParam,
            Func<bool, T> statusCheck = null,
            Func<string, bool> extensionFilter = null,
            bool closeConnection = false);

        IProcessResult<T> CheckIfFileExists<T>(string remoteDirectoryParam, string fileNameParam,
            Func<bool, T> statusCheck = null,
            bool closeConnection = false);

        //IProcessResult<T, SftpFile> GetFile<T>(string remoteDirectoryParam, string fileNameParam,
        //    Func<bool, T> statusCheck = null,
        //    bool closeConnection = false);

        IProcessResult<T> DeleteFile<T>(string remoteDirectoryParam, string fileNameParam,
            Func<bool, T> statusCheck = null,
            bool closeConnection = false);
    }/*End of ITransferFileIo interface*/

    public interface ITransferService : ITransferConnection,
        ITransferFile, ITransferFileIo
    {
    }/*End of ITransferService interface*/
}/*End of CmkFileTransferUtilities.Model namespace*/
