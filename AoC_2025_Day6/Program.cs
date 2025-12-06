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

        SolvePart(args[0], 2);
    }

    private static void SolvePart(string inputFile, int partNumber)
    {
        Console.WriteLine($"Part {partNumber}:");
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        ProblemContainer problemContainer;
        if (partNumber==1)
        {
            problemContainer = GetProblemsForPart1(inputFile);
        }
        else
        {
            problemContainer = GetProblemsForPart2(inputFile);
        }
        

        Console.WriteLine($"{problemContainer.GetGrandTotal()}");
    }

    private static ProblemContainer GetProblemsForPart2(string inputFile)
    {
        var lines = InputParser.ReadInputAsRows(inputFile);
        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines.Last()))
        {
            lines.Remove(lines.Last());
        }

        ProblemContainer output = new ProblemContainer();
        int currentProblemId = -1;
        int maxLineLength = lines.Max(x => x.Length);
        
        for (int i = 0; i<maxLineLength;i++)
        {
            string operation = lines.Last()[i].ToString();
            if (!string.IsNullOrWhiteSpace(operation))
            {
                currentProblemId++;
                output.AddOperation(currentProblemId,operation);                
            }
            if(currentProblemId>=0)
            {
                string value = string.Empty;
                foreach (var line in lines.Take(lines.Count-1))
                {
                    if(line.Length>i)
                    {
                        string part = line[i].ToString();
                        if(!string.IsNullOrWhiteSpace(part))
                        {
                            value += part;
                        }
                    }
                }
                if(!string.IsNullOrWhiteSpace(value))
                {
                    output.AddValue(currentProblemId,long.Parse(value));
                }
            }
        }

        return output;        
    }

    private static ProblemContainer GetProblemsForPart1(string inputFile)
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
