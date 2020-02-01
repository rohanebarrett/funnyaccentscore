using System;
using System.Collections.Generic;
using System.IO;
using Funny.Accents.Core.Email.Interfaces;

namespace Funny.Accents.Core.Email.Models
{
    public class MailingInformation : IMailingInformation
    {
        private const string CharactersToRemove = "\r\n";
        private const string StringBlank = "";
        private string _emailSubject;
        public string FromAddress { get; set; }
        public List<string> ToRecipientsList { get; set; } = new List<string>();
        public List<string> CcRecipientsList { get; set; } = new List<string>();
        public List<string> BccRecipientsList { get; set; } = new List<string>();
        public string EmailSubject
        {
            get => _emailSubject.Replace(CharactersToRemove, StringBlank).Trim();
            set => _emailSubject = value;
        }

        public string MessageBody { get; set; }
        public string SmtpAddress { get; set; }
        public int SmtpPort { get; set; }
        public List<string> AttachmentList { get; set; }
        public bool IsBodyHtml { get; set; }
        public Func<FileInfo, string> CustomAttachmentName { get; set; }
            = p => $"{Path.GetFileNameWithoutExtension(p.Name)}_{DateTime.Now:yyyyMMdd}.{p.Extension}";

    }/*End of class MailingInformation*/
}/*End of CmkEmailUtilitiesCore.Model namespace*/
