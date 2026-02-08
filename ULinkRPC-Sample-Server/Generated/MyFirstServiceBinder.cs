using System;
using Shared.Interfaces;
using ULinkRPC.Runtime;

namespace Shared.Interfaces.Server.Generated
{
    public static class MyFirstServiceBinder
    {
        private const int ServiceId = 1;

        public static void Bind(RpcServer server, IMyFirstService impl)
        {
            server.Register(ServiceId, 1, async (req, ct) =>
            {
                var arg = server.Serializer.Deserialize<int>(req.Payload.AsSpan())!;
                var resp = await impl.SumAsync(arg);
                return new RpcResponseEnvelope { RequestId = req.RequestId, Status = RpcStatus.Ok, Payload = server.Serializer.Serialize(resp) };
            });

        }
    }
}
