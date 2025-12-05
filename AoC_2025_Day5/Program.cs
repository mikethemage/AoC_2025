using AoC.Utilities;

namespace AoC_2025_Day5;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("filename is required!");
            throw new Exception("No file provided!");
        }
        
        SolvePart(args[0], 1);
        
        SolvePart(args[0], 2);        
    }

    private static void SolvePart(string inputFile, int partNumber)
    {
        Console.WriteLine($"Part {partNumber}:");
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

               
    }
}
