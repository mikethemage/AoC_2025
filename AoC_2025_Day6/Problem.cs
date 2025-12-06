namespace AoC_2025_Day6;

internal class Problem
{
    private List<long> _values = new List<long>();
    private string _operation = string.Empty;

    public void AddValue(long value)
    {
        _values.Add(value);
    }

    public void SetOperation(string operation)
    {
        _operation = operation;
    }

    public long Calculate()
    {
        switch(_operation)
        {
            case "+":
                   return _values.Sum();

            case "*":
                long accumulator = 1;
                foreach (var value in _values)
                {
                    accumulator *= value;
                }
                return accumulator;
            default:
                throw new Exception("Invalid Operation!");
        }
    }
}