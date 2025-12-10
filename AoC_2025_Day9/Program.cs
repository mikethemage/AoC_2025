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
            List<Line> borderLines = GetBorderLines(tiles);
           

            //VisualizeTilesWithFilled(tiles, completedFilled);
            //Console.WriteLine();

            foreach (Rectangle rectangle in rectangles)
            {
                if (IsValidRectangle(rectangle, borderLines))
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
        List<Line> borderLines = new List<Line>();
        Tile previousTile = tiles[0];
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile currentTile;
            if (i == tiles.Count - 1)
            {
                currentTile = tiles[0];
            }
            else
            {
                currentTile = tiles[i + 1];
            }

            if (currentTile.Row == previousTile.Row)
            {
                borderLines.Add(new Line { StartRow = currentTile.Row, EndRow = currentTile.Row, StartColumn = Math.Min(currentTile.Column, previousTile.Column), EndColumn = Math.Max(currentTile.Column, previousTile.Column) });
            }
            if (currentTile.Column == previousTile.Column)
            {
                borderLines.Add(new Line { StartColumn = currentTile.Column, EndColumn = currentTile.Column, StartRow = Math.Min(currentTile.Row, previousTile.Row), EndRow = Math.Max(currentTile.Row, previousTile.Row) });
            }
            previousTile = currentTile;
        }
        return borderLines;
    }

    private static bool IsValidRectangle(Rectangle rectangle, List<Line> borderLines)
    {
        if (!ValidBounding((rectangle.Tile1.Row +rectangle.Tile2.Row)/2 ,(rectangle.Tile1.Column+rectangle.Tile2.Column)/2, borderLines)
            //|| 
            //!ValidBounding(rectangle.Tile1.Row, rectangle.Tile1.Column, borderLines)
            //|| !ValidBounding(rectangle.Tile1.Row, rectangle.Tile2.Column, borderLines)
            //|| !ValidBounding(rectangle.Tile2.Row, rectangle.Tile1.Column, borderLines)
            //|| !ValidBounding(rectangle.Tile2.Row, rectangle.Tile2.Column, borderLines)
            )
        {
            return false;
        }

        //Check each side of the rectangle for crossing borders
        List<Line> perimeter = rectangle.GetPerimeter();
        var item = perimeter.Where(x => x.StartColumn == x.EndColumn).MinBy(x => x.StartColumn);        
        if(CheckHorizontalPerimeterLeft(item!, borderLines.Where(x => x.StartRow == x.EndRow)))
        {
            return false;
        }
        item = perimeter.Where(x => x.StartColumn == x.EndColumn).MaxBy(x => x.StartColumn);
        if (CheckHorizontalPerimeterRight(item!, borderLines.Where(x => x.StartRow == x.EndRow)))
        {
            return false;
        }

        item = perimeter.Where(x => x.StartRow == x.EndRow).MinBy(x => x.StartRow);        
        if(CheckVerticalPerimeterTop(item!, borderLines.Where(x => x.StartColumn == x.EndColumn)))
        {
            return false;
        }

        item = perimeter.Where(x => x.StartRow == x.EndRow).MaxBy(x => x.StartRow);
        if (CheckVerticalPerimeterBottom(item!, borderLines.Where(x => x.StartColumn == x.EndColumn)))
        {
            return false;
        }


        return true;
    }

    private static bool CheckVerticalPerimeterTop(Line horizontalLine, IEnumerable<Line> verticalLines)
    {
        return verticalLines.Any(x => x.StartRow <= horizontalLine.StartRow && x.EndRow > horizontalLine.StartRow
            && x.StartColumn < horizontalLine.EndColumn
            && x.StartColumn > horizontalLine.StartColumn);
    }

    private static bool CheckVerticalPerimeterBottom(Line horizontalLine, IEnumerable<Line> verticalLines)
    {
        return verticalLines.Any(x => x.StartRow < horizontalLine.StartRow && x.EndRow >= horizontalLine.StartRow
            && x.StartColumn < horizontalLine.EndColumn
            && x.StartColumn > horizontalLine.StartColumn);
    }

    private static bool CheckHorizontalPerimeterLeft(Line verticalLine, IEnumerable<Line> horizontalLines)
    {
        return horizontalLines.Any(x => x.StartColumn <= verticalLine.StartColumn && x.EndColumn > verticalLine.StartColumn 
            && x.StartRow < verticalLine.EndRow
            && x.StartRow > verticalLine.StartRow);
    }

    private static bool CheckHorizontalPerimeterRight(Line verticalLine, IEnumerable<Line> horizontalLines)
    {
        return horizontalLines.Any(x => x.StartColumn < verticalLine.StartColumn && x.EndColumn >= verticalLine.StartColumn
            && x.StartRow < verticalLine.EndRow
            && x.StartRow > verticalLine.StartRow);
    }

    private static bool ValidBounding(int row, int column, List<Line> borderLines)
    {
        if(
            (borderLines.Where(x=>x.StartRow <= row && x.EndRow >= row && x.StartColumn==x.EndColumn && x.StartColumn < column).Count() % 2 == 0
            //&& borderLines.Where(x => x.StartRow <= row && x.EndRow >= row && x.StartColumn == x.EndColumn && x.StartColumn <= column).Count() % 2 == 0
            )
            || (borderLines.Where(x => x.StartColumn <= column && x.EndColumn >= column && x.StartRow == x.EndRow && x.StartRow < row).Count() % 2 == 0
            //&& borderLines.Where(x => x.StartColumn <= column && x.EndColumn >= column && x.StartRow == x.EndRow && x.StartRow <= row).Count() % 2 == 0
            )
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
    public required int StartRow { get; init; }
    public required int EndRow { get; init; }
    public required int StartColumn { get; init; }
    public required int EndColumn { get; init; }

}