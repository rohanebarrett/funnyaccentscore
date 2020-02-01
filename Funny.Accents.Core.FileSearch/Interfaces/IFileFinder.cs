using System.Collections.Generic;

namespace Funny.Accents.Core.FileSearch.Interfaces
{
    internal interface IFileFinder
    {
        IEnumerable<string> SearchDirectory();
        //IEnumerable<string> SearchDirectory(
        //    string searchDirectories, ISearchPredicate searchPredicate);
    }
}
