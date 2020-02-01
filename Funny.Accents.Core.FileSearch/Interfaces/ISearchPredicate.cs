using System;
using System.Collections.Generic;
using System.Text;
using Funny.Accents.Core.FileSearch.Enum;

namespace Funny.Accents.Core.FileSearch.Interfaces
{
    public interface ISearchPredicate
    {
        int GetSearchSizeLimit();
        bool GetSearchSubDirectory();
        Func<string, bool> GetExtensionFilterPredicate();
        Func<string, bool> GetFileNameFilterPredicate();
        Func<string, bool> GetRetentionThresholdPredicate();

        //Predicate<string> GetExtensionFilterPredicate(string[] extensionFilter);
        //Predicate<string> GetFileNameFilterPredicate(string[] fileFilter);
        //Predicate<string> GetRetentionThresholdPredicate(TimePeriod timePeriod,
        //    int retentionThreshold, bool resetRetentionTimePortion);
    }
}
