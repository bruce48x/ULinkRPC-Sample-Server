using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryPack;
using ULinkRPC.Core;
using UnityEngine;

namespace Shared.Interfaces
{
    [RpcService(1)]
    public interface IPlayerService : IRpcService<IPlayerService, IPlayerServiceCallback>
    {
        [RpcMethod(1)]
        ValueTask<LoginReply> LoginAsync(LoginRequest req);
        
        [RpcMethod(2)]
        ValueTask Move(MoveRequest req);
    }

    public interface IPlayerServiceCallback
    {
        [RpcMethod(1)]
        void OnMove(List<PlayerPosition> playerPositions);
    }

    [MemoryPackable]
    public partial class LoginRequest
    {
        public string Account { get; set; } = "";
        public string Password { get; set; } = "";
    }

    [MemoryPackable]
    public partial class LoginReply
    {
        public int Code { get; set; }
        public string Token { get; set; } = "";
    }

    [MemoryPackable]
    public partial class PlayerPosition
    {
        public string PlayerId { get; set; } = "";
        public Vector2 Position { get; set; }
    }
    
    
    [MemoryPackable]
    public partial class MoveRequest
    {
        public string PlayerId { get; set; } = "";
        public int Direction { get; set; }
    }
}