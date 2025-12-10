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

        //SolvePart(args[0], 2);
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
            int buttonPressesRequired = GetMinimumButtonPresses(machine, partNumber!=1);
            Console.WriteLine($"Machine {machineId}: Minimum button presses: {buttonPressesRequired}");
            totalButtonPresses += buttonPressesRequired;
            machineId++;
        }
        Console.WriteLine($"Total button presses: {totalButtonPresses}");
    }

    private static int GetMinimumButtonPresses(Machine machine, bool useJoltages)
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
            if (TargetStatusReached(currentState.CurrentLightStatus, machine.TargetLightStatus))
            {
                return currentState.ButttonsPressed.Count;
            }
            for (int i = 0; i < machine.Buttons.Count; i++)
            {
                if (currentState.ButttonsPressed.Count < 1 || currentState.ButttonsPressed.Last() != i)
                {
                    int priority = 1;
                    if (useJoltages)
                    {
                        priority = machine.JoltageRequirements[i];
                    }

                    lightStates.Enqueue(machine.PressButton(currentState, i));
                }
            }
        }
        return -1;
    }

    private static bool TargetStatusReached(List<bool> currentLightStatus, List<bool> targetLightStatus)
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
    public LightState PressButton(LightState currentLightState, int button)
    {
        return currentLightState.Pressbutton(button, Buttons[button]);
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
    public LightState Pressbutton(int buttonId, List<int> toggleLights)
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