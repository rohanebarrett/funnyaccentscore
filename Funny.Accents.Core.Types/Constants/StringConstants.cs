namespace Funny.Accents.Core.Types.Constants
{
    public sealed class StringConstants
    {
        private readonly string _name;

        private StringConstants(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        private static readonly StringConstants Blank = new StringConstants("");
        private static readonly StringConstants Space = new StringConstants(" ");
        private static readonly StringConstants Colon = new StringConstants(":");
        private static readonly StringConstants SemiColon = new StringConstants(";");

        public static string GetBlank()
        {
            return Blank.ToString();
        }

        public static string GetSpace()
        {
            return Space.ToString();
        }

        public static string GetColon()
        {
            return Colon.ToString();
        }

        public static string GetSemiColon()
        {
            return SemiColon.ToString();
        }
    }/*End of StringConstants sealed class*/
}/*End of EcommerceServices.Constants namespace*/
