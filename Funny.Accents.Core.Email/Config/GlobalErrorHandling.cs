namespace Funny.Accents.Core.Email.Config
{
    public class GlobalErrorHandling
    {
        public string ExceptionFromAddress { get; set; }
        public string ExceptionToAddress { get; set; }
        public string ExceptionCcAddress { get; set; }
        public string ExceptionBccAddress { get; set; }
        public string ExceptionAddressDelimiter { get; set; }
        public bool SendErrorEmail { get; set; }
        public string ErrorLog { get; set; }
    }/*End of GlobalErrorHandling class*/
}/*End of Funny.Accents.Core.Email.Config namespace*/
