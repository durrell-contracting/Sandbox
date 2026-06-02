using System.Text.Json;

public class Program
{
    // Simple CLI solver:
    // Modes:
    // 1) If a JSON filename is provided as the first command-line argument, the program reads queries from that file.
    //    The file can be a top-level array of Query objects or an object with a "queries" array.
    //    Example JSON (array):
    //      [ { "type":1, "x":5 }, { "type":2, "x":10, "size":3 } ]
    //
    // 2) If no arguments are provided, the program falls back to interactive stdin mode:
    //    q
    //    then q lines, each either:
    //     "1 x"        -> add obstacle at x
    //     "2 x sz"     -> query: can place block of size sz anywhere in [0, x]
    //
    // Output:
    //  For each type-2 query prints "true" or "false" on its own line.
    static void Main(string[] args)
    {
        if (args.Length <= 0)
        {
            Console.Error.WriteLine("No input file provided");
            return;
        }

        string json = ReadInput(args[0]);
        if (string.IsNullOrEmpty(json))
        {
            Console.Error.WriteLine($"Failed to read JSON File '{args[0]}'");
            return;
        }

        Query[] queries = ParseQueries(json);

        ProcessQueries(queries);
    }

    private static string ReadInput(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        try
        {
            return File.ReadAllText(path);
        }
        catch
        {
            return string.Empty;
        }

    }

    private static Query[] ParseQueries(string json)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Query[]>(json, options) ?? throw new Exception($"Failed to parse '{json}' as Query[]");
        }
        catch
        {
            return new Query[0];
        }
    }

    public static List<string> ProcessQueries(Query[] queries)
    {
        var messages = new List<string>();
        var obstacles = new SortedSet<long>();

        foreach (var query in queries)
        {
            if (query == null) continue;
            if (query.Type == 1)
            {
                obstacles.Add(query.X);
                messages.Add(query.ToString());
                Console.WriteLine(query);
            }
            else if (query.Type == 2)
            {
                bool possible = CanPlaceBlock(obstacles, query);
                string message = $"{query} -> {(possible ? "true" : "false")}";
                messages.Add(message);
                Console.WriteLine(message);
            }
            else
            {
                // ignore other types
                string message = $"{query} -> Unknown query type";
                messages.Add(message);
                Console.WriteLine(message);
            }
        }
        return messages;
    }

    // Check if there exists an interval [L, L+sz] subset of [0, x] such that no obstacle lies strictly inside (L, L+sz).
    // Touching obstacles at endpoints is allowed.
    static bool CanPlaceBlock(SortedSet<long> obstacles, Query query)
    {
        if (query.Size <= 0) return true; // zero or negative size trivially fits

        long prev = 0;
        // iterate obstacles in [0, x]
        var view = obstacles.GetViewBetween(0L, query.X);
        if (view.Count == 0)
            return query.X >= query.Size;

        foreach (var p in view)
        {
            if (p - prev >= query.Size) return true;
            prev = p;
        }

        // gap from last obstacle (<= x) to x
        return query.X - prev >= query.Size;
    }
}

public class Query
{
    public int Type { get; set; }
    public long X { get; set; }
    public long Size { get; set; } // only for type 2

    public override string ToString()
    {
        if (Type == 1)
            return $"[{Type}, {X}]";
        return $"[{Type}, {X}, {Size}]";
    }
}
