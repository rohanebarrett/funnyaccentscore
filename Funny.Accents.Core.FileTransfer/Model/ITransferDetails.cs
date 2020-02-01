using Newtonsoft.Json;

namespace Funny.Accents.Core.FileTransfer.Model
{
    public interface IFileIoDetails
    {
        string RemoteDirectory { get; set; }
        string RemoteFileName { get; set; }
        string FileFilter { get; set; }
    }/*End of IFileIoDetails interface*/

    public interface ITransferDetails
    {
        string RemoteDirectory { get; set; }
        string RemoteFileName { get; set; }
        string LocalDirectory { get; set; }
        string LocalFileName { get; set; }
        bool OverrideExisting { get; set; }
    }/*End of ITransferDetails interface*/

    [JsonObject(MemberSerialization.OptIn)]
    public class FileIoDetails : IFileIoDetails
    {
        private string _fileFilter = "*";

        [JsonProperty("remoteDirectory", Required = Required.Always)]
        public string RemoteDirectory { get; set; }

        [JsonProperty("remoteFileName", Required = Required.Default)]
        public string RemoteFileName { get; set; }

        [JsonProperty("fileFilter", Required = Required.Default)]
        public string FileFilter
        {
            get { return _fileFilter; }
            set { _fileFilter = string.IsNullOrWhiteSpace(value) ? "*" : value; }
        }
    }/*End of FileIoDetails class*/

    [JsonObject(MemberSerialization.OptIn)]
    public class TransferDetails : ITransferDetails
    {
        [JsonProperty("remoteDirectory", Required = Required.Always)]
        public string RemoteDirectory { get; set; }

        [JsonProperty("remoteFileName", Required = Required.Always)]
        public string RemoteFileName { get; set; }

        [JsonProperty("localDirectory", Required = Required.Always)]
        public string LocalDirectory { get; set; }

        [JsonProperty("localFileName", Required = Required.Always)]
        public string LocalFileName { get; set; }

        [JsonProperty("overrideExisting", Required = Required.Always)]
        public bool OverrideExisting { get; set; }
    }/*End of TransferDetails class*/

    [JsonObject(MemberSerialization.OptIn)]
    public class TransferRequestDetails
    {
        [JsonProperty("fileIoDetails", Required = Required.Default)]
        public FileIoDetails FileIoDetails { get; set; }

        [JsonProperty("transferDetails", Required = Required.Default)]
        public TransferDetails TransferDetails { get; set; }
    }/*End of TransferRequestDetails class*/
}/*End of CmkUtilities.FileTransfer.Model namespace*/
