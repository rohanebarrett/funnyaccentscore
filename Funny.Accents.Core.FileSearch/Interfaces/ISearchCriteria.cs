using Funny.Accents.Core.FileSearch.Enum;

namespace Funny.Accents.Core.FileSearch.Interfaces
{
    public interface ISearchCriteria
    {
        TimePeriod GetTimePeriod();
        int GetRetentionThreshold();
        bool GetResetRetentionTimePortion();
        bool GetSearchSubDirectory();
        string[] GetExtensionFilter();
        string[] GetFileNameFilter();
        int GetSearchSizeLimit();
    }
}
