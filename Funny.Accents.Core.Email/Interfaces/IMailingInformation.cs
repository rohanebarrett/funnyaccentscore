using System;
using System.Collections.Generic;
using System.IO;

namespace Funny.Accents.Core.Email.Interfaces
{
    public interface IMailingInformation
    {
        string FromAddress { get; set; }
        List<string> ToRecipientsList { get; set; }
        List<string> CcRecipientsList { get; set; }
        List<string> BccRecipientsList { get; set; }
        string EmailSubject { get; set; }
        string MessageBody { get; set; }
        string SmtpAddress { get; set; }
        int SmtpPort { get; set; }
        List<string> AttachmentList { get; set; }
        bool IsBodyHtml { get; set; }
        Func<FileInfo, string> CustomAttachmentName { get; set; }
    }
}
