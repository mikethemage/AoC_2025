using Microsoft.Z3;

namespace AoC_2025_Day10;

public static class Z3Minimizer
{
    public static int MinimizeTotalSum(List<List<int>> buttons, List<int> targetValues)
    {
        using var ctx = new Context(new() { { "MODEL", "true" } });
        Optimize opt = ctx.MkOptimize();

        int varCount = buttons.Count;
        int eqCount = targetValues.Count;

        // Create X[0..varCount-1]
        IntExpr[] X = new IntExpr[varCount];
        for (int j = 0; j < varCount; j++)
        {
            X[j] = ctx.MkIntConst($"X_{j}");
            // Enforce non-negative integers
            opt.Assert(ctx.MkGe(X[j], ctx.MkInt(0)));
        }

        // Build constraints for each targetValue
        for (int i = 0; i < eqCount; i++)
        {
            var involved =
                buttons
                    .Select((list, j) => new { list, j })
                    .Where(x => x.list.Contains(i))
                    .Select(x => X[x.j])
                    .ToArray();

            ArithExpr sum = involved.Length > 0
                ? (ArithExpr)ctx.MkAdd(involved)
                : ctx.MkInt(0);

            opt.Assert(ctx.MkEq(sum, ctx.MkInt(targetValues[i])));
        }

        // Objective: minimize total sum of X
        ArithExpr totalSum = (ArithExpr)ctx.MkAdd(X);
        opt.MkMinimize(totalSum);

        // Solve
        if (opt.Check() == Status.SATISFIABLE)
        {
            Model m = opt.Model;
            int sum = X.Select(xi => ((IntNum)m.Evaluate(xi)).Int).Sum();
            return sum;
        }
        else
        {
            throw new InvalidOperationException("No solution exists for the given constraints.");
        }
    }
}
