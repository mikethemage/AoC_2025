using AoC.Utilities;

namespace AoC_2025_Day9;

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

        List<Tile> tiles = LoadTiles(inputFile);

        //VisualizeTiles(tiles);
        //Console.WriteLine();

        List<Rectangle> rectangles = new List<Rectangle>();
        for (int i = 0; i < tiles.Count - 1; i++)
        {
            for (int j = i + 1; j < tiles.Count; j++)
            {
                rectangles.Add(new Rectangle { Tile1 = tiles[i], Tile2 = tiles[j] });
            }
        }

        Rectangle? biggestRectangle = rectangles.MaxBy(x => x.Area);
        //VisualizeTiles(tiles, biggestRectangle);
        //Console.WriteLine();

        if (biggestRectangle is not null)
        {
            Console.WriteLine($"Biggest Rectangle Area: {biggestRectangle.Area}");
        }

    }

    private static void VisualizeTiles(List<Tile> tiles, Rectangle? rectangle = null)
    {
        for (int j = tiles.Min(x => x.Row) - 1; j <= tiles.Max(x => x.Row) + 1; j++)
        {
            for (int i = tiles.Min(x => x.Column) - 1; i <= tiles.Max(x => x.Column) + 1; i++)
            {
                if (rectangle is not null
                    && j >= Math.Min(rectangle.Tile1.Row, rectangle.Tile2.Row)
                    && j <= Math.Max(rectangle.Tile1.Row, rectangle.Tile2.Row)
                    && i >= Math.Min(rectangle.Tile1.Column, rectangle.Tile2.Column)
                    && i <= Math.Max(rectangle.Tile1.Column, rectangle.Tile2.Column)
                    )
                {
                    Console.Write('O');
                }
                else if (tiles.Any(t => t.Row == j && t.Column == i))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }

    private static List<Tile> LoadTiles(string inputFile)
    {
        List<Tile> tiles = new List<Tile>();
        var inputData = InputParser.ReadInputAsCsvIntRows(inputFile);
        foreach (var item in inputData)
        {
            tiles.Add(new Tile { Column = item[0], Row = item[1] });
        }

        return tiles;
    }
}
