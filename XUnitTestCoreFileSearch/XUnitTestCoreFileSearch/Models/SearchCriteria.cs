using Funny.Accents.Core.FileSearch.Abstractions;
using Funny.Accents.Core.FileSearch.Enum;

namespace XUnitTestCoreFileSearch.Models
{
    internal class SearchCriteria : SearchCriteriaBase
    {
        public SearchCriteria(TimePeriod timePeriod,
            int retentionThreshold, bool resetRetentionTimePortion,
            bool searchSubDirectory,
            string[] extensionFilter,
            string[] fileNameFilter,
            int searchSizeLimit) 
            : base(timePeriod, retentionThreshold, resetRetentionTimePortion,
                searchSubDirectory, extensionFilter, fileNameFilter, searchSizeLimit)
        {
        }
    }
}
