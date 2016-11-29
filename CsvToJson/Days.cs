using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CsvToJson
{
    internal class Days
    {
        private const string FileName = "dager2017_utf-8.txt";

        public static void GenerateDaysJson()
        {
            Console.WriteLine($"Reading {FileName}");

            var daysLines = File.ReadAllLines(FileName, Encoding.UTF8).ToList();

            Console.WriteLine($"Read {daysLines.Count} lines from file. Converting to objects.");

            var days = daysLines.Select(l =>
            {
                var lineElements = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return new DayToRemember
                {
                    Name = $"{lineElements[2].Trim()}",
                    Date = DateTime.ParseExact(lineElements[0], "dd.MM.yyyy", null),
                    Type = ConvertToDayType(lineElements[1])
                };
            }).ToArray();

            Console.WriteLine($"{days.Length} objects created from {FileName}.");

            Console.WriteLine("Creating json file");

            var jsonString = JsonConvert.SerializeObject(days);
            File.WriteAllText("days_2017.json", jsonString, Encoding.UTF8);
        }

        private class DayToRemember
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public DayType Type { get; set; }

        }

        private enum DayType
        {
            DayToRemember = 0, // default
            PublicHoliday = 1,
            AstronomicalEvent = 2,
            CourtHoliday = 3,
            ReligiousDay = 4,
            Miscellaneous = 5,
        }

        private static DayType ConvertToDayType(string s)
        {
            switch (s)
            {
                case "Merkedag": return DayType.DayToRemember;
                case "Høytidsdag": return DayType.PublicHoliday;
                case "Astronomisk": return DayType.AstronomicalEvent;
                case "Rettsferie": return DayType.CourtHoliday;
                case "Religiøs": return DayType.ReligiousDay;
                case "Diverse": return DayType.Miscellaneous;
                default: throw new ArgumentException($"Ukjent DayType: {s}");
            }
        }
    }
}
