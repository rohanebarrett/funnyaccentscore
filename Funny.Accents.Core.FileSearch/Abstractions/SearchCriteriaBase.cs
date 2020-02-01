using Funny.Accents.Core.FileSearch.Enum;
using Funny.Accents.Core.FileSearch.Interfaces;

namespace Funny.Accents.Core.FileSearch.Abstractions
{
    public abstract class SearchCriteriaBase : ISearchCriteria
    {
        private readonly TimePeriod _timePeriod;
        private readonly int _retentionThreshold;
        private readonly bool _resetRetentionTimePortion;
        private readonly bool _searchSubDirectory;
        private readonly string[] _extensionFilter;
        private readonly string[] _fileNameFilter;
        private readonly int _searchSizeLimit;

        protected SearchCriteriaBase(TimePeriod timePeriod,
            int retentionThreshold, bool resetRetentionTimePortion,
            bool searchSubDirectory, string[] extensionFilter,
            string[] fileNameFilter, int searchSizeLimit)
        {
            _timePeriod = timePeriod;
            _retentionThreshold = retentionThreshold;
            _resetRetentionTimePortion = resetRetentionTimePortion;
            _searchSubDirectory = searchSubDirectory;
            _extensionFilter = extensionFilter;
            _fileNameFilter = fileNameFilter;
            _searchSizeLimit = searchSizeLimit;
        }

        public TimePeriod GetTimePeriod()
        {
            return _timePeriod;
        }

        public int GetRetentionThreshold()
        {
            return _retentionThreshold;
        }

        public bool GetResetRetentionTimePortion()
        {
            return _resetRetentionTimePortion;
        }

        public bool GetSearchSubDirectory()
        {
            return _searchSubDirectory;
        }

        public string[] GetExtensionFilter()
        {
            return _extensionFilter;
        }

        public string[] GetFileNameFilter()
        {
            return _fileNameFilter;
        }

        public int GetSearchSizeLimit()
        {
            return _searchSizeLimit;
        }
    }
}
