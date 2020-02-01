using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Funny.Accents.Core.Core.Parsing.StringUtilities;
using Funny.Accents.Core.FileTransfer.Model;
using Funny.Accents.Core.Types.Results;

namespace Funny.Accents.Core.FileTransfer.Workers
{
    public class FtpWorker : ITransferService
    {
        private readonly string _ftpHost;
        private readonly string _ftpUserName;
        private readonly string _ftpPassword;

        private FtpWebRequest _ftpWebRequest;
        private string _uriValues = "";

        public FtpWorker(string ftpHost,
            string ftpUserName, string ftpPassword)
        {
            _ftpHost = ftpHost;
            _ftpUserName = ftpUserName;
            _ftpPassword = ftpPassword;
        }/*End of FtpWorker constructor*/

        public IProcessResult<T> Connect<T>(Func<bool, T> statusCheck)
        {
            try
            {
                var finalUri = StringManipulation.PathCombine(
                    _ftpHost, '/', paths: new[] { _uriValues });

                _ftpWebRequest = (FtpWebRequest)WebRequest.Create(finalUri);
                _ftpWebRequest.Credentials
                    = new NetworkCredential(_ftpUserName, _ftpPassword);
                _ftpWebRequest.Proxy = null;

                return ProcessResultWithState(true, statusCheck.Invoke(true), "Connection Successful");
            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , "Connection to the SFTP server failed because an exception occurred", ex);
            }
        }/*End of Connect method*/

        public IProcessResult<T> CloseConnection<T>(
            Func<bool, T> statusCheck)
        {
            try
            {
                if (_ftpWebRequest == null
                    || _ftpWebRequest.KeepAlive == false)
                {
                    return ProcessResultWithState(false, statusCheck.Invoke(false), "Disconnection Failed.");
                }

                _ftpWebRequest.KeepAlive = false;
                _ftpWebRequest = null;
                return ProcessResultWithState(true, statusCheck.Invoke(true), "Disconnection Successful.");
            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , "Disconnection Failed because an exception occurred", ex);
            }
        }/*End of CloseConnection method*/

        public IProcessResult<T> UploadFile<T>(string remoteFilePath,
            string remoteFileNameParam, string localFilePathParam,
            string localFileNameParam,
            Func<bool, T> statusCheck,
            bool overrideFile = false,
            Func<string, string> customFileName = null,
            bool closeConnection = false)
        {
            var fileName = customFileName == null
                ? remoteFileNameParam
                : customFileName.Invoke(remoteFileNameParam);
            _uriValues = StringManipulation.PathCombine(
                remoteFilePath, '/', fileName);

            var connectionResult = Connect(statusCheck);
            if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , connectionResult.ProcessResultMessage, connectionResult.ProcessResultException);
            }

            FtpWebResponse uploadResponse = null;
            try
            {
                var fullFilePath = Path.Combine(localFilePathParam, localFileNameParam);
                _ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;

                var requestStream = _ftpWebRequest.GetRequestStream();
                var fileStream = File.Open(fullFilePath, FileMode.Open);
                var buffer = new byte[1024];
                while (true)
                {
                    var bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    requestStream.Write(buffer, 0, bytesRead);
                }

                //The request stream must be closed before getting the response.
                requestStream.Close();
                uploadResponse = (FtpWebResponse)_ftpWebRequest.GetResponse();

                /*Check if the file was uploaded to the FTP server successfully*/
                var files = ListDirectoryContent(Connect(
                    _ftpHost, remoteFilePath, _ftpUserName, _ftpPassword));
                var existenceCheck = files.Any(file => file.Equals(fileName));

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
        }/*End of UploadFile method*/

        public IProcessResult<T> DownloadFile<T>(string remoteFilePath,
            string remoteFileNameParam, string localFilePathParam,
            string localFileNameParam,
            Func<bool, T> statusCheck,
            bool overrideFile = false,
            Func<string, string> customFileName = null,
            bool closeConnection = false)
        {
            var fileName = customFileName == null
                ? remoteFileNameParam
                : customFileName.Invoke(remoteFileNameParam);
            _uriValues = StringManipulation.PathCombine(
                remoteFilePath, '/', fileName);

            var connectionResult = Connect(statusCheck);
            if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , connectionResult.ProcessResultMessage, connectionResult.ProcessResultException);
            }

            try
            {
                _ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                var response = (FtpWebResponse)_ftpWebRequest.GetResponse();

                var fullFilePath = Path.Combine(localFilePathParam, localFileNameParam);

                /*Write the stream to a file*/
                using (var responseStream = response.GetResponseStream())
                {
                    using (var output =
                        new FileStream(fullFilePath,
                            overrideFile ? FileMode.Create : FileMode.CreateNew))
                    {
                        if (responseStream.CanSeek)
                        {
                            responseStream.Position = 0;
                        }
                        responseStream.CopyTo(output);
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
                    , connectionResult.ProcessResultMessage, ex);
            }
        }/*End of DownloadFile method*/

        public IProcessResult<T, List<string>> ListDirectoryContent<T>(
            string remoteDirectoryParam,
            Func<bool, T> statusCheck,
            Func<string, bool> extensionFilter = null,
            bool closeConnection = false)
        {
            try
            {
                _uriValues = remoteDirectoryParam;

                var connectionResult = Connect(statusCheck);
                if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
                {
                    return ProcessResultWithReturnValue<T, List<string>>(false, statusCheck.Invoke(false)
                        , processResultMessage: connectionResult.ProcessResultMessage
                        , processException: connectionResult.ProcessResultException);
                }

                var names = ListDirectoryContent(_ftpWebRequest);

                /*close connection if requested*/
                if (closeConnection) { CloseConnection(); }

                return ProcessResultWithReturnValue(true, statusCheck.Invoke(true)
                    , names
                    , $"{names.Count} file" + (names.Count == 1 ? "" : "s") +
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
        }/*End of ListDirectoryContent method*/

        public IProcessResult<T> CheckIfFileExists<T>(
            string remoteDirectoryParam, string fileNameParam,
            Func<bool, T> statusCheck,
            bool closeConnection = false)
        {
            _uriValues = remoteDirectoryParam;

            var connectionResult = Connect(statusCheck);
            if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , connectionResult.ProcessResultMessage, connectionResult.ProcessResultException);
            }

            /*Check if file was removed from FTP server*/
            var isExist = CheckIfFileExists(Connect(_ftpHost,
                remoteDirectoryParam, _ftpUserName, _ftpPassword),
                fileNameParam);

            var fileToCheck = StringManipulation.PathCombine(
                _ftpHost, '/', new[] { remoteDirectoryParam, fileNameParam });
            /*Return response back to calling application*/
            /*close connection if requested*/
            if (closeConnection) { CloseConnection(); }

            /*Indicate to the calling process that the indicated file was not found*/
            return ProcessResultWithState(isExist, statusCheck.Invoke(isExist)
                , isExist
                    ? $"Exists: The file: {fileToCheck} was found on the server"
                    : $"Absent: The file: {fileToCheck} was NOT found on the server");
        }/*End of CheckIfFileExists method*/

        public IProcessResult<T> DeleteFile<T>(
            string remoteDirectoryParam,
            string fileNameParam, Func<bool, T> statusCheck,
            bool closeConnection = false)
        {
            try
            {
                _uriValues = StringManipulation.PathCombine(
                    remoteDirectoryParam, '/', fileNameParam);

                var connectionResult = Connect(statusCheck);
                if (connectionResult.ProcessResultValue.Equals(statusCheck.Invoke(true)) == false)
                {
                    return ProcessResultWithState(false, statusCheck.Invoke(false)
                        , connectionResult.ProcessResultMessage, connectionResult.ProcessResultException);
                }

                _ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                var response = (FtpWebResponse)_ftpWebRequest.GetResponse();

                var filePath = _ftpWebRequest.RequestUri;
                var status = response.StatusCode;
                var description = response.StatusDescription;

                response.Close();

                /*Check if file was removed from FTP server*/
                var isExist = CheckIfFileExists(Connect(_ftpHost,
                        remoteDirectoryParam, _ftpUserName, _ftpPassword),
                    fileNameParam);

                /*close connection if requested*/
                if (closeConnection) { CloseConnection(); }

                /*Indicate to the calling process that the indicated file was not found*/
                return ProcessResultWithState(!isExist, statusCheck.Invoke(!isExist)
                    , !isExist
                        ? $"Success: The file: {filePath} was deleted | FTP Status: {status} | FTP Message: {description}"
                        : $"Failure: The file: {filePath} was NOT deleted| FTP Status: {status} | FTP Message: {description}");
            }
            catch (Exception ex)
            {
                return ProcessResultWithState(false, statusCheck.Invoke(false)
                    , $"The file: {fileNameParam} failed to be deleted because an exception occurred", ex);
            }
        }/*End of DeleteFile method*/

        private bool CloseConnection()
        {
            try
            {
                if (_ftpWebRequest == null
                    || _ftpWebRequest.KeepAlive == false)
                {
                    return true;
                }

                _ftpWebRequest.KeepAlive = false;
                _ftpWebRequest = null;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }/*End of CloseConnection method*/

        private static FtpWebRequest Connect(string ftpHost,
            string uriValues, string ftpUserName, string ftpPassword)
        {
            try
            {
                var finalUri = StringManipulation.PathCombine(
                    ftpHost, '/', uriValues);

                var ftpWebRequest = (FtpWebRequest)WebRequest.Create(finalUri);
                ftpWebRequest.Credentials
                    = new NetworkCredential(ftpUserName, ftpPassword);
                ftpWebRequest.Proxy = null;

                return ftpWebRequest;
            }
            catch (Exception ex)
            {
                return null;
            }
        }/*End of Connect private method*/

        private static List<string> ListDirectoryContent(FtpWebRequest ftpWebRequest)
        {
            ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;

            var response = (FtpWebResponse)ftpWebRequest.GetResponse();
            var responseStream = response.GetResponseStream();

            var reader = new StreamReader(responseStream);
            var names = reader.ReadToEnd();

            reader.Close();
            response.Close();
            return names.Split(new string[] { "\r\n" },
                StringSplitOptions.RemoveEmptyEntries).ToList();
        }/*End of ListDirectoryContent private method*/

        private static bool CheckIfFileExists(
            FtpWebRequest ftpWebRequest, string fileName)
        {
            /*Check if the file was uploaded to the FTP server successfully*/
            var files = ListDirectoryContent(ftpWebRequest);
            var existenceCheck = files.Any(file => file.Equals(fileName));
            return existenceCheck;
        }/*End of CheckIfFileExists private method*/

        private static IProcessResult<T> ProcessResultWithState<T>(
            bool completionStatus, T processState
            , string processResultMessage = null
            , Exception processException = null)
        {
            return new ProcessResult<T>
            {
                ProcessCompletionState = completionStatus,
                ProcessResultValue = processState,
                ProcessResultMessage = processResultMessage,
                ProcessResultException = processException
            };
        }/*End of ProcessResult private method*/

        private static IProcessResult<T, TK> ProcessResultWithReturnValue<T, TK>(
            bool completionStatus, T processState
            , TK processResultValue = default
            , string processResultMessage = null
            , Exception processException = null)
        {
            return new ProcessResult<T, TK>
            {
                ProcessCompletionState = completionStatus,
                ProcessStateValue = processState,
                ProcessResultValue = processResultValue,
                ProcessResultMessage = processResultMessage,
                ProcessResultException = processException
            };
        }/*End of ProcessResult private method*/
    }/*End of FtpWorker class*/
}/*End of Funny.Accents.Core.FileTransfer.Workers namespace*/
