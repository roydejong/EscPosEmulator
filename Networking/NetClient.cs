using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReceiptPrinterEmulator.Logging;

namespace ReceiptPrinterEmulator.Networking;

public class NetClient
{
    public readonly NetServer Server;
    public readonly EndPoint RemoteEndPoint;
    
    private Socket _socket;
    private CancellationTokenSource _lifetimeCts;

    public bool IsConnected => _socket.Connected;
    
    public NetClient(NetServer server, Socket clientSocket)
    {
        Server = server;
        RemoteEndPoint = clientSocket.RemoteEndPoint!;
        
        _socket = clientSocket;
        _lifetimeCts = new();
    }

    public void Close()
    {
        if (!_lifetimeCts.IsCancellationRequested)
            _lifetimeCts.Cancel();
        
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
        
        Logger.Info($"Closed client connection {RemoteEndPoint}");
    }

    public async Task ReceiveLoopAsync()
    {
        try
        {
            var receiveBuffer = GC.AllocateArray<byte>(1024, true);
            var bufferMemory = receiveBuffer.AsMemory();

            while (!_lifetimeCts.Token.IsCancellationRequested)
            {
                var byteCount = await _socket.ReceiveAsync(bufferMemory, SocketFlags.None);

                if (byteCount <= 0)
                {
                    Close();
                    return;
                }

                Logger.Info($"Received TCP data (byteCount={byteCount}, RemoteEndPoint={RemoteEndPoint})");

                HandleIncomingData(bufferMemory.Span[..byteCount]);
            }
        }
        catch (Exception ex)
        {
            Logger.Exception(ex, "Receive error");
            Close();
        }
    }

    private static void HandleIncomingData(ReadOnlySpan<byte> data) =>
        App.Printer?.FeedEscPos(Encoding.ASCII.GetString(data));
}