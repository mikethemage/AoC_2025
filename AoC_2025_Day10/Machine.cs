namespace AoC_2025_Day10;

internal class Machine
{
    public required List<bool> TargetLightStatus { get; init; }
    public required List<List<int>> Buttons { get; init; }
    public required List<int> JoltageRequirements { get; init; }
    public LightState PressButtonLightsMode(LightState currentLightState, int button)
    {
        return currentLightState.PressButton(button, Buttons[button]);
    }
    public JoltageState PressButtonJoltageMode(JoltageState currentJoltageState, int button)
    {
        return currentJoltageState.PressButton(button, Buttons[button]);
    }
}
