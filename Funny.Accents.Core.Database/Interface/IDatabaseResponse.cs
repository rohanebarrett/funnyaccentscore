using System.Collections.Generic;

namespace Funny.Accents.Core.Database.Interface
{
    public interface IDatabaseResponse<T>
        where T : class
    {
        IEnumerable<T> DbData { get; set; }
        IDictionary<string, object> DbParameters { get; set; }
    }/*End of IDatabaseResponse*/
}/*End of Funny.Accents.Core.Database.interface namespace*/
