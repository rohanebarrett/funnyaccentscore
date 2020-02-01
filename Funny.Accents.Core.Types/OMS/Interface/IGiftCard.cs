using Newtonsoft.Json;

namespace Funny.Accents.Core.Types.OMS.Interface
{
    public interface IGiftCard
    {
        string Number { get; set; }
        string Value { get; set; }
        string SkuCode { get; set; }
    }/*End of IGiftCard interface*/

    [JsonObject(MemberSerialization.OptIn)]
    public class GiftCard : IGiftCard
    {
        [JsonProperty("Number", Required = Required.Always)]
        public string Number { get; set; }

        [JsonProperty("Value", Required = Required.Always)]
        public string Value { get; set; }

        [JsonProperty("skuCode", Required = Required.Always)]
        public string SkuCode { get; set; }
    }/*End of GiftCard class*/
}/*End of CmkTypes.OMS.Orders namespace*/
