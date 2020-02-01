using System;
using System.IO;
using System.Linq;
using Funny.Accents.Core.FileSearch.Enum;
using Funny.Accents.Core.FileSearch.Interfaces;

namespace Funny.Accents.Core.FileSearch.Abstractions
{
    public abstract class SearchPredicateBase : ISearchPredicate
    {
        private readonly ISearchCriteria _searchCriteria;

        protected SearchPredicateBase(ISearchCriteria searchCriteria)
        {
            _searchCriteria = searchCriteria;
        }

        public int GetSearchSizeLimit()
        {
            return _searchCriteria.GetSearchSizeLimit();
        }

        public bool GetSearchSubDirectory()
        {
            return _searchCriteria.GetSearchSubDirectory();
        }

        public Func<string, bool> GetExtensionFilterPredicate()
        {
            var extensionFilter = _searchCriteria.GetExtensionFilter();

            return filePath =>
            {
                try
                {
                    if (extensionFilter == null || extensionFilter.Length <= 0) { return true; }
                    //if (extensionFilter.Any(p => p.Equals("*"))) { return true; }
                    if (extensionFilter.Contains("*")) { return true; }

                    var fileInfo = new FileInfo(filePath);
                    return extensionFilter.Contains(fileInfo.Extension);
                }
                catch (Exception ex)
                {
                    return false;
                }
            };
        }

        public Func<string, bool> GetFileNameFilterPredicate()
        {
            var fileFilter = _searchCriteria.GetFileNameFilter();

            return filePath =>
            {
                try
                {
                    if (fileFilter == null || fileFilter.Length <= 0) { return true; }
                    //if (fileFilter.Any(p => p.Equals("*"))) { return true; }
                    if (fileFilter.Contains("*")) { return true; }

                    var fileInfo = new FileInfo(filePath);
                    return fileFilter.Any(filter => fileInfo.Name.Contains(filter));
                    //return fileFilter.Contains(fileInfo.Name);
                }
                catch (Exception ex)
                {
                    return false;
                }
            };
        }

        public Func<string, bool> GetRetentionThresholdPredicate()
        {
            var timePeriod = _searchCriteria.GetTimePeriod();
            var retentionThreshold = _searchCriteria.GetRetentionThreshold();
            var resetRetentionTimePortion = _searchCriteria.GetResetRetentionTimePortion();

            return filePath =>
            {
                try
                {
                    if (retentionThreshold < 0) { return false; }
                    if (retentionThreshold == 0) { return true; }
                    if (timePeriod == TimePeriod.None) { return true; }

                    var fileInfo = new FileInfo(filePath);
                    var fileTimeRetention = GetRetentionDateOffset(timePeriod,
                        retentionThreshold, resetRetentionTimePortion);
                    var x = fileInfo.LastAccessTime < fileTimeRetention;
                    return fileInfo.LastAccessTime < fileTimeRetention;
                }
                catch (Exception ex)
                {
                    return false;
                }
            };
        }

        private DateTime GetRetentionDateOffset(TimePeriod timePeriod,
            int retentionThreshold, bool resetRetentionTimePortion)
        {
            DateTime dateTimeOffset;

            switch (timePeriod)
            {
                case TimePeriod.Minutes:
                    var minuteOffset = DateTime.Now.AddMinutes(-retentionThreshold);
                    dateTimeOffset = resetRetentionTimePortion
                        ? new DateTime(minuteOffset.Year, minuteOffset.Month, minuteOffset.Day, minuteOffset.Hour, minuteOffset.Minute, 0)
                        : minuteOffset;
                    break;
                case TimePeriod.Hours:
                    var hourOffset = DateTime.Now.AddHours(-retentionThreshold);
                    dateTimeOffset = resetRetentionTimePortion
                        ? new DateTime(hourOffset.Year, hourOffset.Month, hourOffset.Day, hourOffset.Hour, 0, 0)
                        : hourOffset;
                    break;
                case TimePeriod.Days:
                    var dayOffset = DateTime.Now.AddDays(-retentionThreshold);
                    dateTimeOffset = resetRetentionTimePortion
                        ? new DateTime(dayOffset.Year, dayOffset.Month, dayOffset.Day, 0, 0, 0)
                        : dayOffset;
                    break;
                case TimePeriod.Months:
                    var monthOffset = DateTime.Now.AddMonths(-retentionThreshold);
                    dateTimeOffset = resetRetentionTimePortion
                        ? new DateTime(monthOffset.Year, monthOffset.Month, monthOffset.Day, 0, 0, 0)
                        : monthOffset;
                    break;
                case TimePeriod.Years:
                    var yearOffset = DateTime.Now.AddYears(-retentionThreshold);
                    dateTimeOffset = resetRetentionTimePortion
                        ? new DateTime(yearOffset.Year, yearOffset.Month, yearOffset.Day, 0, 0, 0)
                        : yearOffset;
                    break;
                case TimePeriod.None:
                    dateTimeOffset = new DateTime();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timePeriod), timePeriod, null);
            }

            return dateTimeOffset;
        }
    }
}
