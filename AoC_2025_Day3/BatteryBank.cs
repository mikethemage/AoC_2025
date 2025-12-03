namespace AoC_2025_Day3;

internal class BatteryBank
{
    public List<Battery> Batteries { get; private set; } = new List<Battery>();

    public BatteryBank(string inputRow)
    {
        for (int i = 0; i < inputRow.Length; i++)
        {
            Batteries.Add(new Battery { Id = i, Joltage = int.Parse(inputRow.Substring(i, 1)) });
        }
    }

    public Battery GetFirstHighest(int remaining)
    {
        int maxJoltage = Batteries.OrderBy(b=>b.Id).Take(Batteries.Count - remaining).Max(x => x.Joltage);
        return Batteries.Where(b => b.Joltage == maxJoltage).MinBy(b=>b.Id)!;
    }

    public Battery GetSecondHighest(int previousHighestId, int remaining)
    {
        int maxJoltage = Batteries.OrderBy(b => b.Id).Take(Batteries.Count - remaining).Where(b => b.Id > previousHighestId).Max(x => x.Joltage);
        return Batteries.Where(b => b.Id > previousHighestId).Where(b => b.Joltage == maxJoltage).MinBy(b => b.Id)!;
    }
}
