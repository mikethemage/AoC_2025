using AoC.Utilities;

namespace AoC_2025_Day10;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("filename is required!");
            throw new Exception("No file provided!");
        }

        SolvePart(args[0], 1);

        SolvePart(args[0], 2);
    }

    private static void SolvePart(string inputFile, int partNumber)
    {
        Console.WriteLine($"Part {partNumber}:");
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        List<Machine> machines = LoadMachines(inputFile);

        int machineId = 1;
        int totalButtonPresses = 0;
        foreach (Machine machine in machines)
        {
            int buttonPressesRequired;
            if (partNumber == 1)
            { buttonPressesRequired = GetMinimumButtonPressesForLights(machine); }
            else
            {
                buttonPressesRequired = GetMinimumButtonPressesForJoltages(machine);
            }
            Console.WriteLine($"Machine {machineId}: Minimum button presses: {buttonPressesRequired}");
            totalButtonPresses += buttonPressesRequired;
            machineId++;
        }
        Console.WriteLine($"Total button presses: {totalButtonPresses}");
        Console.WriteLine();
    }

    private static int GetMinimumButtonPressesForLights(Machine machine)
    {
        List<bool> initialLightStatus = new List<bool>();
        foreach (bool light in machine.TargetLightStatus)
        {
            initialLightStatus.Add(false);
        }
        LightState initialLightState = new LightState(initialLightStatus);
        Queue<LightState> lightStates = new Queue<LightState>();
        lightStates.Enqueue(initialLightState);

        while (lightStates.Count > 0)
        {
            LightState currentState = lightStates.Dequeue();
            if (TargetLightsStatusReached(currentState.CurrentLightStatus, machine.TargetLightStatus))
            {
                return currentState.ButttonsPressed.Count;
            }
            for (int i = 0; i < machine.Buttons.Count; i++)
            {
                if (currentState.ButttonsPressed.Count < 1 || currentState.ButttonsPressed.Last() != i)
                {
                    lightStates.Enqueue(machine.PressButtonLightsMode(currentState, i));
                }
            }
        }
        return -1;
    }

    private static int GetMinimumButtonPressesForJoltages(Machine machine)
    {
        List<int> initialJoltageStatus = new List<int>();
        foreach (int joltage in machine.JoltageRequirements)
        {
            initialJoltageStatus.Add(0);
        }
        JoltageState initialJoltageState = new JoltageState(initialJoltageStatus);
        Queue<JoltageState> joltageStates = new Queue<JoltageState>();
        joltageStates.Enqueue(initialJoltageState);

        while (joltageStates.Count > 0)
        {
            JoltageState currentState = joltageStates.Dequeue();
            int goalReached = TargetJoltagesStatusReached(currentState.CurrentJoltageStatus, machine.JoltageRequirements);
            if (goalReached == 0)
            {
                return currentState.ButttonsPressed.Count;
            }
            if (goalReached == 1)
            {
                continue;
            }
            for (int i = 0; i < machine.Buttons.Count; i++)
            {
                if (currentState.ButttonsPressed.Count < 1 || currentState.ButttonsPressed.Last() != i)
                {
                    joltageStates.Enqueue(machine.PressButtonJoltageMode(currentState, i));
                }
            }
        }
        return -1;
    }

    private static bool TargetLightsStatusReached(List<bool> currentLightStatus, List<bool> targetLightStatus)
    {
        for (int i = 0; i < targetLightStatus.Count; i++)
        {
            if (currentLightStatus[i] != targetLightStatus[i])
            {
                return false;
            }
        }
        return true;
    }

    private static int TargetJoltagesStatusReached(List<int> currentJoltagesStatus, List<int> targetJoltagesStatus)
    {
        for (int i = 0; i < targetJoltagesStatus.Count; i++)
        {
            if (currentJoltagesStatus[i] > targetJoltagesStatus[i])
            {
                return 1;
            }
        }
        for (int i = 0; i < targetJoltagesStatus.Count; i++)
        {
            if (currentJoltagesStatus[i] < targetJoltagesStatus[i])
            {
                return -1;
            }
        }
        return 0;
    }

    private static List<Machine> LoadMachines(string inputFile)
    {
        List<Machine> machines = new List<Machine>();
        List<string> lines = InputParser.ReadInputAsRows(inputFile);
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                machines.Add(LoadMachine(line));
            }
        }

        return machines;
    }

    private static Machine LoadMachine(string line)
    {
        string[] parts1 = line.Split("] ", StringSplitOptions.TrimEntries);
        string[] parts2 = parts1[1].Split(" {", StringSplitOptions.TrimEntries);

        string targetLightString = parts1[0].Trim().TrimStart('[');
        string buttonsString = parts2[0].Trim();
        string joltageString = parts2[1].Trim().TrimEnd('}');

        List<bool> targetLightStates = targetLightString.Select(x => x == '#').ToList();
        List<List<int>> buttons = buttonsString.Split(' ', StringSplitOptions.TrimEntries)
                                                .Select(x => x.TrimStart('(')
                                                                .TrimEnd(')')
                                                                .Split(',', StringSplitOptions.TrimEntries)
                                                                .Select(y => int.Parse(y))
                                                                .ToList())
                                                .ToList();
        List<int> joltages = joltageString.Split(',', StringSplitOptions.TrimEntries).Select(x => int.Parse(x)).ToList();

        Machine output = new Machine { Buttons = buttons, JoltageRequirements = joltages, TargetLightStatus = targetLightStates };
        return output;
    }
}

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