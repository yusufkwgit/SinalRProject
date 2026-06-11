using System;
using System.Text.Json;
class Program {
    static void Main() {
        var obj = new { OrderDetailID = 5, ProductName = "A", Count = 2 };
        var opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        Console.WriteLine(JsonSerializer.Serialize(obj, opts));
    }
}
