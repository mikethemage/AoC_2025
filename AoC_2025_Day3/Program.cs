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
        
        //SolvePart(args[0], 1);
        
        SolvePart(args[0], 2);        
    }

    private static void SolvePart(string inputFile, int partNumber)
    {
        Console.WriteLine($"Part {partNumber}:");
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        List<BatteryBank> batteryBanks = LoadBatteries(inputFile);

        long totalJoltage = 0;
        foreach (BatteryBank batteryBank in batteryBanks)
        {
            if(partNumber==1)
            {
                Battery firstHighest = batteryBank.GetFirstHighest(1);
                Battery secondHighest = batteryBank.GetSecondHighest(firstHighest.Id, 0);

                int joltage = (firstHighest.Joltage * 10) + secondHighest.Joltage;
                totalJoltage += joltage;
                Console.WriteLine($"{joltage}");
            }
            else
            {
                Battery previous = batteryBank.GetFirstHighest(11);
                long joltage = previous.Joltage * (long)Math.Pow(10, 11);
                for (int i = 10; i >= 0; i--)
                {
                    previous = batteryBank.GetSecondHighest(previous.Id, i);
                    joltage += previous.Joltage * (long)Math.Pow(10, i);
                }
                totalJoltage += joltage;
                Console.WriteLine($"{joltage}");
            }
            
        }
        Console.WriteLine($"Total: {totalJoltage}");
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
