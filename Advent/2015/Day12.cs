using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day12
{

    JsonElement Json;
    public static bool IgnoringRed = false;
    

    public Day12()
    {
        using (StreamReader sr = File.OpenText("2015/input12"))
        {
            string jsonString = sr.ReadToEnd();
            Json = JsonSerializer.Deserialize<JsonElement>(jsonString);
        }
    }

    public void Solve()
    {
        Console.WriteLine(SumNumbers(Json));
        IgnoringRed = true;
        Console.WriteLine(SumNumbers(Json));
    }

    public static int SumNumbers(JsonElement json)
    {
        switch (json.ValueKind)
        {
            case JsonValueKind.Number:
                return json.GetInt32();
            case JsonValueKind.Array:
                return json.EnumerateArray().Sum(SumNumbers);
            case JsonValueKind.Object:
                // Check for "red" properties
                if (IgnoringRed && HasRed(json)) return 0;
                return json.EnumerateObject().Sum(e => SumNumbers(e.Value));
            case JsonValueKind.String:
                return 0;
            default:
                throw new Exception($"Unknown ValueKind {json.ValueKind}");
        }
    }

    public static bool HasRed(JsonElement json)
    {
        foreach(var prop in json.EnumerateObject())
            if(prop.Value.ValueKind == JsonValueKind.String && prop.Value.GetString() == "red") 
                return true;

        return false;
    }
}
