using System.Collections.Concurrent;
using System.Threading.Tasks;
using Shared.Interfaces;
using UnityEngine;

namespace Server.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerCallback _callback;

    public PlayerService(IPlayerCallback callback)
    {
        _callback = callback;
    }

    private readonly ConcurrentDictionary<string, Vector2> _playerDictionary =
        new ConcurrentDictionary<string, Vector2>();

    public ValueTask<LoginReply> LoginAsync(LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Account) || string.IsNullOrWhiteSpace(req.Password))
        {
            return new ValueTask<LoginReply>(new LoginReply() { Code = 1 });
        }

        _playerDictionary.TryAdd(req.Account, new Vector2());
        return new ValueTask<LoginReply>(new LoginReply() { Code = 0, Token = req.Account });
    }

    public ValueTask Move(MoveRequest req)
    {
        Vector2 currPos;
        _playerDictionary.TryGetValue(req.PlayerId, out currPos);
        var dir = req.Direction;
        if (dir == 1)
        {
            currPos.x += 1;
        }
        else if (dir == 2)
        {
            currPos.y -= 1;
        }
        else if (dir == 3)
        {
            currPos.x -= 1;
        }
        else
        {
            currPos.y += 1;
        }

        _playerDictionary.AddOrUpdate(req.PlayerId, currPos, (id, pos) => currPos);
        return new ValueTask();
    }
}