using Funny.Accents.Core.Database.Interface;
using System.Collections.Generic;

namespace Funny.Accents.Core.Database.Model
{
    public class DatabaseResponse<T> : IDatabaseResponse<T>
        where T : class
    {
        public IEnumerable<T> DbData { get; set; }
        public IDictionary<string, object> DbParameters { get; set; }
    }
}
