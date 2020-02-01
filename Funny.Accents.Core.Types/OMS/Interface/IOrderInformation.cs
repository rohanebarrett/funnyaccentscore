using Newtonsoft.Json;

namespace Funny.Accents.Core.Types.OMS.Interface
{
    public interface IOrderInformation
    {
        [JsonProperty("orderNumber", Required = Required.Always)]
        string OrderNumber { get; set; }

        [JsonProperty("trackinCode", Required = Required.Always)]
        string TrackinCode { get; set; }
        GiftCard[] Giftcards { get; set; }
        string LoyaltyCard { get; set; }
    }/*End of IOrderInformation interface*/

    [JsonObject(MemberSerialization.OptIn)]
    public class OrderInformation : IOrderInformation
    {
        [JsonProperty("orderNumber", Required = Required.Always)]
        public string OrderNumber { get; set; }

        [JsonProperty("trackinCode", Required = Required.Always)]
        public string TrackinCode { get; set; }

        [JsonProperty("giftCards", Required = Required.Default)]
        public GiftCard[] Giftcards { get; set; }

        [JsonProperty("loyaltyCard", Required = Required.Default)]
        public string LoyaltyCard { get; set; }

    }/*End of OrderInformation class*/

    public interface IOrderCompletionInformation
    {
        IOrderInformation OrderInformation { get; set; }
    }/*End of IOrderCompletionInformation interface*/
}/*End of CmkTypes.OMS.Orders namespace*/
