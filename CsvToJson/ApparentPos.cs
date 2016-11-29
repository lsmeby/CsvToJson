using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CsvToJson
{
    internal class ApparentPos
    {
        private const string InputFileNameFormat = "apparent_pos_{0}_2017_30m.txt";
        private const string OutputFileNameFormat = "apparent_pos_{0}_2017.json";
        private static readonly string[] Objects = {"sun", "moon", "mercury", "venus", "mars", "jupiter", "saturn"};

        public static void GenerateApparentPos()
        {
            Console.WriteLine($"Generating json files for {string.Join(", ", Objects)}.");

            foreach (var aObject in Objects)
            {
                GenerateApparentPosForObject(
                    string.Format(InputFileNameFormat, aObject),
                    string.Format(OutputFileNameFormat, aObject));
            }
        }

        private static void GenerateApparentPosForObject(string inputFile, string outputFile)
        {
            Console.WriteLine($"Reading {inputFile}");

            var lines = File.ReadAllLines(inputFile).ToList();

            Console.WriteLine($"Read {lines.Count} lines from file. Converting to objects.");

            var apparentPositions = lines.Select(l =>
            {
                var lineElements = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return new ApparentPositionDto
                {
                    DateTime =
                        DateTime.ParseExact(lineElements[0], "yyyy MMM dd HH:mm:ss.f", CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal),
                    Rectascension = Convert.ToDouble(lineElements[1].Trim(), CultureInfo.InvariantCulture),
                    Declination = Convert.ToDouble(lineElements[2].Trim(), CultureInfo.InvariantCulture)
                };
            }).ToArray();

            Console.WriteLine($"{apparentPositions.Length} objects created from {inputFile}.");

            Console.WriteLine($"Creating json file {outputFile}");

            var jsonString = JsonConvert.SerializeObject(apparentPositions, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
            File.WriteAllText(outputFile, jsonString, Encoding.UTF8);
        }

        private class ApparentPositionDto
        {
            public DateTime DateTime { get; set; }
            public double Rectascension { get; set; }
            public double Declination { get; set; }
        }
    }
}
