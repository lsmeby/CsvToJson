using System;

namespace CsvToJson
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting csv to json converter");
            
            //Places.GeneratePlacesJson();
            Days.GenerateDaysJson();

            Console.WriteLine("Json file created. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
