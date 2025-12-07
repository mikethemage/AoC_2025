namespace AoC_2025_Day7;

internal class Splitter
{
    public required int Row { get; init; }
    public required int Column { get; init; }
    public bool Triggered { get; private set; } = false;

    public List<Beam> Split()
    {
        Triggered = true;
        return new List<Beam> { new Beam { Column = Column - 1, Row = Row + 1 }, new Beam { Column = Column + 1, Row = Row + 1 } };
    }
}
