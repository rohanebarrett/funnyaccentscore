using Funny.Accents.Core.FileSearch.Abstractions;
using Funny.Accents.Core.FileSearch.Interfaces;

namespace XUnitTestCoreFileSearch.Models
{
    internal class SearchPredicate : SearchPredicateBase
    {
        public SearchPredicate(ISearchCriteria searchCriteria) : base(searchCriteria)
        {
        }
    }
}
