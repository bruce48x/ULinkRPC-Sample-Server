using System;
using System.Threading.Tasks;
using Shared.Interfaces;
using ULinkRPC.Core;
using ULinkRPC.Server;

namespace Shared.Interfaces.Server.Generated
{
    public static class MyFirstServiceBinder
    {
        private const int ServiceId = 1;

        public static void Bind(RpcServer server, Func<LoginRequest, ValueTask<LoginReply>> loginAsyncHandler)
        {
            Bind(server, new DelegateImpl(loginAsyncHandler));
        }

        private sealed class DelegateImpl : IMyFirstService
        {
            private readonly Func<LoginRequest, ValueTask<LoginReply>> _loginAsyncHandler;

            public DelegateImpl(Func<LoginRequest, ValueTask<LoginReply>> loginAsyncHandler)
            {
                _loginAsyncHandler = loginAsyncHandler ?? throw new ArgumentNullException(nameof(loginAsyncHandler));
            }

            public ValueTask<LoginReply> LoginAsync(LoginRequest req)
            {
                return _loginAsyncHandler(req);
            }

        }

        public static void Bind(RpcServer server, Func<IFirserServiceCallback, IMyFirstService> implFactory)
        {
            if (implFactory is null) throw new ArgumentNullException(nameof(implFactory));
            var callback = new FirserServiceCallbackProxy(server);
            var impl = implFactory(callback) ?? throw new InvalidOperationException("Service implementation factory returned null.");
            Bind(server, impl);
        }

        public static void Bind(RpcServer server, IMyFirstService impl)
        {
            server.Register(ServiceId, 1, async (req, ct) =>
            {
                var arg1 = server.Serializer.Deserialize<LoginRequest>(req.Payload.AsSpan())!;
                var resp = await impl.LoginAsync(arg1);
                return new RpcResponseEnvelope { RequestId = req.RequestId, Status = RpcStatus.Ok, Payload = server.Serializer.Serialize(resp) };
            });

        }
    }
}
