using Shared.Interfaces;

namespace Server.Services;

public class MyFirstService : IMyFirstService
{
    private readonly IFirserServiceCallback _callback;

    public MyFirstService(IFirserServiceCallback callback)
    {
        _callback = callback;
    }
    
    public ValueTask<LoginReply> LoginAsync(LoginRequest req)
    {
        _callback.OnNotify(new List<PlayerPosition>());
        return new ValueTask<LoginReply>();
    }
}
