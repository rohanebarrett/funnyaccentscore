using System.Collections.Generic;
using Funny.Accents.Core.Types.Results;
using Funny.Accents.Core.FileTransfer.Enum;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Interfaces
{
    internal interface ITransferServiceDirector
    {
        //T PerformAction<T>();
        IProcessResult DownloadFile(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails);
        IProcessResult UploadFile(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails);
        IProcessResult RemoveFile(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails);
        IProcessResult FileExists(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails);
        IProcessResult<IEnumerable<string>> ListFiles(TransferProtocol protocol,
            ITransmissionDetails transmissionDetails);
    }/*End of ITransferServiceDirector interface*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Interfaces namespace*/
