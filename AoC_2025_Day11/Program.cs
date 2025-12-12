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
            var memo = new Dictionary<(string, bool, bool), long>();
            long paths = CountPathsMemo("svr", false, false, devices, memo);

            Console.WriteLine(paths);

            //Dictionary<(string currentNode, bool visitedDac, bool visitedFft), MemoryTracker> memory = new Dictionary<(string currentNode, bool visitedDac, bool visitedFft), MemoryTracker>();
            //memory.Add(("svr", false, false), new MemoryTracker { Id= ("svr", false, false), CurrentValue = 1});
            //Queue<PathInfo> paths = new Queue<PathInfo>();
            //paths.Enqueue(new PathInfo { CurrentNode = "svr", VisitedDac = false, VisitedFft = false });
            //while (paths.Count>0)
            //{
            //    PathInfo currentDevice = paths.Dequeue();

            //    if (devices.ContainsKey(currentDevice.CurrentNode))
            //    {
            //        List<string> nextDevices = devices[currentDevice.CurrentNode];
            //        foreach (PathInfo nextDevice in nextDevices.Select(x=>new PathInfo { CurrentNode=x, VisitedDac=currentDevice.VisitedDac, VisitedFft=currentDevice.VisitedFft}).ToList())
            //        {
            //            if (nextDevice.CurrentNode == "dac")
            //            {
            //                nextDevice.VisitedDac = true;
            //            }
            //            if (nextDevice.CurrentNode == "fft")
            //            {
            //                nextDevice.VisitedFft = true;
            //            }
            //            if (memory.ContainsKey((nextDevice.CurrentNode, nextDevice.VisitedDac, nextDevice.VisitedFft)))
            //            {
            //                MemoryTracker existingMemory = memory[(nextDevice.CurrentNode, nextDevice.VisitedDac, nextDevice.VisitedFft)];
            //                if(existingMemory.Unresolved.ContainsKey((currentDevice.CurrentNode, currentDevice.VisitedDac, currentDevice.VisitedFft)))
            //                {
            //                    existingMemory.Unresolved[(currentDevice.CurrentNode, currentDevice.VisitedDac, currentDevice.VisitedFft)]++;
            //                }
            //                else
            //                {
            //                    existingMemory.Unresolved.Add((currentDevice.CurrentNode, currentDevice.VisitedDac, currentDevice.VisitedFft), 1);
            //                }
            //            }
            //            else
            //            {
            //                MemoryTracker newMemory = new MemoryTracker { Id= (nextDevice.CurrentNode, nextDevice.VisitedDac, nextDevice.VisitedFft), CurrentValue = memory[(currentDevice.CurrentNode, currentDevice.VisitedDac, currentDevice.VisitedFft)].CurrentValue };
            //                newMemory.Unresolved.Add((currentDevice.CurrentNode, currentDevice.VisitedDac, currentDevice.VisitedFft), 1);
            //                memory.Add((nextDevice.CurrentNode, nextDevice.VisitedDac, nextDevice.VisitedFft), newMemory);
            //                paths.Enqueue(nextDevice);
            //            }
            //        }
            //    }                
            //}

            //while(memory.Values.Any(x=>x.Unresolved.Count>0))
            //{
            //    List<MemoryTracker> flowCandidates = memory.Values.Where(x => x.Unresolved.Count == 0).ToList();
            //    foreach (MemoryTracker flowCandidate in flowCandidates)
            //    {
            //        List<MemoryTracker> flowTos = memory.Values.Where(x => x.Unresolved.ContainsKey(flowCandidate.Id)).ToList();
            //        foreach (MemoryTracker flowTo in flowTos)
            //        {
            //            long newValue = flowTo.Unresolved[flowCandidate.Id] * flowCandidate.CurrentValue;
            //            flowTo.CurrentValue += newValue;
            //            flowTo.Unresolved.Remove(flowCandidate.Id);
            //        }
            //    }
            //}
            //Console.WriteLine(memory[("out", true, true)].CurrentValue);

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

    static long CountPathsMemo(
        string node,
        bool visitedDac,
        bool visitedFft,
        Dictionary<string, List<string>> devices,
        Dictionary<(string, bool, bool), long> memo)
    {
        var key = (node, visitedDac, visitedFft);
        if (memo.TryGetValue(key, out long cached)) return cached;

        // update visited flags for the current node
        if (node == "dac") visitedDac = true;
        if (node == "fft") visitedFft = true;

        // base case: reached "out"
        if (node == "out")
        {
            long result = (visitedDac && visitedFft) ? 1L : 0L;
            memo[key] = result;
            return result;
        }

        long total = 0L;
        if (devices.TryGetValue(node, out List<string>? neighbors))
        {
            foreach (string nxt in neighbors)
            {
                total += CountPathsMemo(nxt, visitedDac, visitedFft, devices, memo);
            }
        }

        memo[key] = total;
        return total;
    }
}

internal class PathInfo
{
    public required string CurrentNode { get; set; }
    public bool VisitedDac { get; set; } = false;
    public bool VisitedFft { get; set; } = false;

}

internal class MemoryTracker
{
    public required (string currentNode, bool visitedDac, bool visitedFft) Id { get; init; }
    public Dictionary<(string currentNode, bool visitedDac, bool visitedFft), long> Unresolved = new Dictionary<(string currentNode, bool visitedDac, bool visitedFft), long>();
    public long CurrentValue { get; set; } = 0;
}