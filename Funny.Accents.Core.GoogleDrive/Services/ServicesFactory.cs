using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;

namespace Funny.Accents.Core.Core.GoogleDrive.Services
{
    public class ServicesFactory
    {
        public static GoogleCredential ConstructCredentialsFromJson(string jsonFilePath, IEnumerable<string> driveScopes)
        {
            GoogleCredential credential;

            using (var stream = new FileStream(jsonFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(driveScopes);
            }

            return credential;
        }/*End of ConstructCredentialsFromJson method*/

        public static (MemoryStream fileStream, Google.Apis.Drive.v3.Data.File fileMetadata)
            ExportGoogleDriveFile(DriveService service, string fileId, string mimeContent, bool fileMetadata = false)
        {
            try
            {
                service.HttpClient.Timeout = TimeSpan.FromSeconds(60);
                var request = service.Files.Export(fileId, mimeContent);
                var stream = new MemoryStream();

                request.Download(stream);
                return (stream, fileMetadata ? service.Files.Get(fileId).Execute() : null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }/*End of ExportGoogleDriveFile method*/

        public static (MemoryStream fileStream, Google.Apis.Drive.v3.Data.File fileMetadata)
            DownloadGoogleDriveFile(DriveService service, string fileId,
            bool fileMetadata = false)
        {
            service.HttpClient.Timeout = TimeSpan.FromSeconds(60);

            var request = service.Files.Get(fileId);
            var stream = new MemoryStream();

            request.Download(stream);
            return (stream, fileMetadata ? service.Files.Get(fileId).Execute() : null);
        }/*End of DownloadGoogleDriveFile method*/
    }/*End of ServicesFactory class*/
}/*End of CmkGoogleDriveUtilities.Services namespace*/
