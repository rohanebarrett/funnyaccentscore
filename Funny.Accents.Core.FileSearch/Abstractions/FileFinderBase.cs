using Funny.Accents.Core.FileSearch.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Funny.Accents.Core.FileSearch.Abstractions
{
    public abstract class FileFinderBase : IFileFinder
    {
        private readonly string _searchDirectories;
        private readonly ISearchPredicate _searchPredicate;

        protected FileFinderBase(string searchDirectories, ISearchPredicate searchPredicate)
        {
            _searchDirectories = searchDirectories;
            _searchPredicate = searchPredicate;
        }

        public IEnumerable<string> SearchDirectory()
        {
            var enumerable = Directory.EnumerateFiles(_searchDirectories,
                    "*.*", _searchPredicate.GetSearchSubDirectory()
                        ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Where(_searchPredicate.GetExtensionFilterPredicate())
                .Where(_searchPredicate.GetFileNameFilterPredicate())
                .Where(_searchPredicate.GetRetentionThresholdPredicate());

            return enumerable;
        }
    }
}
