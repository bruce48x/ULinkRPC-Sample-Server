using System;
using System.Threading.Tasks;
using Shared.Interfaces;
using ULinkRPC.Core;
using ULinkRPC.Server;

namespace Shared.Interfaces.Server.Generated
{
    public static class PlayerServiceBinder
    {
        private const int ServiceId = 1;

        public static void Bind(RpcServer server, Func<LoginRequest, ValueTask<LoginReply>> loginAsyncHandler, Func<MoveRequest, ValueTask> moveHandler)
        {
            Bind(server, new DelegateImpl(loginAsyncHandler, moveHandler));
        }

        private sealed class DelegateImpl : IPlayerService
        {
            private readonly Func<LoginRequest, ValueTask<LoginReply>> _loginAsyncHandler;
            private readonly Func<MoveRequest, ValueTask> _moveHandler;

            public DelegateImpl(Func<LoginRequest, ValueTask<LoginReply>> loginAsyncHandler, Func<MoveRequest, ValueTask> moveHandler)
            {
                _loginAsyncHandler = loginAsyncHandler ?? throw new ArgumentNullException(nameof(loginAsyncHandler));
                _moveHandler = moveHandler ?? throw new ArgumentNullException(nameof(moveHandler));
            }

            public ValueTask<LoginReply> LoginAsync(LoginRequest req)
            {
                return _loginAsyncHandler(req);
            }

            public ValueTask Move(MoveRequest req)
            {
                return _moveHandler(req);
            }

        }

        public static void Bind(RpcServer server, Func<IPlayerServiceCallback, IPlayerService> implFactory)
        {
            if (implFactory is null) throw new ArgumentNullException(nameof(implFactory));
            var callback = new PlayerServiceCallbackProxy(server);
            var impl = implFactory(callback) ?? throw new InvalidOperationException("Service implementation factory returned null.");
            Bind(server, impl);
        }

        public static void Bind(RpcServer server, IPlayerService impl)
        {
            server.Register(ServiceId, 1, async (req, ct) =>
            {
                var arg1 = server.Serializer.Deserialize<LoginRequest>(req.Payload.AsSpan())!;
                var resp = await impl.LoginAsync(arg1);
                return new RpcResponseEnvelope { RequestId = req.RequestId, Status = RpcStatus.Ok, Payload = server.Serializer.Serialize(resp) };
            });

            server.Register(ServiceId, 2, async (req, ct) =>
            {
                var arg1 = server.Serializer.Deserialize<MoveRequest>(req.Payload.AsSpan())!;
                await impl.Move(arg1);
                return new RpcResponseEnvelope { RequestId = req.RequestId, Status = RpcStatus.Ok, Payload = Array.Empty<byte>() };
            });

        }
    }
}
