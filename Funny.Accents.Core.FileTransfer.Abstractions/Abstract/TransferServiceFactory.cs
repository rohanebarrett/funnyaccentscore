using Funny.Accents.Core.FileTransfer.Abstractions.Interfaces;
using Funny.Accents.Core.FileTransfer.Enum;
using Funny.Accents.Core.FileTransfer.Model;
using Funny.Accents.Core.FileTransfer.Workers;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Abstract
{
    internal abstract class TransferServiceFactory : ITransferServiceFactory
    {
        private readonly TransferProtocol _transferProtocol;
        private readonly IConnectionDetails _connectionDetails;

        protected TransferServiceFactory(
            TransferProtocol transferProtocol,
            IConnectionDetails connectionDetails)
        {
            _transferProtocol = transferProtocol;
            _connectionDetails = connectionDetails;
        }

        public ITransferService GetService()
        {
            if (_transferProtocol == TransferProtocol.Sftp)
            {
                return new SftpWorker(_connectionDetails.HostName,
                    _connectionDetails.Port,
                    _connectionDetails.UserName,
                    _connectionDetails.Password);
            }

            if (_transferProtocol == TransferProtocol.Ftp)
            {
                return new FtpWorker(_connectionDetails.HostName,
                    _connectionDetails.UserName,
                    _connectionDetails.Password);
            }

            return null;
        }
    }/*End of TransferServiceFactory abstract class*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Abstract namespace*/
