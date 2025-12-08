namespace AoC_2025_Day8;

internal static class DistanceMeasurer
{
    public static long GetDistanceSquared(JunctionBox junctionBox1, JunctionBox junctionBox2)
    {
        long distance = DimensionComparer(junctionBox1.X, junctionBox2.X);
        distance += DimensionComparer(junctionBox1.Y, junctionBox2.Y);
        distance += DimensionComparer(junctionBox1.Z, junctionBox2.Z);
        return distance;
    }

    private static long DimensionComparer(int value1, int value2)
    {
        long output = (long)value1 - (long)value2;
        output *= output;
        return output;
    }
}