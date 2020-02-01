namespace Funny.Accents.Core.Types.Constants
{
    public class FileExtensions
    {
        private readonly string _name;

        private FileExtensions(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        public static readonly FileExtensions Csv = new FileExtensions(".csv");
        public static readonly FileExtensions Xml = new FileExtensions(".xml");
        public static readonly FileExtensions Json = new FileExtensions(".json");
        public static readonly FileExtensions Xls = new FileExtensions(".xls");
        public static readonly FileExtensions Xlsx = new FileExtensions(".xlsx");

    }/*End of FileExtensions enumeration*/
}/*End of Funny.Accents.Core.Types.Constants namespace*/
