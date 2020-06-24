using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Funny.Accents.Core.Types.Enumeration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Divisions
    {
        [EnumMember(Value = "None")]
        None = 0,
        [EnumMember(Value = "Bootlegger")]
        Bootlegger = 1,
        [EnumMember(Value = "Rickis")]
        Rickis = 1001,
        [EnumMember(Value = "Cleo")]
        Cleo = 1051,
        [EnumMember(Value = "All")]
        All = 9999
    }/*End of Divisions enum*/

    public enum ProcessMode
    {
        None = 0,
        Default = 1,
        Information = 2,
        Data = 3
    }/*End of ProcessMode enum*/

    public enum BooleanSelection
    {
        All = 0,
        True = 1,
        False = 2
    }/*End of BooleanSelection enum*/

    public enum ShippingCouriers
    {
        None = 0,
        CanadaPost = 1,
        Purolator = 2
    }/*End of ShippingCouriers enum*/

    public enum ManifestBrevity
    {
        None = 0,
        Detailed = 1,
        Artifact = 2
    }/*End of ManifestBrevity enum*/

    public enum SerializationFormat
    {
        None = 1,
        Xml = 2,
        JsonJavaScriptSerializer = 3,
        JsonDataContract = 4,
        JsonNewton = 5
    }/*End of SerializationFormat enum*/

    public enum Environment
    {
        None = 0,
        Development = 1,
        Staging = 2,
        Production = 3
    }/*End of Environment enum*/

    public enum ResourceStateResult
    {
        None = 0,
        Created = 1,
        Updated = 2,
        Deleted = 3
    }/*End of ResourceStateResult enum*/

    public enum DcWarehouse
    {
        None = 0,
        Laval = 1,
        Granger = 2
    }/*End of DcWarehouse enum*/

    public enum InventoryAdjustmentType
    {
        None = 0,
        Increment = 1,
        Decrement = 2,
        Replace = 3,
        All = 9999
    }/*End of InventoryAdjustmentType enum*/

    public enum ExecutionWindow
    {
        None = 0,
        Current = 1,
        Previous = 2
    }/*End of DcWarehouse enum*/
}/*End of CmkTypes.Enums namespace*/
