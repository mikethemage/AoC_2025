

namespace AoC.Utilities;

public static class InputParser
{
    public static string ReadInputAsText(string inputFile)
    {
        string output = File.ReadAllText($"{inputFile}.txt");
        return output;
    }

    public static int ReadInputAsInt(string inputFile)
    {
        return int.Parse(ReadInputAsText(inputFile).Trim());
    }

    public static List<int> ReadInputAsIntList(string inputFile)
    {
        string inputData = ReadInputAsText(inputFile);        
        return inputData.Select(x => x.ToString()).Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToList();
    }

    public static List<List<int>> ReadInputAsTabSeparatedIntRows(string inputFile)
    {
        List<string> inputData= ReadInputAsRows(inputFile);
        List<List<int>> output = new List<List<int>>();
        foreach (var row in inputData)
        {
            output.Add(ReadLineAsTabSeparatedIntList(row));
        }
        return output;
    }

    public static List<int> ReadLineAsTabSeparatedIntList(string row)
    {
        return row.Split('\t').Select(int.Parse).ToList();
    }

    public static List<string> ReadInputAsRows(string inputFile)
    {
        return File.ReadAllLines($"{inputFile}.txt").ToList();
    }
}
