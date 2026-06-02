using System;
using System.Linq;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Console.WriteLine("Minimum Cost of Buying Candies With Discount - Examples");

// Example runs
int[] example1 = { 1, 2, 3, 4 };
int[] example2 = { 2, 2, 3, 5 };
int[] example3 = { 6, 5, 6 };

Console.WriteLine($"[{string.Join(',', example1)}] -> {MinimumCostToBuyAllCandies(example1)}"); // 8
Console.WriteLine($"[{string.Join(',', example2)}] -> {MinimumCostToBuyAllCandies(example2)}"); // 10
Console.WriteLine($"[{string.Join(',', example3)}] -> {MinimumCostToBuyAllCandies(example3)}"); // 11

static int MinimumCostToBuyAllCandies(int[] cost)
{
    // Sort descending and skip every 3rd candy (index 2, 5, 8, ...)
    // Because when you buy two most expensive remaining candies, you can take
    // any candy with cost <= min(of those two) as free — choosing the next-most-expensive
    // remaining candy satisfies that and minimizes total.
    var sorted = cost.OrderByDescending(c => c).ToArray();
    int result = 0;
    for (int i = 0; i < sorted.Length; i++)
    {
        // every third candy (0-based index 2,5,8,...) is free
        if (i % 3 == 2) continue;
        result += sorted[i];
    }
    return result;
}