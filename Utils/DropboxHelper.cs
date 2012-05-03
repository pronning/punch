using System;
using System.IO;
using System.Linq;

namespace Punch.Utils
{
    public static class DropboxHelper
    {
        public static string GetOrCreateDropboxPath(params string[] subfolders)
        {
            var newPath = GetDropboxPath(subfolders);
            if( !Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            return newPath;
        }


        public static string GetDropboxPath( params string[] subfolders)
        {
            var basePath = GetDropboxPath();
            return subfolders.Aggregate(basePath, Path.Combine);
        }

        public static string GetDropboxPath()
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if( appDataFolder == null)
            {
                throw new Exception("Får ikke tak i AppData");
            }

            var configFilePath = Path.Combine(appDataFolder, "Dropbox", "host.db");
            if (!File.Exists(configFilePath))
            {
                throw new Exception("finner ikke host.db på " + configFilePath);
            }

            var configFile = File.ReadAllLines(configFilePath);
            var lastLine = configFile.LastOrDefault();
            if (lastLine == null)
            {
                throw new Exception("klarte ikke å lese siste linje i host.db");
            }

            var configBytes = Convert.FromBase64String(lastLine);
            var utfString = System.Text.Encoding.UTF8.GetString(configBytes);
            if( string.IsNullOrEmpty(utfString))
            {
                throw new Exception("klarte ikke å finne Dropbox-mappe");
            }
            return utfString;
        }

        
    }
}