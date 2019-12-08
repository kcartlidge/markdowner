using Markdowner.Generators;
using System;
using System.IO;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading and parsing 'example.md'.");
            var text = File.ReadAllText("example.md");

            Console.WriteLine("Writing 'example.html'.");
            File.WriteAllText("example.html", new HTML().Generate(text).ToString());

            Console.WriteLine();
            Console.Write("Press enter/return ... ");
            Console.ReadLine();
        }
    }
}
