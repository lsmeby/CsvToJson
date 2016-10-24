using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CsvToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting csv to json converter");
            Console.WriteLine("Reading norge.txt");

            var norwayLines = File.ReadAllLines("norge.txt", Encoding.UTF8).Skip(1).ToList();

            Console.WriteLine($"Read {norwayLines.Count} lines from file. Converting to objects.");

            var norwayPlaces = norwayLines.Select(l =>
            {
                var lineElements = l.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                return new Place
                {
                    Name = lineElements[1].Trim(),
                    Type = lineElements[4].Trim(),
                    Location = $"{lineElements[6].Trim()}, {lineElements[7].Trim()}",
                    Lat = Convert.ToDouble(lineElements[8].Trim(), CultureInfo.InvariantCulture),
                    Long = Convert.ToDouble(lineElements[9].Trim(), CultureInfo.InvariantCulture)
                };
            }).ToList();

            Console.WriteLine($"{norwayPlaces.Count} objects created from norway.txt.");
            Console.WriteLine("Reading verden.txt");

            var worldLines = File.ReadAllLines("verden.txt", Encoding.UTF8).Skip(1).ToList();

            Console.WriteLine($"Read {worldLines.Count} lines from file. Converting  to objects.");

            var worldPlaces = worldLines.Select(l =>
            {
                var lineElements = l.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                return new Place
                {
                    Name = lineElements[2].Trim(),
                    Type = lineElements[6].Trim(),
                    Location = lineElements[9].Trim(),
                    Lat = Convert.ToDouble(lineElements[12].Trim(), CultureInfo.InvariantCulture),
                    Long = Convert.ToDouble(lineElements[13].Trim(), CultureInfo.InvariantCulture)
                };
            }).ToList();

            Console.WriteLine($"{worldPlaces.Count} objects created from verden.txt.");
            Console.WriteLine("Concatinating and sorting places.");

            var places = norwayPlaces
                .Concat(worldPlaces)
                .OrderBy(p => p.Name)
                .ToArray();

            Console.WriteLine("Creating json file");

            var jsonString = JsonConvert.SerializeObject(places);
            File.WriteAllText("places.json", jsonString, Encoding.UTF8);

            Console.WriteLine("Json file created. Press any key to exit.");

            Console.Read();
        }

        private class Place
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Location { get; set; }
            public double Lat { get; set; }
            public double Long { get; set; }
        }

        // Columns in norge.txt:
        // =====================
        // [0] Kommunenummer
        // [1] Stadnamn
        // [2] Prioritet
        // [3] Stadtype nynorsk
        // [4] Stadtype bokmål
        // [5] Stadtype engelsk
        // [6] Kommune
        // [7] Fylke
        // [8] Lat
        // [9] Lon
        // [10] Høgd
        // [11] Nynorsk url
        // [12] Bokmål url
        // [13] Engelsk url

        // Columns in verden.txt:
        // ======================
        // [0] Landskode
        // [1] Stadnamn nynorsk
        // [2] Stadnamn bokmål
        // [3] Stadnamn engelsk
        // [4] Geonames-ID
        // [5] Stadtype nynorsk
        // [6] Stadtype bokmål
        // [7] Stadtype engelsk
        // [8] Landsnamn nynorsk
        // [9] Landsnamn bokmål
        // [10] Landsnamn engelsk
        // [11] Folketal
        // [12] Lat
        // [13] Lon
        // [14] Høgd over havet
        // [15] Lenke til nynorsk-XML
        // [16] Lenke til bokmåls-XML
        // [17] Lenke til engelsk-XML
    }
}
