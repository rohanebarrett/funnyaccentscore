using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Funny.Accents.Core.Email.Interfaces;

namespace Funny.Accents.Core.Email.Services
{
    public class MailService : IMailService
    {
        public void SendMail(IMailingInformation mailingInformation)
        {
            var mailMessage = ConstructMailMessage(mailingInformation);

            /*Create a SMTP client to send the email*/
            var mySmtpClient = new SmtpClient(mailingInformation.SmtpAddress)
            {
                Port = mailingInformation.SmtpPort
            };
            mySmtpClient.Send(mailMessage);
        }/*End of SendMail method*/

        public async Task SendMailAsync(IMailingInformation mailingInformation)
        {
            var mailMessage = ConstructMailMessage(mailingInformation);

            /*Create a SMTP client to send the email*/
            var mySmtpClient = new SmtpClient(mailingInformation.SmtpAddress)
            {
                Port = mailingInformation.SmtpPort
            };
            await mySmtpClient.SendMailAsync(mailMessage);
        }/*End of asynchronous SendMail method*/


        private static MailMessage ConstructMailMessage(IMailingInformation mailingInformation)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailingInformation.FromAddress),
                Subject = mailingInformation.EmailSubject,
                Body = mailingInformation.MessageBody,
                IsBodyHtml = mailingInformation.IsBodyHtml
            };

            mailingInformation.ToRecipientsList.ForEach(p =>
            {
                mailMessage.To.Add(p);
            });

            mailingInformation.CcRecipientsList.ForEach(p =>
            {
                mailMessage.CC.Add(p);
            });

            mailingInformation.BccRecipientsList.ForEach(p =>
            {
                mailMessage.Bcc.Add(p);
            });

            if (mailingInformation.AttachmentList == null) return mailMessage;
            foreach (var file in mailingInformation.AttachmentList)
            {
                CreateAttachment(mailMessage, file, mailingInformation.CustomAttachmentName);
            }
            return mailMessage;
        }

        private static void CreateAttachment(MailMessage mailMessageParam, string fileParam
            , Func<FileInfo, string> customAttachmentName)
        {
            var fileInfo = new FileInfo(fileParam);
            if (fileInfo.Exists == false)
            {
                throw new FileNotFoundException($"File attachment: {fileParam} does not exist.");
            }

            /*Create  the file attachment for this e-mail message*/
            var data = new Attachment(fileParam, MediaTypeNames.Application.Octet);

            /*Add time stamp information for the file*/
            var disposition = data.ContentDisposition;
            disposition.CreationDate = fileInfo.CreationTime;
            disposition.ModificationDate = fileInfo.LastWriteTime;
            disposition.ReadDate = fileInfo.LastAccessTime;
            disposition.FileName = customAttachmentName.Invoke(fileInfo);

            /*Add the file attachment to this e-mail message*/
            mailMessageParam.Attachments.Add(data);
        }/*End of createAttachment method*/
    }/*End of EmailHelper class*/
}/**/
