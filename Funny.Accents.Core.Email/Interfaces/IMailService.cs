using System.Threading.Tasks;

namespace Funny.Accents.Core.Email.Interfaces
{
    public interface IMailService
    {
        void SendMail(IMailingInformation mailingInformation);
        Task SendMailAsync(IMailingInformation mailingInformation);
    }
}
