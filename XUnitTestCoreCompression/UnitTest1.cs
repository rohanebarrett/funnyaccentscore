using System.Collections.Generic;
using Funny.Accents.Core.Compression.Interfaces;
using Funny.Accents.Core.Compression.Services;
using Xunit;
using XUnitTestCoreCompression.Model;

namespace XUnitTestCoreCompression
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var arc = new ArchiveDefinition
            {
                FileLocation =
                    @"\\cgivmapp_test\D$\App_Working_Folder\SalesAudit\EcomSales\XPOLLD09800001_2018-08-20_04-57-58",
                ZipLocation = @"\\cgivmapp_test\D$\App_Working_Folder\SalesAudit\EcomSales\Archive",
                ZipFileName = "newZipFile.zip"
            };

            var result = new FileArchiver().ArchiveFile(new List<IArchiveDefinition>
            {
                arc
            });

            Assert.Contains(result, val => val.ArchivalStatus);
        }

        [Fact]
        public void ArchiveDirectory()
        {
            const string dirArch = @"\\cgivmapp_test\D$\App_Working_Folder\SalesAudit\EcomSales";
            const string zipPath = @"\\cgivmapp_test\D$\App_Working_Folder\SalesAudit\EcomSales\Archive\archived.zip";

            var result = new FileArchiver().ArchiveDirectory(dirArch,
                zipPath);

            Assert.Contains(result, val => val.ArchivalStatus);
        }
    }
}
