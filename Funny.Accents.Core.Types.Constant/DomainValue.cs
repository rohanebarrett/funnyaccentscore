namespace Funny.Accents.Core.Types.Constant
{
    public sealed class DomainValue
    {
        private readonly string _name;
        private readonly int _value;

        private DomainValue(int value, string name)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return _name;
        }

        public static readonly DomainValue NoneInventoryList
            = new DomainValue(1, "None");

        public static readonly DomainValue AllInventoryList
            = new DomainValue(2, "All");

        public static readonly DomainValue BootleggerInventoryList
            = new DomainValue(3, "bootInventory");

        public static readonly DomainValue CleoInventoryList
            = new DomainValue(4, "cleoInventory");

        public static readonly DomainValue RickisInventoryList
            = new DomainValue(5, "rickInventory");
    }/*End of DomainValues class*/
}/*End of CmkTypesCore.Constants namespace*/
