using AoC.Utilities;

namespace AoC_2025_Day7;

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

        Manifold manifold = LoadManifold(inputFile);

        int result = manifold.Run();

        Console.WriteLine($"Result: {result}");

    }

    private static Manifold LoadManifold(string inputFile)
    {
        List<string> input = InputParser.ReadInputAsRows(inputFile);
        Manifold manifold = new Manifold { Height = input.Count };
        for (int j = 0; j < input.Count; j++)
        {
            for (int i = 0; i < input[j].Length; i++)
            {
                if (input[j][i] == 'S')
                {
                    manifold.Init(new Beam { Column = i, Row = j });
                }
                else if (input[j][i] == '^')
                {
                    manifold.AddSplitter(new Splitter { Column = i, Row = j });
                }
            }
        }

        return manifold;
    }
}
