using AoC.Utilities;

namespace AoC_2025_Day4;

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

        Grid grid = LoadRolls(inputFile);

        if(partNumber==1)
        {
            //Console.WriteLine("Before:");
            //OutputGrid(grid);
            //Console.WriteLine();

            grid.UpdateRollAccessibility();

            //Console.WriteLine("After:");
            //OutputGrid(grid);
            //Console.WriteLine();

            Console.WriteLine($"Accessible Rolls: {grid.GetAccessibleRollCount()}");
            Console.WriteLine();
        }
        else
        {
            int totalRemoved = GetTotalCanBeRemoved(grid);

            Console.WriteLine($"Total Removed: {totalRemoved}");
        }
    }

    private static int GetTotalCanBeRemoved(Grid grid)
    {
        int totalRemoved = 0;
        int toRemove;
        do
        {
            grid.UpdateRollAccessibility();
            toRemove = grid.GetAccessibleRollCount();
            if (toRemove > 0)
            {
                grid.RemoveAccessibleRolls();
                totalRemoved += toRemove;
            }
        } while (toRemove > 0);
        return totalRemoved;
    }

    private static Grid LoadRolls(string inputFile)
    {
        Grid grid = new Grid();
        List<string> input = InputParser.ReadInputAsRows(inputFile);
        for (int j = 0; j < input.Count; j++)
        {
            for (int i = 0; i < input[j].Length; i++)
            {
                if (input[j][i] == '@')
                {
                    grid.AddRoll(j, i);
                }
            }
        }

        return grid;
    }

    private static void OutputGrid(Grid grid)
    {
        for (int j = grid.MinRow; j <= grid.MaxRow; j++)
        {
            for (int i = grid.MinColumn; i <= grid.MaxColumn; i++)
            {
                Console.Write(grid.GetGridLocationSymbol(j, i));
            }
            Console.WriteLine();
        }
    }
}
