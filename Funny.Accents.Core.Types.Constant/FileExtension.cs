namespace Funny.Accents.Core.Types.Constant
{
    public class FileExtension
    {
        private readonly string _name;

        private FileExtension(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        public static readonly FileExtension Csv = new FileExtension(".csv");
        public static readonly FileExtension Xml = new FileExtension(".xml");
        public static readonly FileExtension Json = new FileExtension(".json");
        public static readonly FileExtension Xls = new FileExtension(".xls");
        public static readonly FileExtension Xlsx = new FileExtension(".xlsx");

    }/*End of FileExtensions enumeration*/
}/*End of Cmk.Devsolutions.Core.Types.Constants namespace*/
