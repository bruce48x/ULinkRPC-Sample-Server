using ULinkRPC.Runtime;
using Shared.Interfaces;

namespace Server.Services;

public class MyFirstService : IMyFirstService
{
    // `UnaryResult<T>` allows the method to be treated as `async` method.
    public ValueTask<int> SumAsync(int x, int y)
    {
        Console.WriteLine($"Received:{x}, {y}");
        return new ValueTask<int>(x + y);
    }
}