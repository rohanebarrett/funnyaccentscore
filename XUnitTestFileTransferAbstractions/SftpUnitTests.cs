using Funny.Accents.Core.FileTransfer.Abstractions.Concretions;
using Funny.Accents.Core.FileTransfer.Abstractions.Models;
using Funny.Accents.Core.FileTransfer.Enum;
using Xunit;

namespace XUnitTestFileTransferAbstractions
{
    public class SftpUnitTests
    {
        private readonly ConnectionDetails _conDetails = new ConnectionDetails
        {
            HostName = "10.128.183.41",
            Port = 22,
            UserName = "comark",
            Password = "salesaudit"
        };

        private readonly TransferDetails _downloadDetails = new TransferDetails
        {
            SourceDirectory = "/home/comark/salesaudit/AWM.08151600.IP",
            SourceFileName = "XPOLLD09800001",
            DestinationDirectory = @"J:\temp\TransferTest",
            DestinationFileName = "RohanTest",
            WorkingDirectory = @"J:\temp\TransferTest"
        };

        private readonly TransferDetails _uploadDetails = new TransferDetails
        {
            SourceDirectory = @"J:\temp\TransferTest",
            SourceFileName = "RohanTest",
            DestinationDirectory = "/home/comark/salesaudit/AWM.08151600.IP",
            DestinationFileName = "XPOLLD09800001-Test",
            WorkingDirectory = @"J:\temp\TransferTest"
        };

        private readonly FileIoDetails _fileIoDetails = new FileIoDetails
        {
            DirectoryName = "/home/comark/salesaudit/AWM.08151600.IP",
            FileName = "XPOLLD09800001"
        };

        [Fact]
        public void TestSftpDownload()
        {
            var srcTransDirector = new ConcreteTransferServiceDirector();

            var result = srcTransDirector.DownloadFile(TransferProtocol.Sftp, new TransmissionDetails
            {
                ConnectionDetails = _conDetails,
                TransferDetails = _downloadDetails
            });

            Assert.True(result.ProcessCompletionState,"File downloaded successfully");
        }/*End of TestSftpDownload method*/

        [Fact]
        public void TestSftpFileExists()
        {
            var srcTransDirector = new ConcreteTransferServiceDirector();

            var result = srcTransDirector.FileExists(TransferProtocol.Sftp,
                new TransmissionDetails
            {
                FileIoDetails = _fileIoDetails,
                ConnectionDetails = _conDetails
            });

            Assert.True(result.ProcessCompletionState, "File exists on the SFTP");
        }/*End of TestSftpDownload method*/

        [Fact]
        public void TestSftpDirectoryContent()
        {
            var srcTransDirector = new ConcreteTransferServiceDirector();

            var result = srcTransDirector.ListFiles(TransferProtocol.Sftp,
                new TransmissionDetails
                {
                    FileIoDetails = _fileIoDetails,
                    ConnectionDetails = _conDetails
                });

            Assert.Contains(result.ProcessResultValue,file => file.Equals("XPOLLD09800001"));
        }/*End of TestSftpDirectoryContent method*/

        [Fact]
        public void TestSftpUpload()
        {
            var srcTransDirector = new ConcreteTransferServiceDirector();

            var result = srcTransDirector.UploadFile(TransferProtocol.Sftp,
                new TransmissionDetails
                {
                    ConnectionDetails = _conDetails,
                    TransferDetails = _uploadDetails
                });

            Assert.True(result.ProcessCompletionState, "The file was uploaded to the SFTP server");
        }/*End of TestSftpDownload method*/
    }/*End of SftpUnitTests class*/
}/*End of XUnitTestFileTransferAbstractions namespace*/
