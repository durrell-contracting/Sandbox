// See https://aka.ms/new-console-template for more information
using Problem._3093;
using System;

string[] wordsContainer = new[] { "tape", "cape", "ape", "apple" };
string[] wordsQuery = new[] { "grape", "escape", "tape" };

int[] ans = LongestCommonSuffixIndices(wordsContainer, wordsQuery);
Console.WriteLine(string.Join(", ", ans)); // example output

/// <summary>
/// For each query string, find the index in wordsContainer that:
/// 1) has the longest common suffix with the query;
/// 2) if tied on suffix length, has the smallest length;
/// 3) if still tied, appeared earlier in wordsContainer.
/// </summary>
static int[] LongestCommonSuffixIndices(string[] wordsContainer, string[] wordsQuery)
{
    SuffixComparer comparer = new SuffixComparer(wordsContainer);
    return comparer.LongestCommonSuffixIndices(wordsQuery);
}
