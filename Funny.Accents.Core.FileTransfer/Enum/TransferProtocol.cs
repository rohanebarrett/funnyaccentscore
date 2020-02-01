namespace Funny.Accents.Core.FileTransfer.Enum
{
    public enum TransferProtocol
    {
        Ftp = 0,
        Sftp = 1
    }/*End of TransferProtocol class*/

    public enum TransferProtocolProcess
    {
        Connect = 0,
        Disconnect = 1,
        ListDirectory = 2,
        CheckIfFileExists = 3,
        GetFile = 4,
        DeleteFile = 5,
        DownloadFile = 6,
        UploadFile = 7
    }/*End of TransferProtocolProcess class*/

    public enum TransferProtocolProcessState
    {
        Success = 0,
        Failure = 1,
        ConnectionFailed = 2,
        DisconnectionFailed = 3,
        DownloadFailed = 4,
        UploadFailed = 5,
        CheckExistsFailed = 6,
        ListDirectoryFailed = 7,
        GetFileFailed = 8,
        DeleteFileFailed = 9
    }/*End of TransferProtocolProcessState enum*/

}/*End of CmkUtilities.FileTransfer.Enum namespace*/
