using System.Net;
using System.Net.Sockets;
using Shared.Interfaces.Server.Generated;
using Server.Services;
using ULinkRPC.Core;
using ULinkRPC.Server;
using ULinkRPC.Serializer.MemoryPack;
using ULinkRPC.Transport.Tcp;

const int defaultTcpPort = 20000;
var tcpPort = defaultTcpPort;
if (args.Length > 0 && int.TryParse(args[0], out var p))
    tcpPort = p;
using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

Console.WriteLine($"RpcCall Server TCP listening on 0.0.0.0:{tcpPort}. Press Ctrl+C to stop.");

var tcpTask = RunTcpListenerAsync(tcpPort, cts.Token);

try
{
    await tcpTask.ConfigureAwait(false);
}
finally
{
    Console.WriteLine("Server stopped.");
}

async Task RunTcpListenerAsync(int port, CancellationToken hostCt)
{
    var listener = new TcpListener(IPAddress.Any, port);
    listener.Start();

    try
    {
        while (!hostCt.IsCancellationRequested)
        {
            TcpClient client;
            try
            {
                client = await listener.AcceptTcpClientAsync(hostCt).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            var transport = new TcpServerTransport(client);
            _ = RunConnectionAsync(transport, client.Client.RemoteEndPoint?.ToString() ?? "?", hostCt);
        }
    }
    finally
    {
        listener.Stop();
    }
}

async Task RunConnectionAsync(ITransport transport, string remote, CancellationToken hostCt)
{
    RpcServer? server = null;

    try
    {
        var serializer = new MemoryPackRpcSerializer();
        server = new RpcServer(transport, serializer);

        // AllServicesBinder.BindAll(server, new MyFirstService());
        MyFirstServiceBinder.Bind(server, callback => new MyFirstService(callback));
        await server.StartAsync(hostCt).ConfigureAwait(false);
        await server.WaitForCompletionAsync().ConfigureAwait(false);
    }
    catch (OperationCanceledException)
    {
        // Host shutdown
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[{remote}] Error: {ex}");
    }
    finally
    {
        if (server is not null)
            await server.StopAsync().ConfigureAwait(false);

        await transport.DisposeAsync().ConfigureAwait(false);
    }

    Console.WriteLine($"[{remote}] Disconnected.");
}

