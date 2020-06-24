using Funny.Accents.Core.Core.Parsing.StringUtilities;
using System;
using Xunit;

namespace XUnitTestCoreParsing
{
    public class TestPathCombine
    {
        [Fact]
        public void PathCombineLastSlash()
        {
            /*Test what happens if the paths have a slash at the end*/
            const string expectedVal = @"\path\to\file.txt\";
            const char separator = '\\';
            var newPath = StringManipulation.PathCombine(
                @"\path", separator, paths: new[] { "to", "file.txt",@"\" });
            Assert.True(newPath.Equals(expectedVal),
                "Correct path construction");
        }/*End of PathCombineLastSlash namespace*/
    }/*End of TestPathCombine class*/
}/*End of XUnitTestCoreParsing namespace*/
