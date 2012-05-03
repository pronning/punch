using System;
using System.IO;
using System.IO.Packaging;

namespace Punch.Utils
{
    public static class FileHelper
    {
        public static void Archive( string sourcePath)
        {
            Archive(sourcePath, null);
        }


        public static void Archive( string sourcePath, string targetPath )
        {
            if( sourcePath.IsNullOrEmpty())
            {
                throw new ArgumentNullException("sourcePath");
            }

            if( !File.Exists(sourcePath))
            {
                throw new Exception("not found".Prepare(sourcePath));
            }

            string fileName = Path.GetFileName(sourcePath) ?? "";
            
            if( string.IsNullOrEmpty(targetPath))
            {
                var sourceExt = Path.GetExtension(sourcePath) ?? ".csv";
                targetPath = sourcePath.Replace(sourceExt, ".zip");
            }

            var zippedFile = Zip(fileName, File.ReadAllBytes(sourcePath));
            File.WriteAllBytes(targetPath, zippedFile);

        }

        public static byte[] Zip( string fileName, byte[] fileData )
        {
            var packageStream = new MemoryStream();
            using (Package package = Package.Open(packageStream, FileMode.Create))
            {
                string destFilename = ".\\" + fileName;
                Uri uri = PackUriHelper.CreatePartUri(new Uri(destFilename, UriKind.Relative));
                PackagePart part = package.CreatePart(uri, "", CompressionOption.Maximum);
                using (var appendFile = new MemoryStream(fileData))
                {
                    using (Stream destinationPartStream = part.GetStream())
                    {
                        CopyStream(appendFile, destinationPartStream);
                    }
                }
            }
            return packageStream.ToByteArray();
        }

        //private const long BufferSize = 4096;
        private static void CopyStream(Stream inputStream, Stream outputStream)
        {
            //long bufferSize = inputStream.Length < BufferSize ? inputStream.Length : BufferSize;
            byte[] buffer = new byte[inputStream.Length];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}
