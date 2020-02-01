using System.IO;
using Microsoft.Extensions.Configuration;

namespace Funny.Accents.Core.Configuration.Helpers
{
    public class ConfigHelper
    {
        public static IConfiguration GetAppJsonFileInfo(string jsonFileNameParam)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jsonFileNameParam).Build();

            return builder;
        }/*End of GetAppJsonFileInfo method*/

        public static string GetAppJsonSettingsValue(string appSettingsValue, string configFileName)
        {
            string apiString = null;
            try
            {
                var appJson = GetAppJsonFileInfo(configFileName);
                apiString = appJson?[appSettingsValue];
                return apiString;
            }
            catch
            {
                return apiString;
            }
        }/*End of GetAppJsonSettingsValue method*/

        public static string GetAppJsonSettingsValue(string appSettingsValue,
            IConfiguration appJson)
        {
            string apiString = null;
            try
            {
                apiString = appJson?[appSettingsValue];
                return apiString;
            }
            catch
            {
                return apiString;
            }
        }/*End of GetAppJsonSettingsValue method*/

        public static T BindConfiguration<T>(IConfiguration config,
            string section)
            where T : new()
        {
            var configModel = new T();
            config.GetSection(section).Bind(configModel);
            return configModel;
        }
    }/*End of */
}/*End of CmkConfigurationUtilities namespace*/
