using AoC.Utilities;

namespace AoC_2025_Day2;

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

        var validRanges = GetValidRanges(inputFile);

        long totalInvalidValues = 0;

        foreach (var validRange in validRanges)
        {
            Console.WriteLine($"Start: {validRange.Start}, End: {validRange.End}");
            List<long> invalidValues;
            if(partNumber==1)
            {
                invalidValues = GetInvalidValuesInRangeForPart1(validRange);
            }
            else
            {
                invalidValues = GetInvalidValuesInRangeForPart2(validRange);
            }

            Console.WriteLine(string.Join(',', invalidValues));
            foreach (var invalidValue in invalidValues)
            {
                totalInvalidValues += invalidValue;
            }
        }

        Console.WriteLine($"Total of all invalid values: {totalInvalidValues}");
        Console.WriteLine();
    }

    private static List<long> GetInvalidValuesInRangeForPart2(ValidRange validRange)
    {
        List<long> output = new List<long>();
        for (long i = validRange.Start; i <= validRange.End; i++)
        {
            string numberAsText = i.ToString();

            for(int j=1; j<=numberAsText.Length/2;j++)
            {
                int numberOfRepeats = numberAsText.Length / j;
                if(j*numberOfRepeats==numberAsText.Length)
                {
                    string repeatingPart = numberAsText.Substring(0, j);
                    string toCheck = string.Concat(Enumerable.Repeat(repeatingPart, numberOfRepeats));
                    if(toCheck==numberAsText)
                    {
                        output.Add(i);
                        break;
                    }
                }               
            }

        }
        return output;
    }

    private static List<long> GetInvalidValuesInRangeForPart1(ValidRange validRange)
    {
        List<long> output = new List<long>();
        for(long i = validRange.Start; i<=validRange.End; i++)
        {
            string numberAsText = i.ToString();
            if(numberAsText.Length % 2 == 0)
            {
                string firstPart = numberAsText.Substring(0, numberAsText.Length / 2);
                string secondPart = numberAsText.Substring(numberAsText.Length / 2);
                if(firstPart==secondPart)
                {
                    output.Add(i);
                }
            }
        }
        return output;
    }

    private static List<ValidRange> GetValidRanges(string inputFile)
    {
        var input = InputParser.ReadInputAsCsvBlock(inputFile);
        var output = new List<ValidRange>();
        foreach (var item in input)
        {
            var parts = item.Split("-");
            output.Add(new ValidRange { Start = long.Parse(parts[0]), End = long.Parse(parts[1]) });
        }
        return output;
    }
}
