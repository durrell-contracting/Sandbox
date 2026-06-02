using System;
using Xunit;
using Problem._3093;

namespace Problem._3093.Tests;

public class SuffixComparerTests
{
    [Fact]
    public void Constructor_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new SuffixComparer(null));
    }

    [Fact]
    public void LongestCommonSuffixIndices_NullQueryArray_ThrowsArgumentNullException()
    {
        var sc = new SuffixComparer(Array.Empty<string>());
        Assert.Throws<ArgumentNullException>(() => sc.LongestCommonSuffixIndices(null));
    }

    [Fact]
    public void SimpleExample_ReturnsExpectedIndices()
    {
        var container = new[] { "tape", "cape", "ape", "apple" };
        var queries = new[] { "grape", "escape", "tape" };

        var sc = new SuffixComparer(container);
        var result = sc.LongestCommonSuffixIndices(queries);

        Assert.Equal([2, 1, 0], result);
    }

    [Fact]
    public void TieOnSuffix_PrefersShorterContainer()
    {
        var container = new[] { "ab", "b", "cab" };
        var queries = new[] { "b" };

        var sc = new SuffixComparer(container);
        var result = sc.LongestCommonSuffixIndices(queries);

        Assert.Equal([1], result);
    }

    [Fact]
    public void TieOnSuffixAndLength_PrefersEarlierIndex()
    {
        var container = new[] { "xb", "yb" };
        var queries = new[] { "b" };

        var sc = new SuffixComparer(container);
        var result = sc.LongestCommonSuffixIndices(queries);

        Assert.Equal([0], result);
    }

    [Fact]
    public void EmptyContainer_ReturnsMinusOneForAllQueries()
    {
        var sc = new SuffixComparer(Array.Empty<string>());
        var queries = new[] { "abc", "" };

        var result = sc.LongestCommonSuffixIndices(queries);

        Assert.Equal([-1, -1], result);
    }
}