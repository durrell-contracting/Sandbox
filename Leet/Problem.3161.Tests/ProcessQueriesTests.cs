using System;
using System.IO;
using Xunit;

public class ProcessQueriesTests
{
    private static string[] RunAndCollectOutput(Query[] queries)
    {
        var swOut = new StringWriter();
        var swErr = new StringWriter();
        var origOut = Console.Out;
        var origErr = Console.Error;

        try
        {
            Console.SetOut(swOut);
            Console.SetError(swErr);

            Program.ProcessQueries(queries);

            swOut.Flush();
            swErr.Flush();

            var combined = swOut.ToString();
            // split on both \r\n and \n to normalize
            var lines = combined.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }
        finally
        {
            Console.SetOut(origOut);
            Console.SetError(origErr);
        }
    }

    [Fact]
    public void NoObstacles_BlockFits_ReturnsTrue()
    {
        var queries = new[]
        {
            new Query { Type = 2, X = 10, Size = 5 }
        };

        var lines = RunAndCollectOutput(queries);

        Assert.Contains("[2, 10, 5] -> true", lines);
    }

    [Fact]
    public void ObstacleBlocks_LargerBlock_ReturnsFalse()
    {
        var queries = new[]
        {
            new Query { Type = 1, X = 5 },
            new Query { Type = 2, X = 10, Size = 6 }
        };

        var lines = RunAndCollectOutput(queries);

        // Expect an entry for the add-obstacle, then the query result
        Assert.Equal("[1, 5]", lines[0]);
        Assert.Equal("[2, 10, 6] -> false", lines[1]);
    }

    [Fact]
    public void TouchingObstacle_AllowsPlacement_ReturnsTrue()
    {
        var queries = new[]
        {
            new Query { Type = 1, X = 5 },
            new Query { Type = 2, X = 10, Size = 5 }
        };

        var lines = RunAndCollectOutput(queries);

        Assert.Equal("[1, 5]", lines[0]);
        Assert.Equal("[2, 10, 5] -> true", lines[1]);
    }

    [Fact]
    public void LeetExample1()
    {
        var queries = new[]
        {
            new Query { Type = 1, X = 2 },
            new Query { Type = 2, X = 3, Size = 3 },
            new Query { Type = 2, X = 3, Size = 1 },
            new Query { Type = 2, X = 3, Size = 2 }
        };

        var lines = Program.ProcessQueries(queries);

        Assert.Equal("[1, 2]", lines[0]);
        Assert.Equal("[2, 3, 3] -> false", lines[1]);
        Assert.Equal("[2, 3, 1] -> true", lines[2]);
        Assert.Equal("[2, 3, 2] -> true", lines[3]);
    }

    [Fact]
    public void LeetExample2()
    {
        var queries = new[]
        {
            new Query { Type = 1, X = 7 },
            new Query { Type = 2, X = 7, Size = 6 },
            new Query { Type = 1, X = 2 },
            new Query { Type = 2, X = 7, Size = 5 },
            new Query { Type = 2, X = 7, Size = 6 }
        };

        var lines = Program.ProcessQueries(queries);

        Assert.Equal("[1, 7]", lines[0]);
        Assert.Equal("[2, 7, 6] -> true", lines[1]);
        Assert.Equal("[1, 2]", lines[2]);
        Assert.Equal("[2, 7, 5] -> true", lines[3]);
        Assert.Equal("[2, 7, 6] -> false", lines[4]);
    }
}