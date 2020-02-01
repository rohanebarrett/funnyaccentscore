using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Funny.Accents.Core.Core.IO.Streams
{
    public static class StreamUtil
    {
        public static async Task WriteStreamToFileAsync(Stream data, string fileName
            , FileMode fileMode = FileMode.Create)
        {
            using (data)
            using (var output = new FileStream($"{fileName}", fileMode))
            {
                data.Position = 0;
                await data.CopyToAsync(output);
            }
        }/*End of WriteStreamToFileAsync method*/

        public static void WriteStreamToFile(Stream data, string fileName, FileMode fileMode = FileMode.Create)
        {
            using (var output = new FileStream($"{fileName}", fileMode))
            {
                data.Position = 0;
                data.CopyTo(output);
            }
        }/*End of WriteStreamToFile method*/

        public static void WriteToFile(string fileToWrite, string dataToWrite, bool appendData)
        {
            using (var outputFile = new StreamWriter(fileToWrite, appendData))
            {
                outputFile.WriteLine(dataToWrite);
            }
        }

        public static async void WriteToFileAsync(string fileToWrite, string dataToWrite, bool appendData)
        {
            using (var outputFile = new StreamWriter(fileToWrite, appendData))
            {
                await outputFile.WriteLineAsync(dataToWrite);
            }
        }

        public static Stream ToStream(string str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream ToStream(IEnumerable<string> str)
        {
            if (str == null) { return null; }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            foreach (var line in str)
            {
                writer.WriteLine(line);
            }

            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream ToStream(Image image, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        public static async Task CopyFileAsync(string sourceFile, string destinationFile)
        {
            using (var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read,
                FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                using (var destinationStream = new FileStream(destinationFile, FileMode.CreateNew,
                    FileAccess.Write, FileShare.None, 4096,
                    FileOptions.Asynchronous | FileOptions.SequentialScan))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }
        }/*End of CopyFileAsync method*/

    }/*End of StreamUtil class*/
}/*End of CmkIoUtilities.Streams namespace*/
