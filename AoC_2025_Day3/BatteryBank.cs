namespace AoC_2025_Day3;

internal class BatteryBank
{
    public List<Battery> Batteries { get; private set; } = new List<Battery>();

    public BatteryBank(string inputRow)
    {
        foreach (char item in inputRow)
        {
            Batteries.Add(new Battery { Joltage = int.Parse(item.ToString()) });
        }
    }
}
