using System;
using Shared.Interfaces;
using ULinkRPC.Core;
using ULinkRPC.Server;

namespace Shared.Interfaces.Server.Generated
{
    public sealed class FirserServiceCallbackProxy : IFirserServiceCallback
    {
        private const int ServiceId = 1;
        private readonly RpcServer _server;

        public FirserServiceCallbackProxy(RpcServer server) { _server = server; }

        public void OnNotify(List<PlayerPosition> playerPositions)
        {
            _server.PushAsync<List<PlayerPosition>>(ServiceId, 1, playerPositions).AsTask().Wait();
        }

    }
}
