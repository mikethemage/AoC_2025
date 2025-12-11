using AoC.Utilities;

namespace AoC_2025_Day11;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("filename is required!");
            throw new Exception("No file provided!");
        }

        SolvePart(args[0], 1);

        SolvePart(args[1], 2);
    }

    private static void SolvePart(string inputFile, int partNumber)
    {
        Console.WriteLine($"Part {partNumber}:");
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        Dictionary<string, List<string>> devices = LoadDevices(inputFile);

        if(partNumber==1)
        {
            Queue<string> paths = new Queue<string>();
            paths.Enqueue("you");
            int pathCount = 0;
            while (paths.Count > 0)
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
        else
        {
            Dictionary<(string currentNode, bool visitedDac, bool visitedFft), int> memory = new Dictionary<(string currentNode, bool visitedDac, bool visitedFft), int>();
            memory.Add(("svr", false, false), 1);
            Stack<PathInfo> paths = new Stack<PathInfo>();
            paths.Push(new PathInfo { CurrentNode = "svr", VisitedDac = false, VisitedFft = false });
            while (paths.Count>0)
            {
                PathInfo currentDevice = paths.Pop();

                if (devices.ContainsKey(currentDevice.CurrentNode))
                {
                    List<string> nextDevices = devices[currentDevice.CurrentNode];
                    foreach (var nextDevice in nextDevices.Select(x=>new PathInfo { CurrentNode=x, VisitedDac=currentDevice.VisitedDac, VisitedFft=currentDevice.VisitedFft}).ToList())
                    {
                        if (nextDevice.CurrentNode == "dac")
                        {
                            nextDevice.VisitedDac = true;
                        }
                        if (nextDevice.CurrentNode == "fft")
                        {
                            nextDevice.VisitedFft = true;
                        }
                        if (memory.ContainsKey((nextDevice.CurrentNode, nextDevice.VisitedDac, nextDevice.VisitedFft)))
                        {
                            memory[(nextDevice.CurrentNode, nextDevice.VisitedDac, nextDevice.VisitedFft)] += memory[(currentDevice.CurrentNode, currentDevice.VisitedDac, currentDevice.VisitedFft)];
                        }
                        else
                        {
                            memory.Add((nextDevice.CurrentNode, nextDevice.VisitedDac, nextDevice.VisitedFft), memory[(currentDevice.CurrentNode, currentDevice.VisitedDac, currentDevice.VisitedFft)]);
                            paths.Push(nextDevice);
                        }
                    }
                }                
            }

            Console.WriteLine(memory[("out", true, true)]);           
        }
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

internal class PathInfo
{
    public required string CurrentNode { get; set; }
    public bool VisitedDac { get; set; } = false;
    public bool VisitedFft { get; set; } = false;

}

