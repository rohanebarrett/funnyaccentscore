using Funny.Accents.Core.FileSearch.Enum;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using XUnitTestCoreFileSearch.Models;

namespace XUnitTestCoreFileSearch
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public static IEnumerable<object[]> ExtensionFilterTestParameters =>
            new List<object[]>
            {
                new object[] { new[]{".xlsx"}, 1},
                new object[] { new[]{".json",".xlsx"}, 4},
            };

        public static IEnumerable<object[]> NameFilterTestParameters =>
            new List<object[]>
            {
                new object[] { new[]{"Excel"}, 3},
                new object[] { new[]{"Test","Cars","Years"}, 2},
            };

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void File_Is_X_Days_Old()
        {
            const int daysOld = 5;

            var sCriteria = new SearchCriteria(TimePeriod.Days, daysOld,
                true, false, null,
                null,100);

            var sPredicate = new SearchPredicate(sCriteria);

            var sFinder = new FileFinder(@"J:\temp\Excel_Testing", sPredicate);
            foreach (var filePath in sFinder.SearchDirectory())
            {
                _testOutputHelper.WriteLine(filePath);
            }

            Assert.True(true);
        }

        [Theory]
        [MemberData(nameof(ExtensionFilterTestParameters))]
        public void File_Has_Extension(string[] extFilter, int expectedFiles)
        {
            //string[] extFilter = {".xlsx"};
            var fileFound = 0;

            var sCriteria = new SearchCriteria(TimePeriod.Days, 0,
                true, false, extFilter,
                null, 100);

            var sPredicate = new SearchPredicate(sCriteria);

            var sFinder = new FileFinder(@"J:\temp\Excel_Testing", sPredicate);
            foreach (var filePath in sFinder.SearchDirectory())
            {
                _testOutputHelper.WriteLine(filePath);
                fileFound++;
            }

            Assert.True(fileFound == expectedFiles);
        }

        [Theory]
        [MemberData(nameof(NameFilterTestParameters))]
        public void File_Has_String_In_Name(string[] nameFilter, int expectedFiles)
        {
            //string[] extFilter = {".xlsx"};
            var fileFound = 0;

            var sCriteria = new SearchCriteria(TimePeriod.Days, 0,
                true, false, null,
                nameFilter, 100);

            var sPredicate = new SearchPredicate(sCriteria);

            var sFinder = new FileFinder(@"J:\temp\Excel_Testing", sPredicate);
            foreach (var filePath in sFinder.SearchDirectory())
            {
                _testOutputHelper.WriteLine(filePath);
                fileFound++;
            }

            Assert.True(fileFound == expectedFiles);
        }
    }
}
