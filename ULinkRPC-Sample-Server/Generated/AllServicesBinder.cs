using Shared.Interfaces;
using ULinkRPC.Server;

namespace Shared.Interfaces.Server.Generated
{
    public static class AllServicesBinder
    {
        public static void BindAll(RpcServer server, IMyFirstService myFirstService)
        {
            MyFirstServiceBinder.Bind(server, myFirstService);
        }
    }
}
