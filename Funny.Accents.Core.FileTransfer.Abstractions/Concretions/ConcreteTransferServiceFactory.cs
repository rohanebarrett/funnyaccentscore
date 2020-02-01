using Funny.Accents.Core.FileTransfer.Abstractions.Abstract;
using Funny.Accents.Core.FileTransfer.Abstractions.Interfaces;
using Funny.Accents.Core.FileTransfer.Enum;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Concretions
{
    internal class ConcreteTransferServiceFactory : TransferServiceFactory
    {
        public ConcreteTransferServiceFactory(
            TransferProtocol transferProtocol,
            IConnectionDetails connectionDetails)
            : base(transferProtocol, connectionDetails)
        { }
    }/*End of TransferServiceFactory abstract class*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Abstract namespace*/
