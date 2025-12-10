namespace AoC_2025_Day9;

internal class Rectangle
{
    public required Tile Tile1 { get; init; }
    public required Tile Tile2 { get; init; }

    private long _cachedArea = -1;
    public long Area
    {
        get
        {
            if (_cachedArea < 0)
            {
                _cachedArea = (long)(Math.Abs(Tile1.Row - Tile2.Row) + 1) * (long)(Math.Abs(Tile1.Column - Tile2.Column) + 1);
            }
            return _cachedArea;
        }
    }

    private List<Line>? _perimeter = null;
    public List<Line> GetPerimeter()
    {
        if (_perimeter is null)
        {

            _perimeter = new List<Line>();

            _perimeter.Add(new Line { StartRow = Math.Min(Tile1.Row, Tile2.Row), EndRow = Math.Max(Tile1.Row, Tile2.Row), StartColumn = Tile1.Column, EndColumn = Tile1.Column });
            _perimeter.Add(new Line { StartRow = Math.Min(Tile1.Row, Tile2.Row), EndRow = Math.Max(Tile1.Row, Tile2.Row), StartColumn = Tile2.Column, EndColumn = Tile2.Column });
            _perimeter.Add(new Line { StartColumn = Math.Min(Tile1.Column, Tile2.Column), EndColumn = Math.Max(Tile1.Column, Tile2.Column), StartRow = Tile1.Row, EndRow = Tile1.Row });
            _perimeter.Add(new Line { StartColumn = Math.Min(Tile1.Column, Tile2.Column), EndColumn = Math.Max(Tile1.Column, Tile2.Column), StartRow = Tile2.Row, EndRow = Tile2.Row });
        }
        return _perimeter;
    }
}
