using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CsvToJson
{
    internal class ApparentSiderial
    {
        private const string InputFile = "apparent_siderial_greenwich_2017_30m.txt";
        private const string OutputFile = "apparent_siderial_greenwich_2017.json";

        public static void GenerateApparentSiderialAtGreenwich()
        {
            Console.WriteLine($"Reading {InputFile}");

            var lines = File.ReadAllLines(InputFile).ToList();

            Console.WriteLine($"Read {lines.Count} lines from file. Converting to objects.");

            var siderialTimes = lines.Select(l =>
            {
                var lineElements = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return new SiderialTimeDto
                {
                    DateTime =
                        DateTime.ParseExact(lineElements[0], "yyyy MMM dd HH:mm:ss.f", CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal),
                    SiderialTime = Convert.ToDouble(lineElements[1].Trim(), CultureInfo.InvariantCulture)
                };
            }).ToArray();

            Console.WriteLine($"{siderialTimes.Length} objects created from {InputFile}.");

            Console.WriteLine($"Creating json file {OutputFile}");

            var jsonString = JsonConvert.SerializeObject(siderialTimes, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
            File.WriteAllText(OutputFile, jsonString, Encoding.UTF8);
        }

        private class SiderialTimeDto
        {
            public DateTime DateTime { get; set; }
            public double SiderialTime { get; set; }
        }
    }
}
