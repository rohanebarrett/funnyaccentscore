namespace Funny.Accents.Core.Types.Constants
{
    public sealed class DomainValues
    {
        private readonly string _name;
        private readonly int _value;

        private DomainValues(int value, string name)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return _name;
        }

        public static readonly DomainValues NoneInventoryList
            = new DomainValues(1, "None");

        public static readonly DomainValues AllInventoryList
            = new DomainValues(2, "All");

        public static readonly DomainValues BootleggerInventoryList
            = new DomainValues(3, "bootInventory");

        public static readonly DomainValues CleoInventoryList
            = new DomainValues(4, "cleoInventory");

        public static readonly DomainValues RickisInventoryList
            = new DomainValues(5, "rickInventory");
    }/*End of DomainValues class*/
}/*End of CmkTypesCore.Constants namespace*/
