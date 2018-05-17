using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtitleTimeshift
{
    public class Shifter
    {
        async static public Task Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding, int bufferSize = 1024, bool leaveOpen = false)
        {
            Regex g = new Regex(@"(?<start>\S+)\s-->\s(?<end>\S+)");
            using (StreamReader r = new StreamReader(input))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    Match m = g.Match(line);
                    if (m.Success)
                    {
                        string v = m.Groups[1].Value;
                    }
                }
            }
        }
    }
}
