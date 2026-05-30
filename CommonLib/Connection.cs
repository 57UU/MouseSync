using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CommonLib;

public class Connection : IDisposable
{
    public Exception? Exception { get; private set; }
    public static readonly string EOF = DataExchange.EOF;
    public event onError? onError;
    public static int BufferSize { set; get; } = 4096;

    private bool _disposed = false;
    public bool IsConnected => !_disposed && TCPclient.Connected;

    public static Connection connect(string ip, int port, MessageHandler hander)
    {
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Parse(ip), port);
        Connection conn = new Connection(client, hander);
        return conn;
    }
    public delegate void MessageHandler(string msg);

    public readonly TcpClient TCPclient;
    private NetworkStream stream;
    public MessageHandler messageHandler { get; set; }
    public Connection(TcpClient client, MessageHandler hander) : this(client)
    {
        this.messageHandler = hander;
    }

    public Connection(TcpClient client)
    {
        this.TCPclient = client;
        stream = client.GetStream();
    }
    public void StartReceive()
    {
        receiveTask = receive();
    }
    public void close()
    {
        if (_disposed) return;
        _disposed = true;
        stream.Close();
        TCPclient.Close();
    }

    public void Dispose()
    {
        close();
    }

    public Task<int> receiveTask { get; private set; }
    public virtual async Task sendAsync(string msg)
    {
#if DEBUG
        Console.WriteLine("Sending: " + msg);
#endif
        byte[] responseData = Encoding.UTF8.GetBytes(msg + EOF);
        await stream.WriteAsync(responseData, 0, responseData.Length);
        await stream.FlushAsync();
    }

    /// <summary>
    /// 同步发送包装，调用方无需 async/await 即可使用。
    /// 内部捕获异常避免 async void 崩溃进程。
    /// 连接已断开时通过 onError 上报。
    /// </summary>
    public void send(string msg)
    {
        if (_disposed)
        {
            onError?.Invoke(new ObjectDisposedException(nameof(Connection)));
            return;
        }
        _ = Task.Run(async () =>
        {
            try
            {
                await sendAsync(msg);
            }
            catch (Exception ex)
            {
                Exception = ex;
                onError?.Invoke(ex);
            }
        });
    }

    private async Task<int> receive()
    {
        string pending = string.Empty;
        try
        {
            byte[] buffer = new byte[BufferSize];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string dataReceived = pending + Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var parts = dataReceived.Split(EOF);
                // 最后一个元素是不完整的消息片段（或空串），保留到下次
                pending = parts[^1];
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    if (parts[i].Length > 0)
                    {
                        messageHandler?.Invoke(parts[i]);
                    }
                }
            }
            // 连接正常关闭，处理 pending 中残留的最后一段完整消息
            if (pending.Length > 0)
            {
                messageHandler?.Invoke(pending);
            }
        }
        catch (TaskCanceledException)
        {
            return 0;
        }
        catch (Exception ex)
        {
            Exception = ex;
            onError?.Invoke(ex);
            return -1;
        }
        return 0;
    }
}


public delegate void ConnectionHandler(Connection connection);
public delegate void onError(Exception e);
public class ConnectionServer : IDisposable
{
    ConnectionHandler handler;
    TcpListener server;
    public Thread thread { get; private set; }

    TcpListener? server_v6;
    public Thread? thread_v6 { get; private set; }

    public event onError OnError;


    /// <summary>
    /// Build IPv4 Server
    /// </summary>
    /// <param name="port"></param>
    /// <param name="handler">How to deal with the connection</param>
    public ConnectionServer(int port, ConnectionHandler handler) : this(port, handler, isEnableIPv6: true) { }

    public ConnectionServer(int port, ConnectionHandler handler, bool isEnableIPv6)

    {
        this.handler = handler;
        if (isEnableIPv6)
        {
            Console.WriteLine("IPv6 server has been enabled");
            server_v6 = new TcpListener(IPAddress.IPv6Any, port);
            server_v6.Start();
            thread_v6 = new(() => Start(server_v6));
            thread_v6.Start();
        }

        server = new TcpListener(IPAddress.Any, port);
        server.Start();
        thread = new(() => Start(server));
        thread.Start();
    }


    public void close()
    {
        server_v6?.Stop();
        server.Stop();
        thread_v6?.Interrupt();
        thread.Interrupt();
    }

    public void Dispose()
    {
        close();
    }
    private void Start(TcpListener _server)
    {
        Console.WriteLine($"Network Server {_server.LocalEndpoint.AddressFamily} Start");
        while (true)
        {
            try
            {
                TcpClient client = _server.AcceptTcpClient();
                handler.Invoke(new Connection(client));
            }
            catch (ThreadInterruptedException)
            {
                break;
            }
            catch (InvalidOperationException)
            {
                break;
            }
            catch (Exception e)
            {
                OnError?.Invoke(e);
            }
        }
        Console.WriteLine("Network Server Shutdown");
    }
}

