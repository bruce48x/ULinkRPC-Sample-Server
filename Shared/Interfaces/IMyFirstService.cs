using System.Threading.Tasks;
using ULinkRPC.Runtime;

namespace Shared.Interfaces
{
    [RpcService(1)]
    public interface IMyFirstService
    {
        [RpcMethod(1)]
        ValueTask<int> SumAsync(int x, int y);
    }
}