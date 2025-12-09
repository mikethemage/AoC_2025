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
}
