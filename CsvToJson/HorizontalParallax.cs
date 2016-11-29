using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CsvToJson
{
    internal class HorizontalParallax
    {
        private const string InputFile = "moon_hor_parallax_2017_30m.txt";
        private const string OutputFile = "moon_hor_parallax_2017.json";

        public static void GenerateHorizontalParallaxOfMoon()
        {
            Console.WriteLine($"Reading {InputFile}");

            var lines = File.ReadAllLines(InputFile).ToList();

            Console.WriteLine($"Read {lines.Count} lines from file. Converting to objects.");

            var horizontalParallaxes = lines.Select(l =>
            {
                var lineElements = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return new HorizontalParallaxDto
                {
                    DateTime =
                        DateTime.ParseExact(lineElements[0], "yyyy MMM dd HH:mm", CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal),
                    Arc = Convert.ToDouble(lineElements[1].Trim(), CultureInfo.InvariantCulture)
                };
            }).ToArray();

            Console.WriteLine($"{horizontalParallaxes.Length} objects created from {InputFile}.");

            Console.WriteLine($"Creating json file {OutputFile}");

            var jsonString = JsonConvert.SerializeObject(horizontalParallaxes, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
            File.WriteAllText(OutputFile, jsonString, Encoding.UTF8);
        }

        private class HorizontalParallaxDto
        {
            public DateTime DateTime { get; set; }
            public double Arc { get; set; }
        }
    }
}
