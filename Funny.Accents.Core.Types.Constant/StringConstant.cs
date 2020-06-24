namespace Funny.Accents.Core.Types.Constant
{
    public sealed class StringConstant
    {
        private readonly string _name;

        private StringConstant(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        private static readonly StringConstant Blank = new StringConstant("");
        private static readonly StringConstant Space = new StringConstant(" ");
        private static readonly StringConstant Colon = new StringConstant(":");
        private static readonly StringConstant SemiColon = new StringConstant(";");

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
