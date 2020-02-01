using System;
using System.Text;

namespace Funny.Accents.Core.Types.Interfaces
{
    public interface IHttpContentDefinition<TContentPayload>
    {
        string ContentType { get; set; }
        Encoding ContentEncoding { get; set; }
        string SerializedContentPayLoad { get;}
        TContentPayload ContentPayload { get; set; }
        Func<TContentPayload, string> SerializationAction { get; set; }
    }/*End of IHttpContentDefinition class*/
}/*End of CmkTypesCore.Interfaces namespace*/
