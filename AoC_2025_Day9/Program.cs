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

        SolvePart(args[0], 2);
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


        if (partNumber == 1)
        {
            Rectangle? biggestRectangle = rectangles.MaxBy(x => x.Area);
            //VisualizeTiles(tiles, biggestRectangle);
            //Console.WriteLine();

            if (biggestRectangle is not null)
            {
                Console.WriteLine($"Biggest Rectangle Area: {biggestRectangle.Area}");
            }
        }
        else
        {
            //Order rectangles by area:
            rectangles = rectangles.OrderByDescending(x => x.Area).ToList();

            //Todo - calculate bounding borders and use in validation.
            List<Line> borderLine = GetBorderLines(tiles);
           

            //VisualizeTilesWithFilled(tiles, completedFilled);
            //Console.WriteLine();

            foreach (Rectangle rectangle in rectangles)
            {
                if (IsValidRectangle(rectangle, entireBorder))
                {
                    //VisualizeTiles(tiles, rectangle);
                    //Console.WriteLine();
                    Console.WriteLine($"Biggest Valiid Rectangle Area: {rectangle.Area}");
                    break;
                }
            }
        }
    }

    private static List<Line> GetBorderLines(List<Tile> tiles)
    {
        throw new NotImplementedException();
    }

    private static bool IsValidRectangle(Rectangle rectangle, List<Tile> entireBorder)
    {
        if(    !ValidBoundingCorner(rectangle.Tile1.Row,rectangle.Tile1.Column, entireBorder)
            || !ValidBoundingCorner(rectangle.Tile1.Row, rectangle.Tile2.Column, entireBorder)
            || !ValidBoundingCorner(rectangle.Tile2.Row, rectangle.Tile2.Column, entireBorder)
            || !ValidBoundingCorner(rectangle.Tile2.Row, rectangle.Tile1.Column, entireBorder)
            )
        {
            return false;
        }

        //Check each side of the rectangle for crossing borders

    }

    private static bool ValidBoundingCorner(int row, int column, List<Tile> entireBorder)
    {
        if(entireBorder.Where(x=>x.Row == row && x.Column < column).Count() % 2 == 0
            && entireBorder.Where(x => x.Row == row && x.Column <= column).Count() % 2 == 0
            )
        {
            return false;
        }

        if (entireBorder.Where(x => x.Column == column && x.Row < row).Count() % 2 == 0
            && entireBorder.Where(x => x.Column == column && x.Row <= row).Count() % 2 == 0
            )
        {
            return false;
        }

        return true;
    }

    private static List<Tile> GetBorderTiles(List<Tile> tiles)
    {
        List<Tile> borderTiles = new List<Tile>();
        Tile previousTile = tiles[0];
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile currentTile;
            if(i==tiles.Count-1)
            {
                currentTile = tiles[0];
            }
            else
            {
                currentTile = tiles[i+1];
            }
            
            if (currentTile.Row == previousTile.Row)
            {
                for (int j = Math.Min(currentTile.Column, previousTile.Column) + 1; j < Math.Max(currentTile.Column, previousTile.Column); j++)
                {
                    borderTiles.Add(new Tile { Column = j, Row = currentTile.Row });
                }
            }
            if (currentTile.Column == previousTile.Column)
            {
                for (int j = Math.Min(currentTile.Row, previousTile.Row) + 1; j < Math.Max(currentTile.Row, previousTile.Row); j++)
                {
                    borderTiles.Add(new Tile { Row = j, Column = currentTile.Column });
                }
            }
            previousTile = currentTile;
        }
        return borderTiles;
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

internal class Line
{

}