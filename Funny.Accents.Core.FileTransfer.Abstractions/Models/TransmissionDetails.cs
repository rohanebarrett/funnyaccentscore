using Funny.Accents.Core.FileTransfer.Abstractions.Interfaces;
using Funny.Accents.Core.FileTransfer.Enum;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Models
{
    public class TransmissionDetails : ITransmissionDetails
    {
        public TransferProtocol TransferProtocol { get; set; }
        //public TransferProtocolProcess TransferProtocolProcess { get; set; }
        public IFileIoDetails FileIoDetails { get; set; }
        public ITransferDetails TransferDetails { get; set; }
        public IConnectionDetails ConnectionDetails { get; set; }
    }/*End of TransmissionDetails namespace*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Models namespace*/
