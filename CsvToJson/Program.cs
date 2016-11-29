using System;

namespace CsvToJson
{
    internal class Program
    {
        private const bool GeneratePlaces = false;
        private const bool GenerateDays = false;
        private const bool GenerateMoonIllumination = false;
        private const bool GenerateApparentPos = false;
        private const bool GenerateApparentSiderialGreenwich = false;
        private const bool GenerateHorizontalParallaxOfMoon = false;
        private const bool GenerateMoonPhases = false;

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting csv to json converter");

            if (GeneratePlaces)
                Places.GeneratePlacesJson();
            if(GenerateDays)
                Days.GenerateDaysJson();
            if(GenerateMoonIllumination)
                MoonIllumination.GenerateMoonIlluminationJson();
            if(GenerateApparentPos)
                ApparentPos.GenerateApparentPos();
            if(GenerateApparentSiderialGreenwich)
                ApparentSiderial.GenerateApparentSiderialAtGreenwich();
            if(GenerateHorizontalParallaxOfMoon)
                HorizontalParallax.GenerateHorizontalParallaxOfMoon();
            if(GenerateMoonPhases)
                MoonPhases.GenerateMoonPhases();

            Console.WriteLine("Json file(s) created. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
