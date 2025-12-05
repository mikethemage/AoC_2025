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

        Kitchen kitchen = LoadRangesAndIngredients(inputFile);
        
        if(partNumber==1)
        {
            List<long> freshIngredients = GetFreshIngredients(kitchen);
            Console.WriteLine($"Number of fresh ingredients: {freshIngredients.Count}");
            Console.WriteLine();
        }
        else
        {
            List<Range> combinedRanges = CombineRanges(kitchen.Ranges);
            foreach (var range in combinedRanges)
            {
                Console.WriteLine($"{range.Min}-{range.Max}");
            }
            Console.WriteLine($"{combinedRanges.Sum(x => (x.Max-x.Min) + 1)}");
            Console.WriteLine();
        }
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

    private static List<Range> CombineRanges(List<Range> input)
    {
        List<Range> output = new List<Range>();
        List<Range> ranges = input.OrderBy(x => x.Min).ThenBy(x => x.Max).ToList();
        while(ranges.Count>0)
        {
            Range currentRange = ranges.First();
            ranges.Remove(currentRange);
            List<Range> newRanges = new List<Range>();
            foreach (Range range in ranges)
            {
                if(range.Min <= currentRange.Max && range.Max >= currentRange.Min)
                {
                    Range newRange = new Range { Min = Math.Min(range.Min, currentRange.Min), Max = Math.Max(range.Max, currentRange.Max) };                    
                    currentRange = newRange;
                }
                else
                {
                    newRanges.Add(range);
                }
            }
            ranges = newRanges;
            output.Add(currentRange);
        }
        return output;
    }
}
