using AoC.Utilities;

namespace AoC_2025_Day12;

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

        var lines = InputParser.ReadInputAsRows(inputFile);
        bool spaceMode = false;
        List<TreeRegion> treeRegions = new List<TreeRegion>();
        List<Present> presents = new List<Present>();
        Present? currentPresent = null;

        foreach (string line in lines)
        {
            if(line.Contains('x'))
            {
                spaceMode = true;
            }
            if(spaceMode)
            {
                if(!string.IsNullOrWhiteSpace(line))
                {
                    string[] parts = line.Split(": ", StringSplitOptions.TrimEntries);
                    List<int> dimensions = parts[0].Split("x", StringSplitOptions.TrimEntries).Select(int.Parse).ToList();
                    List<int> requiredShapes = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                    treeRegions.Add(new TreeRegion { Width = dimensions[0], Height = dimensions[1], NumbersOfShapesRequired = requiredShapes });
                }
            }
            else
            {
                if (line.Contains(':'))
                {
                    //new present
                    currentPresent = new Present { Id = int.Parse(line.TrimEnd(":"))};
                }
                else if(string.IsNullOrWhiteSpace(line))
                {
                    //finish current present
                    presents.Add(currentPresent!);
                }
                else
                {
                    //add current line to present
                    currentPresent!.Layout.Add(line.ToCharArray().ToList());
                }
            }
        }

        int regionsThatCanFitAllPresents = 0;
        foreach (TreeRegion treeRegion in treeRegions)
        {
            int totalNumberOfShapes = 0;
            int totalFilledSpaces = 0;
            for (int i = 0; i < treeRegion.NumbersOfShapesRequired.Count; i++)
            {
                if (treeRegion.NumbersOfShapesRequired[i]>0)
                {
                    int numberOfShape = treeRegion.NumbersOfShapesRequired[i];
                    totalNumberOfShapes += numberOfShape;
                    Present present = presents.First(x => x.Id == i);
                    int filledSpacesForShape = present.Layout.Sum(x => x.Count(y => y == '#'));
                    totalFilledSpaces += filledSpacesForShape * numberOfShape;
                }
            }
            if((treeRegion.Width/3) * (treeRegion.Height/3) >= totalNumberOfShapes)
            {
                if(treeRegion.Width*treeRegion.Height >= totalFilledSpaces)
                {
                    //Might be valid
                    regionsThatCanFitAllPresents++;
                }
            }
        }
        Console.WriteLine(regionsThatCanFitAllPresents);
    }   
}

internal class TreeRegion
{
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required List<int> NumbersOfShapesRequired { get; init; }
}

internal class Present
{
    public required int Id { get; init; }
    public List<List<char>> Layout { get; private set; } = new List<List<char>>();
}