using AoC.Utilities;

namespace AoC_2025_Day3;

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
        
        //SolvePart(args[0], 2);        
    }

    private static void SolvePart(string inputFile, int partNumber)
    {
        Console.WriteLine($"Part {partNumber}:");
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        List<BatteryBank> batteryBanks = LoadBatteries(inputFile);

        foreach (var batteryBank in batteryBanks)
        {
            foreach (var battery in batteryBank.Batteries)
            {
                Console.Write($"{battery.Joltage}");   
            }
            Console.WriteLine();
        }

    }

    private static List<BatteryBank> LoadBatteries(string inputFile)
    {
        List<string> inputRows = InputParser.ReadInputAsRows(inputFile);
        List<BatteryBank> batteryBanks = new List<BatteryBank>();
        foreach (string inputRow in inputRows)
        {
            batteryBanks.Add(new BatteryBank(inputRow));
        }

        return batteryBanks;
    }
}
