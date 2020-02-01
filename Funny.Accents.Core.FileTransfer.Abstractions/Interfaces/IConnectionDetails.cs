namespace Funny.Accents.Core.FileTransfer.Abstractions.Interfaces
{
    public interface IConnectionDetails
    {
        string HostName { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool DisconnectOnComplete { get; set; }
    }/*End of IConnectionDetails interface*/
}/*End of Funny.Accents.Core.FileTransfer.Abstractions.Interfaces namespace*/
