using System.Collections.Generic;
using Funny.Accents.Core.Types.Results;
using Funny.Accents.Core.FileTransfer.Abstractions.Concretions;
using Funny.Accents.Core.FileTransfer.Abstractions.Interfaces;
using Funny.Accents.Core.FileTransfer.Enum;
using Funny.Accents.Core.FileTransfer.Model;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Abstract
{
    public abstract class TransferServiceDirector
        : ITransferServiceDirector
    {
        private static ITransferService GetTransferService(
            TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            return new ConcreteTransferServiceFactory(
                protocol, transmissionDetails.ConnectionDetails)
                .GetService();
        }/*End of GetTransferService method*/

        public IProcessResult DownloadFile(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            switch (protocol)
            {
                case TransferProtocol.Sftp:
                case TransferProtocol.Ftp:
                    return DownloadFileViaFileTransfer(protocol, transmissionDetails);
                default:
                    throw new System.ArgumentException($"Invalid Transfer Protocol: {protocol}");
            }
        }/*End of DownloadFile method*/

        public IProcessResult UploadFile(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            switch (protocol)
            {
                case TransferProtocol.Sftp:
                case TransferProtocol.Ftp:
                    return UploadFileViaFileTransfer(protocol, transmissionDetails);
                default:
                    throw new System.ArgumentException($"Invalid Transfer Protocol: {protocol}");
            }
        }/*End of UploadFile method*/

        public IProcessResult RemoveFile(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            switch (protocol)
            {
                case TransferProtocol.Sftp:
                case TransferProtocol.Ftp:
                    return RemoveFileTransfer(protocol, transmissionDetails);
                default:
                    throw new System.ArgumentException($"Invalid Transfer Protocol: {protocol}");
            }
        }/*End of RemoveFile method*/

        public IProcessResult FileExists(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            switch (protocol)
            {
                case TransferProtocol.Sftp:
                case TransferProtocol.Ftp:
                    return FileExistsFileTransfer(protocol, transmissionDetails);
                default:
                    throw new System.ArgumentException($"Invalid Transfer Protocol: {protocol}");
            }
        }/*End of FileExists method*/

        public IProcessResult<IEnumerable<string>> ListFiles(
            TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            switch (protocol)
            {
                case TransferProtocol.Sftp:
                case TransferProtocol.Ftp:
                    return ListFilesFileTransfer(protocol, transmissionDetails);
                default:
                    throw new System.ArgumentException($"Invalid Transfer Protocol: {protocol}");
            }
        }/*End of ListFiles method*/

        private IProcessResult DownloadFileViaFileTransfer(
            TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            TransferProtocolProcessState StateCheck(bool p) => p
                ? TransferProtocolProcessState.Success
                : TransferProtocolProcessState.DownloadFailed;

            var fileProtocolWorker
                = GetTransferService(protocol, transmissionDetails);
            return fileProtocolWorker.DownloadFile(
                transmissionDetails.TransferDetails.SourceDirectory,
                transmissionDetails.TransferDetails.SourceFileName,
                transmissionDetails.TransferDetails.DestinationDirectory,
                transmissionDetails.TransferDetails.DestinationFileName,
                StateCheck,
                transmissionDetails.TransferDetails.OverrideExisting);
        }/*End of DownloadFileViaFileTransfer method*/

        private IProcessResult UploadFileViaFileTransfer(
            TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            TransferProtocolProcessState StateCheck(bool p) => p
                ? TransferProtocolProcessState.Success
                : TransferProtocolProcessState.DownloadFailed;

            var fileProtocolWorker
                = GetTransferService(protocol, transmissionDetails);
            return fileProtocolWorker.UploadFile(
                transmissionDetails.TransferDetails.DestinationDirectory,
                transmissionDetails.TransferDetails.DestinationFileName,
                transmissionDetails.TransferDetails.SourceDirectory,
                transmissionDetails.TransferDetails.SourceFileName,
                StateCheck,
                transmissionDetails.TransferDetails.OverrideExisting,
                closeConnection: transmissionDetails.ConnectionDetails.DisconnectOnComplete);
        }/*End of UploadFileViaFileTransfer method*/

        private IProcessResult RemoveFileTransfer(
            TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            TransferProtocolProcessState StateCheck(bool p) => p
                ? TransferProtocolProcessState.Success
                : TransferProtocolProcessState.DeleteFileFailed;

            var fileProtocolWorker
                = GetTransferService(protocol, transmissionDetails);
            return fileProtocolWorker.DeleteFile(
                transmissionDetails.FileIoDetails.DirectoryName,
                transmissionDetails.FileIoDetails.FileName,
                StateCheck);
        }/*End of RemoveFileTransfer method*/

        private IProcessResult FileExistsFileTransfer(
            TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            TransferProtocolProcessState StateCheck(bool p) => p
                ? TransferProtocolProcessState.Success
                : TransferProtocolProcessState.CheckExistsFailed;

            var fileProtocolWorker
                = GetTransferService(protocol, transmissionDetails);
            return fileProtocolWorker.CheckIfFileExists(
                transmissionDetails.FileIoDetails.DirectoryName,
                transmissionDetails.FileIoDetails.FileName,
                StateCheck);
        }/*End of FileExistsFileTransfer method*/

        private IProcessResult<IEnumerable<string>> ListFilesFileTransfer(
            TransferProtocol protocol,
            ITransmissionDetails transmissionDetails)
        {
            TransferProtocolProcessState StateCheck(bool p) => p
                ? TransferProtocolProcessState.Success
                : TransferProtocolProcessState.ListDirectoryFailed;

            var fileProtocolWorker
                = GetTransferService(protocol, transmissionDetails);
            var result = fileProtocolWorker.ListDirectoryContent(
                transmissionDetails.FileIoDetails.DirectoryName,
                StateCheck, file => true,
                transmissionDetails.ConnectionDetails.DisconnectOnComplete);
            return new ProcessResult<IEnumerable<string>>
            {
                ProcessCompletionState = result.ProcessCompletionState,
                ProcessResultException = result.ProcessResultException,
                ProcessResultMessage = result.ProcessResultMessage,
                ProcessResultValue = result.ProcessResultValue
            };
        }/*End of ListFilesFileTransfer method*/
    }/*End of TransferServiceDirector abstract class*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Abstract namespace*/
