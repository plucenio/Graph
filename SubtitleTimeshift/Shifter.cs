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
            using (StreamWriter writer = new StreamWriter(output, encoding, bufferSize, leaveOpen))
            {
                using (StreamReader reader = new StreamReader(input, encoding, true, bufferSize, leaveOpen))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match m = g.Match(line);
                        if (m.Success)
                        {
                            string start = m.Groups[1].Value;
                            string end = m.Groups[2].Value;

                            var newStart = TimeSpan.Parse(start.Replace(",", ".")).Add(timeSpan).ToString().Replace(".", ",");
                            var newEnd = TimeSpan.Parse(end.Replace(",", ".")).Add(timeSpan).ToString().Replace(".", ",");

                            line = line.Replace(start, newStart);
                            line = line.Replace(end, newEnd);
                        }                        

                        //byte[] bytes = encoding.GetBytes(line);

                        //output.Write(bytes, 0, 1);

                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}
