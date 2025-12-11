using AoC.Utilities;

namespace AoC_2025_Day11;

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

        Dictionary<string, List<string>> devices = LoadDevices(inputFile);

        Queue<string> paths = new Queue<string>();
        paths.Enqueue("you");
        int pathCount = 0;
        while(paths.Count>0)
        {
            string currentDevice = paths.Dequeue();
            if (currentDevice == "out")
            {
                pathCount++;
            }
            else
            {
                foreach (string nextPath in devices[currentDevice])
                {
                    paths.Enqueue(nextPath);
                }
            }
        }
        Console.WriteLine(pathCount);

    }

    private static Dictionary<string, List<string>> LoadDevices(string inputFile)
    {
        var lines = InputParser.ReadInputAsRows(inputFile);
        Dictionary<string, List<string>> devices = new Dictionary<string, List<string>>();
        foreach (string line in lines)
        {
            string[] parts1 = line.Split(": ", StringSplitOptions.TrimEntries);
            string[] parts2 = parts1[1].Split(" ", StringSplitOptions.TrimEntries);

            string deviceName = parts1[0];
            List<string> outputs = parts2.ToList();
            devices.Add(deviceName, outputs);
        }

        return devices;
    }
}
