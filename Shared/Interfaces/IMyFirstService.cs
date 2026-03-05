using System.Collections.Generic;
using System.Threading.Tasks;
using ULinkRPC.Core;
using MemoryPack;
using UnityEngine;

namespace Shared.Interfaces
{
    [RpcService(1)]
    public interface IMyFirstService: IRpcService<IMyFirstService, IFirserServiceCallback>
    {
        [RpcMethod(1)]
        ValueTask<LoginReply> LoginAsync(LoginRequest req);
    }
    
    public interface IFirserServiceCallback
    {
        [RpcMethod(1)]
        void OnNotify(List<PlayerPosition> playerPositions);
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
        string playerId;
        Vector3 position;
    }
}
