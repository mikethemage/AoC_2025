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

        //SolvePart(args[0], 2);
    }

    private static void SolvePart(string inputFile, int partNumber)
    {
        Console.WriteLine($"Part {partNumber}:");
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        Kitchen kitchen = LoadRangesAndIngredients(inputFile);

        List<long> freshIngredients = GetFreshIngredients(kitchen);

        Console.WriteLine($"Number of fresh ingredients: {freshIngredients.Count}");
        
    }

    private static Kitchen LoadRangesAndIngredients(string inputFile)
    {
        Kitchen output = new Kitchen();
        List<string> input = InputParser.ReadInputAsRows(inputFile);
        bool rangeMode = true;
        foreach (string line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                rangeMode = false;
            }
            else if (rangeMode)
            {
                string[] parts = line.Split("-");
                output.Ranges.Add(new Range { Min = long.Parse(parts[0]), Max = long.Parse(parts[1]) });
            }
            else
            {
                output.Ingredients.Add(long.Parse(line));
            }
        }
        return output;
    }

    private static List<long> GetFreshIngredients(Kitchen input)
    {
        List<long> output = new List<long>();
        foreach (long ingredient in input.Ingredients)
        {
            Range? validRange = input.Ranges.FirstOrDefault(x => ingredient >= x.Min && ingredient <= x.Max);
            if(validRange is not null)
            {
                output.Add(ingredient);
            }
        }
        return output;
    }
}
