using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CsvToJson
{
    internal class MoonIllumination
    {
        private const string FileName = "moon_2017.txt";

        public static void GenerateMoonIlluminationJson()
        {
            Console.WriteLine($"Reading {FileName}");

            var illuminationLines = File.ReadAllLines(FileName, Encoding.UTF8).ToList();

            Console.WriteLine($"Read {illuminationLines.Count} lines from file. Converting to objects.");

            var moonIlluminations = illuminationLines.Select(l =>
            {
                var lineElements = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return new MoonIlluminationDto
                {
                    DateTime =
                        DateTime.ParseExact(lineElements[0], "yyyy MMM dd HH:mm:ss.f", CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal),
                    Percent = Convert.ToDouble(lineElements[1].Trim(), CultureInfo.InvariantCulture)
                };
            }).ToArray();

            Console.WriteLine($"{moonIlluminations.Length} objects created from {FileName}.");

            Console.WriteLine("Creating json file");

            var jsonString = JsonConvert.SerializeObject(moonIlluminations, new JsonSerializerSettings {DateTimeZoneHandling = DateTimeZoneHandling.Utc});
            File.WriteAllText("moon_illumination_2017.json", jsonString, Encoding.UTF8);
        }

        private class MoonIlluminationDto
        {
            public DateTime DateTime { get; set; }
            public double Percent { get; set; }
        }
    }
}
