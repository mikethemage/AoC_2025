namespace AoC_2025_Day4;

internal class Grid
{
    private Dictionary<(int Row, int Column), Roll> _map = new Dictionary<(int Row, int Column), Roll>();

    public int MinRow { get; private set; } = 0;
    public int MaxRow { get; private set; } = 0;
    public int MinColumn { get; private set; } = 0;
    public int MaxColumn { get; private set; } = 0;

    public void AddRoll(int row, int column)
    {
        _map.Add((row, column), new Roll { Row = row, Column = column });
        if(row<MinRow)
        {
            MinRow = row;
        }
        if(row>MaxRow)
        {
            MaxRow = row;
        }
        if(column<MinColumn)
        {
            MinColumn = column;
        }
        if(column>MaxColumn)
        {
            MaxColumn = column;
        }
    }

    public char GetGridLocationSymbol(int row, int column)
    {
        if(_map.TryGetValue((row, column), out Roll? roll))
        {
            if(roll.IsAccessible)
            {
                return 'x';
            }
            else
            {
                return '@';
            }
        }
        else
        {
            return '.';
        }
    }

    public void UpdateRollAccessibility()
    {
        foreach (Roll roll in _map.Values)
        {
            int neighboutCount = 0;
            for(int j=-1; j<=1; j++)
            {
                for (int i=-1; i<=1; i++)
                {
                    if(j!=0 || i!=0)
                    {
                        _map.TryGetValue((roll.Row + j, roll.Column + i), out Roll? neighbour);
                        if(neighbour is not null)
                        {
                            neighboutCount++;
                        }
                    }
                }
            }
            if(neighboutCount<4)
            {
                roll.IsAccessible = true;
            }
        }
    }

    public int GetAccessibleRollCount()
    {
        return _map.Values.Where(x => x.IsAccessible).Count();
    }
}