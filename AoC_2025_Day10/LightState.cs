namespace AoC_2025_Day10;

internal class LightState
{
    public List<int> ButttonsPressed { get; private set; } = new List<int>();
    public List<bool> CurrentLightStatus { get; private set; }
    public LightState(List<bool> initialLightStatus)
    {
        CurrentLightStatus = new List<bool>(initialLightStatus);
    }
    public LightState PressButton(int buttonId, List<int> toggleLights)
    {
        List<bool> nextLightStatus = new List<bool>(CurrentLightStatus);
        foreach (int toggleLight in toggleLights)
        {
            nextLightStatus[toggleLight] = !nextLightStatus[toggleLight];
        }
        LightState nextLightState = new LightState(nextLightStatus);
        nextLightState.ButttonsPressed.AddRange(ButttonsPressed);
        nextLightState.ButttonsPressed.Add(buttonId);
        return nextLightState;
    }
}
