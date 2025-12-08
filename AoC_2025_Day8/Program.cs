using AoC.Utilities;

namespace AoC_2025_Day8;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("filename is required!");
            throw new Exception("No file provided!");
        }
        if (args.Length < 2)
        {
            Console.WriteLine("max iterations is required!");
            throw new Exception("max iterations not provided!");
        }
        int maxIterations = int.Parse(args[1]);

        SolvePart(args[0], 1, maxIterations);

        SolvePart(args[0], 2, 0);
    }

    private static void SolvePart(string inputFile, int partNumber, int maxIterations)
    {
        Console.WriteLine($"Part {partNumber}:");
        if (partNumber != 1 && partNumber != 2)
        {
            throw new Exception("Invalid part number!");
        }

        List<JunctionBox> junctionBoxes = LoadJunctionBoxes(inputFile);
        List<List<JunctionBox>> circuits = junctionBoxes.Select(x => new List<JunctionBox> { x }).ToList();
        List<DistanceMeasurement> distanceMeasurements = new List<DistanceMeasurement>();
        for (int i = 0; i < junctionBoxes.Count - 1; i++)
        {
            for (int j = i + 1; j < junctionBoxes.Count; j++)
            {
                distanceMeasurements.Add(new DistanceMeasurement
                {
                    JunctionBox1 = junctionBoxes[i],
                    JunctionBox2 = junctionBoxes[j],
                    Distance = DistanceMeasurer.GetDistanceSquared(junctionBoxes[i], junctionBoxes[j])
                });
            }
        }
        distanceMeasurements = distanceMeasurements.OrderBy(x => x.Distance).ToList();

        int lastX = 0;
        int secondLastX = 0;
        int connectionCounter = 0;
        foreach (DistanceMeasurement distanceMeasurement in distanceMeasurements)
        {
            if (NotInSameCircuit(distanceMeasurement.JunctionBox1, distanceMeasurement.JunctionBox2, circuits))
            {
                MergeCircuits(distanceMeasurement.JunctionBox1, distanceMeasurement.JunctionBox2, circuits);
                secondLastX = distanceMeasurement.JunctionBox2.X;
                lastX = distanceMeasurement.JunctionBox1.X;
            }
            connectionCounter++;
            if (partNumber == 1 && connectionCounter >= maxIterations)
            {
                break;
            }
        }
        if (partNumber == 1)
        {
            List<int> topThree = circuits.OrderByDescending(x => x.Count).Select(x => x.Count).Take(3).ToList();
            if (topThree.Count < 3)
            {
                throw new Exception("Less than 3 results!");
            }
            int accumulator = 1;
            foreach (int item in topThree)
            {
                accumulator *= item;
            }
            Console.WriteLine($"Result: {accumulator}");
        }
        else
        {
            long result = (long)secondLastX * (long)lastX;
            Console.WriteLine($"Result: {result}");
        }

    }

    private static void MergeCircuits(JunctionBox junctionBox1, JunctionBox junctionBox2, List<List<JunctionBox>> circuits)
    {
        List<JunctionBox> newCircuit = new List<JunctionBox>();
        List<JunctionBox>? circuit1 = circuits.Where(x => x.Contains(junctionBox1)).FirstOrDefault();
        if (circuit1 is not null)
        {
            newCircuit.AddRange(circuit1);
            circuits.Remove(circuit1);
        }
        List<JunctionBox>? circuit2 = circuits.Where(x => x.Contains(junctionBox2)).FirstOrDefault();
        if (circuit2 is not null)
        {
            newCircuit.AddRange(circuit2);
            circuits.Remove(circuit2);
        }
        if (newCircuit.Count > 0)
        {
            circuits.Add(newCircuit);
        }
    }

    private static bool NotInSameCircuit(JunctionBox junctionBox1, JunctionBox junctionBox2, List<List<JunctionBox>> circuits)
    {
        List<JunctionBox>? circuit1 = circuits.Where(x => x.Contains(junctionBox1)).FirstOrDefault();
        List<JunctionBox>? circuit2 = circuits.Where(x => x.Contains(junctionBox2)).FirstOrDefault();
        if (circuit1 is null || circuit2 is null)
        {
            throw new Exception("Not in any circuits!!!");
        }
        return circuit1 != circuit2;
    }

    private static List<JunctionBox> LoadJunctionBoxes(string inputFile)
    {
        List<JunctionBox> output = new List<JunctionBox>();
        List<string> lines = InputParser.ReadInputAsRows(inputFile);
        int id = 0;
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                List<int> values = line.Split(",", StringSplitOptions.TrimEntries).Select(x => int.Parse(x)).ToList();
                output.Add(new JunctionBox { Id = id, X = values[0], Y = values[1], Z = values[2] });
                id++;
            }
        }
        return output;
    }
}
