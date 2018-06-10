using System;
using System.IO;
using TokenCalculator.Generator;

namespace TokenCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            File.WriteAllText(@"C:\Users\noarein\Documents\Useful\TokenCalculator2.json", 
                Utf8Json.JsonSerializer.PrettyPrint(Utf8Json.JsonSerializer.Serialize(new TokenSetGenerator().Get())));
            Console.ReadLine();
        }
    }
}
