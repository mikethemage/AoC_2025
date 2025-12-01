using AoC.Utilities;

namespace AoC_2025_Day1;

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
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        List<Rotation> instructions = GetInstructions(inputFile);

        //Part 1:
        int dialPosition = 50;
        Console.WriteLine("The dial starts by pointing at 50.");
        int password = 0;
        foreach (var rotation in instructions)
        {
            dialPosition = RotateDial(dialPosition, rotation);
            if(dialPosition==0)
            {
                password++;
            }
        }

        Console.WriteLine($"Part {partNumber} Solution: {password}");
    }

    private static int RotateDial(int dialPosition, Rotation rotation)
    {
        if(rotation.Direction=='L')
        {
            dialPosition -= rotation.Amount;            
        }
        else
        {
            dialPosition += rotation.Amount;            
        }

        dialPosition %= 100;
        if (dialPosition < 0)
        {
            dialPosition += 100;
        }

        Console.WriteLine($"The dial is rotated {rotation.Direction}{rotation.Amount} to point at {dialPosition}.");

        return dialPosition;
    }

    private static List<Rotation> GetInstructions(string inputFile)
    {
        List<string> input = InputParser.ReadInputAsRows(inputFile);
        List<Rotation> instructions = new List<Rotation>();
        foreach (var row in input)
        {
            instructions.Add(
            new Rotation
            {
                Direction = row[0],
                Amount = int.Parse(row.Substring(1))
            });
        }
        return instructions;
    }
}
