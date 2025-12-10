namespace AoC_2025_Day10;

internal class JoltageState
{
    public List<int> ButttonsPressed { get; private set; } = new List<int>();
    public List<int> CurrentJoltageStatus { get; private set; }
    public JoltageState(List<int> initialJoltageStatus)
    {
        CurrentJoltageStatus = new List<int>(initialJoltageStatus);
    }
    public JoltageState PressButton(int buttonId, List<int> increaseJoltages)
    {
        List<int> nextJoltageStatus = new List<int>(CurrentJoltageStatus);
        foreach (int increaseJoltage in increaseJoltages)
        {
            nextJoltageStatus[increaseJoltage] = CurrentJoltageStatus[increaseJoltage] + 1;
        }
        JoltageState nextJoltageState = new JoltageState(nextJoltageStatus);
        nextJoltageState.ButttonsPressed.AddRange(ButttonsPressed);
        nextJoltageState.ButttonsPressed.Add(buttonId);
        return nextJoltageState;
    }
}