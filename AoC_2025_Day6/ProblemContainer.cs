namespace AoC_2025_Day6;

internal class ProblemContainer
{
    private List<Problem> _problems = new List<Problem>();

    public void AddValue(int id, long value)
    {
        while(_problems.Count <= id)
        {
            _problems.Add(new Problem());
        }
        _problems[id].AddValue(value);
    }

    public void AddOperation(int id, string operation)
    {
        while (_problems.Count <= id)
        {
            _problems.Add(new Problem());
        }
        _problems[id].SetOperation(operation);
    }

    public long GetGrandTotal()
    {
        long accumulator = 0;
        foreach (var problem in _problems)
        {
            accumulator += problem.Calculate();
        }
        return accumulator;
    }
}
