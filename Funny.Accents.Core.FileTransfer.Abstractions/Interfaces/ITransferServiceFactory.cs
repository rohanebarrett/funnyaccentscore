using Funny.Accents.Core.FileTransfer.Model;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Interfaces
{
    internal interface ITransferServiceFactory
    {
        ITransferService GetService();
    }/*End of ITransferServiceFactory interface*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Interfaces namespace*/
