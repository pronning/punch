using System.IO;
using Punch.Models;

namespace Punch.Data
{
    public class SkandiabankenInputFile : InputFileBase
    {
        public SkandiabankenInputFile(Stream fileStream)
        {
            
            FileStream = fileStream;
            HasHeaderRow = true;
            DatePos = 0;
            DescPos = 4;
            AmountPos = 5;
            SourceName = "VISA";
        }
    }
}