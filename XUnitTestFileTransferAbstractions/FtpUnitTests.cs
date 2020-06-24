using Funny.Accents.Core.FileTransfer.Abstractions.Concretions;
using Funny.Accents.Core.FileTransfer.Abstractions.Models;
using Funny.Accents.Core.FileTransfer.Enum;
using Funny.Accents.Core.FileTransfer.Workers;
using Xunit;

namespace XUnitTestFileTransferAbstractions
{
    public class FtpUnitTests
    {
        private readonly ConnectionDetails _conDetails = new ConnectionDetails
        {
            HostName = "ftp://inventive.momentum.com",
            Port = 21,
            UserName = "comarkftp",
            Password = "ftp@0mark"
        };

        private readonly TransferDetails _downloadDetails = new TransferDetails
        {
            SourceDirectory = "",
            SourceFileName = "XPOLLD09800001-Test",
            DestinationDirectory = @"J:\temp\TransferTest",
            DestinationFileName = "FtpRohanTest",
            WorkingDirectory = @"J:\temp\TransferTest",
            OverrideExisting = true
        };

        private readonly TransferDetails _uploadDetails = new TransferDetails
        {
            SourceDirectory = @"J:\temp\TransferTest",
            SourceFileName = "RohanTest",
            DestinationDirectory = "",
            DestinationFileName = "XPOLLD09800001-Test",
            WorkingDirectory = @"J:\temp\TransferTest"
        };

        private readonly FileIoDetails _fileIoDetails = new FileIoDetails
        {
            DirectoryName = "",
            FileName = "XPOLLD09800001-Test"
        };

        [Fact]
        public void AdHoc_Test()
        {
            string StateCheck(bool p) => p
                ? "success"
                : "failed";

            var ftpWorker = new FtpWorker("host",
                "username",
                "password");

            var results = ftpWorker.DownloadFile(
                "remote directory location",
                "remote file name",
                "directory to save the file",
                "the name of the file",
                StateCheck,true,null,true);
        }

        [Fact]
        public void TestSftpDownload()
        {
            var srcTransDirector = new ConcreteTransferServiceDirector();

            var result = srcTransDirector.DownloadFile(TransferProtocol.Ftp,
                new TransmissionDetails
                {
                    ConnectionDetails = _conDetails,
                    TransferDetails = _downloadDetails
                });

            Assert.True(result.ProcessCompletionState, "The file was uploaded to the SFTP server");
        }/*End of TestSftpDownload method*/

        [Fact]
        public void Test_Ftp_File_Exists()
        {
            var srcTransDirector = new ConcreteTransferServiceDirector();

            var result = srcTransDirector.FileExists(TransferProtocol.Ftp,
                new TransmissionDetails
                {
                    ConnectionDetails = _conDetails,
                    FileIoDetails = _fileIoDetails
                });

            Assert.True(result.ProcessCompletionState,
                "The was found on the server");
        }/*End of TestSftpDownload method*/

        [Fact]
        public void Test_Ftp_Directory_Content()
        {
            
        }/*End of TestSftpDirectoryContent method*/

        [Fact]
        public void Test_Ftp_Upload()
        {
            var srcTransDirector = new ConcreteTransferServiceDirector();

            var result = srcTransDirector.UploadFile(TransferProtocol.Ftp,
                new TransmissionDetails
                {
                    ConnectionDetails = _conDetails,
                    TransferDetails = _uploadDetails
                });

            Assert.True(result.ProcessCompletionState, "The file was uploaded to the SFTP server");
        }/*End of TestSftpDownload method*/

        [Fact]
        public void Test_Ftp_Remove()
        {
            var srcTransDirector = new ConcreteTransferServiceDirector();

            var result = srcTransDirector.RemoveFile(TransferProtocol.Ftp,
                new TransmissionDetails
                {
                    ConnectionDetails = _conDetails,
                    FileIoDetails = _fileIoDetails
                });

            Assert.True(result.ProcessCompletionState,
                "The file was deleted from the FTP sever");
        }/*End of TestSftpDirectoryContent method*/
    }/*End of SftpUnitTests class*/
}/*End of XUnitTestFileTransferAbstractions namespace*/
