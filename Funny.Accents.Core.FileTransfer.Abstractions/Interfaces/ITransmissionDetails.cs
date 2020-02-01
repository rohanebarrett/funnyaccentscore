using Funny.Accents.Core.FileTransfer.Enum;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Interfaces
{
    public interface ITransmissionDetails
    {
        //TransferProtocol TransferProtocol { get; set; }
        //TransferProtocolProcess TransferProtocolProcess { get; set; }
        IFileIoDetails FileIoDetails { get; set; }
        ITransferDetails TransferDetails { get; set; }
        IConnectionDetails ConnectionDetails { get; set; }
    }/*End of ITransmissionDetails interface*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Interfaces namespace*/
