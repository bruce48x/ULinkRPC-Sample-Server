using Shared.Interfaces;
using ULinkRPC.Runtime;

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
