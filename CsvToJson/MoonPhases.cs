using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CsvToJson
{
    internal class MoonPhases
    {
        private const string InputFile = "moon_phases_2017_utf-8.txt";
        private const string OutputFile = "moon_phases_2017.json";

        public static void GenerateMoonPhases()
        {
            Console.WriteLine($"Reading {InputFile}");

            var lines = File.ReadAllLines(InputFile, Encoding.UTF8).ToList();

            Console.WriteLine($"Read {lines.Count} lines from file. Converting to objects.");

            var moonPhases = lines.Select(l =>
            {
                var lineElements = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return new MoonPhaseDto
                {
                    DateTime =
                        DateTime.ParseExact(lineElements[1], "yyyy MMM dd HH:mm", CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal),
                    MoonPhase = ConvertToMoonPhase(lineElements[0].Trim())
                };
            }).ToArray();

            Console.WriteLine($"{moonPhases.Length} objects created from {InputFile}.");

            Console.WriteLine($"Creating json file {OutputFile}");

            var jsonString = JsonConvert.SerializeObject(moonPhases, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
            File.WriteAllText(OutputFile, jsonString, Encoding.UTF8);
        }

        private static MoonPhase ConvertToMoonPhase(string input)
        {
            switch (input)
            {
                case "Nymåne:":
                    return MoonPhase.NewMoon;
                case "Voksende halvmåne:":
                    return MoonPhase.FirstQuarter;
                case "Fullmåne:":
                    return MoonPhase.FullMoon;
                case "Minkende halvmåne:":
                    return MoonPhase.ThirdQuarter;
                default:
                    throw new Exception("Unknown moon phase");
            }
        }

        private class MoonPhaseDto
        {
            public DateTime DateTime { get; set; }
            public MoonPhase MoonPhase { get; set; }
        }

        private enum MoonPhase
        {
            NewMoon = 0,
            FirstQuarter = 1,
            FullMoon = 2,
            ThirdQuarter = 3
        }
    }
}
