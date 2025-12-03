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

    public Battery GetFirstHighest()
    {
        int maxJoltage = Batteries.OrderBy(b=>b.Id).Take(Batteries.Count - 1).Max(x => x.Joltage);
        return Batteries.Where(b => b.Joltage == maxJoltage).MinBy(b=>b.Id)!;
    }

    public Battery GetSecondHighest(int firstHighestId)
    {
        int maxJoltage = Batteries.Where(b => b.Id > firstHighestId).OrderBy(b => b.Id).Max(x => x.Joltage);
        return Batteries.Where(b => b.Id > firstHighestId).Where(b => b.Joltage == maxJoltage).MinBy(b => b.Id)!;
    }
}
