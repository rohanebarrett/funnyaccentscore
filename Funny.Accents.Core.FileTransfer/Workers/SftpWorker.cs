using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Funny.Accents.Core.Core.Parsing.StringUtilities;
using Funny.Accents.Core.FileTransfer.Model;
using Funny.Accents.Core.Types.Results;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace Funny.Accents.Core.FileTransfer.Workers
{
    public class SftpWorker : ITransferService
    {
        private readonly string _sftpHost;
        private readonly string _sftpUserName;
        private readonly string _sftpPassword;
        private readonly int _sftpPort;

        private SftpClient SftpConnection { get; set; }

        /*Constructor for SFTPManager class used to create a connection to the remote SFTP server*/
        public SftpWorker(string hostParam, int portParam,
            string userNameParam, string passwordParam)
        {
            _sftpHost = hostParam;
            _sftpUserName = userNameParam;
            _sftpPassword = passwordParam;
            _sftpPort = portParam;
        }

        public IProcessResult<T> Connect<T>(Func<bool, T> statusCheck)
        {
            try
            {
                if (SftpConnection?.IsConnected == true)
                {
                    return ProcessResultWithState(true, statusCheck.Invoke(true), "Connection Successful");
                }

                SftpConnection = new SftpClient(new PasswordConnectionInfo(
                    _sftpHost, _sftpPort, _sftpUserName, _sftpPassword));
                //ProxyTypes.Http, "milproxy01.cmk.ca", 8080));
                SftpConnection.Connect();

                return ProcessResultWithState(true, statusCheck.Invoke(true), "Connection Successful");
            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , "Connection to the FTP server failed because an exception occurred", ex);
            }
        }/*End of checkSFTPConnection method*/

        /*Checks if a file exists in an SFTP directory*/
        public IProcessResult<T> CheckIfFileExists<T>(string remoteDirectoryParam,
            string fileNameParam, Func<bool, T> statusCheck,
            bool closeConnection = false)
        {
            try
            {
                /*Test the connection to the SFTP site if one does not exist then attempt to establish a new connection*/
                var connectionResult = Connect(statusCheck);
                if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
                {
                    return ProcessResultWithState(false, statusCheck.Invoke(false)
                        , connectionResult.ProcessResultMessage, connectionResult.ProcessResultException);
                }

                var remoteFilePath = StringManipulation.PathCombine(
                    remoteDirectoryParam, paths: new[] { fileNameParam });

                var fileExists = SftpConnection.Exists(remoteFilePath);

                /*close connection if requested*/
                if (closeConnection) { CloseConnection(); }

                return ProcessResultWithState(fileExists
                    , statusCheck.Invoke(fileExists)
                    , fileExists
                        ? $"File: {fileNameParam} was found in directory: {remoteDirectoryParam}"
                        : $"File: {fileNameParam} was NOT found in directory: {remoteDirectoryParam}");

                /*Indicate to the calling process that the indicated file was not found*/
            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , "Failed to validate the existence of the file because an exception occurred", ex);
            }
        } /*End of checkIfFileExists method*/

        /*Checks if a file exists in an SFTP directory*/
        private IProcessResult<T, SftpFile> GetFile<T>(string remoteDirectoryParam,
            string fileNameParam, Func<bool, T> statusCheck,
            bool closeConnection = false)
        {
            try
            {
                /*Test the connection to the SFTP site if one does not exist then attempt to establish a new connection*/
                var connectionResult = Connect(statusCheck);
                if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
                {
                    return ProcessResultWithReturnValue<T, SftpFile>(false
                       , statusCheck.Invoke(false)
                       , processResultMessage: connectionResult.ProcessResultMessage
                       , processException: connectionResult.ProcessResultException);
                }

                //var files = SftpConnection.ListDirectory(remoteDirectoryParam);
                var sftpFile = SftpConnection.Get($"{remoteDirectoryParam}{fileNameParam}"); //files.FirstOrDefault(file => file.FullName.Contains(fileNameParam));

                /*close connection if requested*/
                if (closeConnection) { CloseConnection(); }

                return ProcessResultWithReturnValue(true
                        , statusCheck.Invoke(true)
                        , sftpFile, $"Successfully retrieve file: {fileNameParam}");
            }
            catch (Exception ex)
            {
                return ProcessResultWithReturnValue<T, SftpFile>(false
                    , statusCheck.Invoke(false)
                    , processResultMessage: $"Failed to retrieve the file: {fileNameParam} because an exception occurred."
                    , processException: ex);
            }
        } /*End of checkIfFileExists method*/

        public IProcessResult<T> DeleteFile<T>(string remoteDirectoryParam, string fileNameParam,
            Func<bool, T> statusCheck,
            bool closeConnection = false)
        {
            try
            {
                /*Test the connection to the SFTP site if one does not exist then attempt to establish a new connection*/
                var connectionResult = Connect(statusCheck);
                if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
                {
                    return ProcessResultWithState(false, statusCheck.Invoke(false)
                        , connectionResult.ProcessResultMessage, connectionResult.ProcessResultException);
                }

                var remoteFilePath = StringManipulation.PathCombine(
                    remoteDirectoryParam, paths: new[] { fileNameParam });

                var fileToDelete = SftpConnection.Get(remoteFilePath);
                fileToDelete.Delete();

                /*close connection if requested*/
                if (closeConnection) { CloseConnection(); }

                /*Indicate to the calling process that the indicated file was not found*/
                return ProcessResultWithState(true, statusCheck.Invoke(true)
                    , $"The file: {fileToDelete.Name} was deleted successfully");
            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , $"The file: {fileNameParam} failed to be deleted because an exception occurred", ex);
            }
        } /*End of checkIfFileExists method*/

        /*Downloads a file from an SFTP server and writes it to another network location*/
        public IProcessResult<T> DownloadFile<T>(string remoteFilePath, string remoteFileNameParam,
            string localFilePathParam, string localFileNameParam,
            Func<bool, T> statusCheck, bool overrideFile = false,
            Func<string, string> customFileName = null,
            bool closeConnection = false)
        {
            try
            {
                /*Test the connection to the SFTP site if one does not exist then attempt to establish a new connection*/
                var connectionResult = Connect(statusCheck);
                if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
                {
                    return ProcessResultWithState(false, statusCheck.Invoke(false)
                        , connectionResult.ProcessResultMessage, connectionResult.ProcessResultException);
                }

                //var fullFilePath = Path.Combine(localFilePathParam, localFileNameParam);
                var fullFilePath = Path.Combine(localFilePathParam
                                        , customFileName == null
                                            ? localFileNameParam
                                            : customFileName.Invoke(localFileNameParam));

                /*Create the directory to store the file*/
                Directory.CreateDirectory(localFilePathParam);

                var destinationFilePath = StringManipulation.PathCombine(
                    remoteFilePath, paths: new[] { remoteFileNameParam });

                var fileExists = SftpConnection.Exists(destinationFilePath);

                if (fileExists)
                {
                    using (var file = File.Open(fullFilePath, overrideFile ? FileMode.Create : FileMode.CreateNew))
                    {
                        SftpConnection.DownloadFile(destinationFilePath, file);
                    }
                }

                /*Check to see if the file was successfully created*/
                var downloadResult = File.Exists(fullFilePath);

                /*close connection if requested*/
                if (closeConnection) { CloseConnection(); }

                return ProcessResultWithState(downloadResult
                    , statusCheck.Invoke(downloadResult)
                    , downloadResult
                        ? $"The file: {remoteFileNameParam} was downloaded successfully"
                        : $"The file: {remoteFileNameParam} was NOT downloaded successfully");

            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , $"The file: {remoteFileNameParam} was not downloaded because and exception occurred.", ex);
            }
        } /*End of downloadFile method*/

        public IProcessResult<T> UploadFile<T>(string remoteFilePath, string remoteFileNameParam,
            string localFilePathParam, string localFileNameParam, Func<bool, T> statusCheck,
            bool overrideFile = false, Func<string, string> customFileName = null,
            bool closeConnection = false)
        {
            try
            {
                /*Test the connection to the SFTP site if one does not exist then attempt to establish a new connection*/
                var connectionResult = Connect(statusCheck);
                if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
                {
                    return ProcessResultWithState(false, statusCheck.Invoke(false)
                        , connectionResult.ProcessResultMessage, connectionResult.ProcessResultException);
                }

                var fullFilePath = Path.Combine(localFilePathParam, localFileNameParam);
                var cstFileName = customFileName == null
                    ? remoteFileNameParam
                    : customFileName.Invoke(remoteFileNameParam);

                var destinationFilePath = StringManipulation.PathCombine(
                    remoteFilePath, paths: new[] { cstFileName });

                using (var fileStream = new FileStream(fullFilePath, FileMode.Open))
                {
                    SftpConnection.UploadFile(fileStream, destinationFilePath, overrideFile);
                }

                /*Check if the file was uploaded to the SFTP server successfully*/
                var files = SftpConnection.ListDirectory(remoteFilePath);
                var existenceCheck = files.Any(file => file.FullName.Contains(cstFileName));

                /*close connection if requested*/
                if (closeConnection)
                {
                    CloseConnection();
                }

                return ProcessResultWithState(existenceCheck
                    , statusCheck.Invoke(existenceCheck)
                    , existenceCheck
                        ? $"The file: {remoteFileNameParam} was uploaded successfully"
                        : $"The file: {remoteFileNameParam} was NOT uploaded successfully");
            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , $"The file: {remoteFileNameParam} was NOT uploaded successfully because an exception occurred", ex);
            }
        } /*End of downloadFile method*/

        //public IProcessResult<T,List<string>> ListDirectoryContent<T>(string remoteDirectoryParam,
        //    Func<bool, T> statusCheck, Func<SftpFile,bool> extensionFilter,
        //    bool closeConnection = false)
        //{
        //    var directorList = new List<string>();
        //    try
        //    {
        //        /*Test the connection to the SFTP site if one does not exist then attempt to establish a new connection*/
        //        var connectionResult = Connect(statusCheck);
        //        if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
        //        {
        //            return ProcessResultWithReturnValue<T,List<string>>(false, statusCheck.Invoke(false)
        //                , processResultMessage: connectionResult.ProcessResultMessage
        //                , processException: connectionResult.ProcessResultException);
        //        }

        //        directorList.AddRange(SftpConnection.ListDirectory(remoteDirectoryParam)
        //            .Where(extensionFilter)
        //            .Select(file => file.Name)
        //            .ToList());

        //        /*close connection if requested*/
        //        if (closeConnection) { CloseConnection(); }

        //        return ProcessResultWithReturnValue(true, statusCheck.Invoke(true)
        //            , directorList
        //            , $"{directorList.Count} file" + (directorList.Count == 1 ? "" : "s") +
        //              $" exist in directory: {remoteDirectoryParam}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ProcessResultWithReturnValue<T,List<string>>(false
        //            , statusCheck.Invoke(false)
        //            , processResultMessage: $"Failed to list files from directory {remoteDirectoryParam} " +
        //                                    "because an exception occurred"
        //            , processException: ex);
        //    }
        //} /*End of checkIfFileExists method*/

        public IProcessResult<T, List<string>> ListDirectoryContent<T>(string remoteDirectoryParam,
            Func<bool, T> statusCheck, Func<string, bool> extensionFilter = null,
            bool closeConnection = false)
        {
            var directorList = new List<string>();
            try
            {
                /*Test the connection to the SFTP site if one does not exist then attempt to establish a new connection*/
                var connectionResult = Connect(statusCheck);
                if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
                {
                    return ProcessResultWithReturnValue<T, List<string>>(false, statusCheck.Invoke(false)
                        , processResultMessage: connectionResult.ProcessResultMessage
                        , processException: connectionResult.ProcessResultException);
                }

                directorList.AddRange(SftpConnection.ListDirectory(remoteDirectoryParam)
                    .Select(file => file.Name)
                    .Where(file => extensionFilter?.Invoke(file) ?? true)
                    .ToList());

                /*close connection if requested*/
                if (closeConnection) { CloseConnection(); }

                return ProcessResultWithReturnValue(true, statusCheck.Invoke(true)
                    , directorList
                    , $"{directorList.Count} file" + (directorList.Count == 1 ? "" : "s") +
                      $" exist in directory: {remoteDirectoryParam}");
            }
            catch (Exception ex)
            {
                return ProcessResultWithReturnValue<T, List<string>>(false
                    , statusCheck.Invoke(false)
                    , processResultMessage: $"Failed to list files from directory {remoteDirectoryParam} " +
                                            "because an exception occurred"
                    , processException: ex);
            }
        } /*End of checkIfFileExists method*/

        public IProcessResult<T> CloseConnection<T>(Func<bool, T> statusCheck)
        {
            try
            {
                if (SftpConnection?.IsConnected != true)
                {
                    return ProcessResultWithState(false, statusCheck.Invoke(false), "Disconnection Failed.");
                }

                SftpConnection.Disconnect();
                return ProcessResultWithState(true, statusCheck.Invoke(true), "Disconnection Successful.");
            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , "Disconnection Failed because an exception occurred", ex);
            }
        }

        private bool CloseConnection()
        {
            try
            {
                if (SftpConnection?.IsConnected != true)
                {
                    return true;
                }

                SftpConnection.Disconnect();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }/*End of CloseConnection method*/

        private static IProcessResult<T> ProcessResultWithState<T>(
            bool completionStatus, T processState
            , string processResultMessage = null
            , Exception processException = null)
        {
            return new ProcessResult<T>
            {
                ProcessCompletionState = completionStatus
                ,
                ProcessResultValue = processState
                ,
                ProcessResultMessage = processResultMessage
                ,
                ProcessResultException = processException
            };
        }/*End of ProcessResult method*/

        private static IProcessResult<T, TK> ProcessResultWithReturnValue<T, TK>(
            bool completionStatus, T processState
            , TK processResultValue = default
            , string processResultMessage = null
            , Exception processException = null)
        {
            return new ProcessResult<T, TK>
            {
                ProcessCompletionState = completionStatus
                ,
                ProcessStateValue = processState
                ,
                ProcessResultValue = processResultValue
                ,
                ProcessResultMessage = processResultMessage
                ,
                ProcessResultException = processException
            };
        }/*End of ProcessResult method*/

    }/*End of SftpWorker class*/
}/*End of CmkFileTransferUtilities.Workers namespace*/
