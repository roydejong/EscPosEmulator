using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ReceiptPrinterEmulator.Logging;

namespace ReceiptPrinterEmulator.Networking;

public class NetServer
{
    public IPEndPoint EndPoint { get; private set; }
    
    private Socket? _tcpSocket;
    private CancellationTokenSource? _cts;
    
    private List<NetClient> _clients;

    public bool IsRunning => _tcpSocket is not null && _tcpSocket.IsBound;

    public NetServer(int port)
    {
        EndPoint = new IPEndPoint(IPAddress.Any, port);
        
        _clients = new();
    }
    
    public async Task Run()
    {
        Stop();
        
        Logger.Info($"Starting NetServer on TCP port {EndPoint.Port}");
        
        _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _tcpSocket.Bind(EndPoint);
        _tcpSocket.Listen(8);
        
        Logger.Info($"Server bound to {EndPoint}, starting accept/receive loop");
        
        _cts = new CancellationTokenSource();
        await AcceptLoopAsync(_cts.Token);
    }

    public void Stop()
    {
        if (_cts is not null)
        {
            _cts.Cancel();
            _cts = null;
        }
    }

    private async Task AcceptLoopAsync(CancellationToken cancellationToken)
    {
        while (IsRunning && !cancellationToken.IsCancellationRequested)
        {
            var clientSocket = await _tcpSocket!.AcceptAsync(cancellationToken);
            
            if (!clientSocket.Connected)
                continue;

            var client = new NetClient(this, clientSocket);
            _clients.Add(client);
            
            Logger.Info($"Accepted new connection (RemoteEndPoint={client.RemoteEndPoint})");
            
            _ = client.ReceiveLoopAsync();
        }
    }
}