using Funny.Accents.Core.FileSearch.Abstractions;
using Funny.Accents.Core.FileSearch.Interfaces;

namespace XUnitTestCoreFileSearch.Models
{
    class FileFinder : FileFinderBase
    {
        public FileFinder(string searchDirectories, ISearchPredicate searchPredicate) 
            : base(searchDirectories, searchPredicate)
        {
        }
    }
}
