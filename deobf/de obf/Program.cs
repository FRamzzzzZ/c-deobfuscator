using de_obf;
using System;

namespace DeObf
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the C# Deobfuscator!");
            Console.WriteLine("Please enter the file path of the obfuscated C# code:");
            string filePath = Console.ReadLine();

            // Check if the file path is valid
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Invalid file path. Please provide a valid path.");
                return;
            }

            // Create an instance of the Analyze class and start the analysis
            Analyze analyzer = new Analyze(filePath);
            analyzer.StartAnalysis();

            // Create an instance of the Deobf class and start the deobfuscation
            Deobf deobfuscator = new Deobf(filePath);
            deobfuscator.StartDeobfuscation();

            Console.WriteLine("Deobfuscation process complete!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}