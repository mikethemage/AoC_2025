using AoC.Utilities;

namespace AoC_2025_Day6;

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

        ProblemContainer problemContainer = GetProblems(inputFile);

        Console.WriteLine($"{problemContainer.GetGrandTotal()}");
    }

    private static ProblemContainer GetProblems(string inputFile)
    {
        var lines = InputParser.ReadInputAsRows(inputFile);
        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines.Last()))
        {
            lines.Remove(lines.Last());
        }

        var problemContainer = new ProblemContainer();
        for (int i = 0; i < lines.Count; i++)
        {
            var parts = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < parts.Length; j++)
            {
                if (i < lines.Count - 1)
                {
                    problemContainer.AddValue(j, long.Parse(parts[j]));
                }
                else
                {
                    problemContainer.AddOperation(j, parts[j]);
                }
            }
        }

        return problemContainer;
    }
}
