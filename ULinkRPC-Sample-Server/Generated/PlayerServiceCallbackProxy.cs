using System;
using Shared.Interfaces;
using ULinkRPC.Core;
using ULinkRPC.Server;

namespace Shared.Interfaces.Server.Generated
{
    public sealed class PlayerServiceCallbackProxy : IPlayerServiceCallback
    {
        private const int ServiceId = 1;
        private readonly RpcServer _server;

        public PlayerServiceCallbackProxy(RpcServer server) { _server = server; }

        public void OnMove(List<PlayerPosition> playerPositions)
        {
            _server.PushAsync<List<PlayerPosition>>(ServiceId, 1, playerPositions).AsTask().Wait();
        }

    }
}
