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
        
        SolvePart(args[0], 2);
        
    }

    private static void SolvePart(string inputFile, int partNumber)
    {
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        List<Rotation> instructions = GetInstructions(inputFile);

        //Part 1:
        Dial dial = new Dial();
        Console.WriteLine("The dial starts by pointing at 50.");
        int password = 0;
        foreach (var rotation in instructions)
        {
            password = RotateDial(dial, rotation, password, partNumber);            
        }

        Console.WriteLine($"Part {partNumber} Solution: {password}");
        Console.WriteLine();
    }

    private static int RotateDial(Dial dial, Rotation rotation, int password, int partNumber)
    {
        bool startedAt0 = false;
        if(dial.Position==0)
        {
            startedAt0 = true;
        }

        if(rotation.Direction=='L')
        {
            dial.Position -= rotation.Amount;            
        }
        else
        {
            dial.Position += rotation.Amount;            
        }

        int timesPointingAt0 = 0;
        if (partNumber==2)
        {            
            if (dial.Position >= 100)
            {
                timesPointingAt0 = dial.Position / 100;
            }
            if (dial.Position <= 0)
            {
                timesPointingAt0 = ((dial.Position / 100) * -1) + 1;
                if(startedAt0)
                {
                    timesPointingAt0--;
                }
            }
            password += timesPointingAt0;
        }        

        dial.Position %= 100;
        if (dial.Position < 0)
        {
            dial.Position += 100;
        }

        if(partNumber == 1 && dial.Position == 0)
        {
            password++;
        }

        //Console.Write($"The dial is rotated {rotation.Direction}{rotation.Amount} to point at {dial.Position}.");
        //if(partNumber==2)
        //{
        //    Console.Write($"Times pointing at 0: {timesPointingAt0}");
        //}
        //Console.WriteLine();

        return password;
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

internal class Dial
{
    public int Position { get; set; } = 50;
}