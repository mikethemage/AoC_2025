namespace AoC_2025_Day7;

internal class Beam
{
    public required int Row { get; init; }
    public required int Column { get; init; }
    public Beam MoveDown()
    {
        return new Beam { Column = Column, Row = Row + 1 };
    }
}
