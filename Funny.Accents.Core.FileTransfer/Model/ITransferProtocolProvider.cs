using Funny.Accents.Core.FileTransfer.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Funny.Accents.Core.FileTransfer.Model
{
    public interface ITransferProtocolProvider
    {
        string HostName { get; set; }
        int? Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        TransferProtocol TransferProtocol { get; set; }
    }/*End of ITransferProtocolProvider interface*/

    public interface ITransferProtocolRequester : ITransferProtocolProvider
    {
        TransferProtocolProcess TransferProcess { get; set; }
        TransferRequestDetails TransferRequestDetails { get; set; }
    }/*End of ITransferProtocolRequester interface*/

    [JsonObject(MemberSerialization.OptIn)]
    public class TransferProtocalRequest : ITransferProtocolRequester
    {
        [JsonProperty("hostName", Required = Required.Always)]
        public string HostName { get; set; }

        [JsonProperty("port", Required = Required.Always)]
        public int? Port { get; set; }

        [JsonProperty("userName", Required = Required.Always)]
        public string UserName { get; set; }

        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }

        [JsonProperty("transferProtocol", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransferProtocol TransferProtocol { get; set; }

        [JsonProperty("transferProcess", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransferProtocolProcess TransferProcess { get; set; }

        [JsonProperty("transferRequestDetails", Required = Required.Always)]
        public TransferRequestDetails TransferRequestDetails { get; set; }
    }/*End of TransferProtocalRequest class*/
}/*End of CmkUtilities.FileTransfer.Model namespace*/
