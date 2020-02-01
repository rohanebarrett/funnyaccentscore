using Funny.Accents.Core.FileTransfer.Abstractions.Interfaces;
using Funny.Accents.Core.FileTransfer.Enum;

namespace Funny.Accents.Core.FileTransfer.Abstractions.Models
{
    public class ConnectionDetails : IConnectionDetails
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool DisconnectOnComplete { get; set; }
        public TransferProtocol TransferProtocol { get; set; }
    }/*End of ConnectionDetails class*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Models namespace*/

