using System.Net.Sockets;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
HttpServerses httpServerses = new HttpServerses();
httpServerses.HttpInitialize();
app.Run();


class HttpServerses
{
    public async void HttpInitialize()
    {
        ServerObject server = new ServerObject();// ������� ������
        await server.ListenAsync(); // ��������� ������
        Console.WriteLine("Mission Complate");
    }
}
class ServerObject
{
    TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888); // ������ ��� �������������
    List<ClientObject> clients = new List<ClientObject>(); // ��� �����������
    protected internal void RemoveConnection(string id)
    {
        // �������� �� id �������� �����������
        ClientObject? client = clients.FirstOrDefault(c => c.Id == id);
        // � ������� ��� �� ������ �����������
        if (client != null) clients.Remove(client);
        client?.Close();
    }
    // ������������� �������� �����������
    protected internal async Task ListenAsync()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("������ �������. �������� �����������...");

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                ClientObject clientObject = new ClientObject(tcpClient, this);
                clients.Add(clientObject);
                await Task.Run(clientObject.ProcessAsync);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    // ���������� ��������� ������������ ��������
    protected internal async Task BroadcastMessageAsync(string message, string id)
    {
        foreach (var client in clients)
        {
            if (client.Id != id) // ���� id ������� �� ����� id �����������
            {
                await client.Writer.WriteLineAsync(message); //�������� ������
                await client.Writer.FlushAsync();
            }
        }
    }
    // ���������� ���� ��������
    protected internal void Disconnect()
    {
        foreach (var client in clients)
        {
            client.Close(); //���������� �������
        }
        tcpListener.Stop(); //��������� �������
    }
}
class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get; }
    protected internal StreamReader Reader { get; }

    TcpClient client;
    ServerObject server; // ������ �������

    public ClientObject(TcpClient tcpClient, ServerObject serverObject)
    {
        client = tcpClient;
        server = serverObject;
        // �������� NetworkStream ��� �������������� � ��������
        var stream = client.GetStream();
        // ������� StreamReader ��� ������ ������
        Reader = new StreamReader(stream);
        // ������� StreamWriter ��� �������� ������
        Writer = new StreamWriter(stream);
    }

    public async Task ProcessAsync()
    {
        try
        {
            // �������� ��� ������������
            string? userName = await Reader.ReadLineAsync();
            string? message = $"{userName} ����� � ���";
            // �������� ��������� � ����� � ��� ���� ������������ �������������
            await server.BroadcastMessageAsync(message, Id);
            Console.WriteLine(message);
            // � ����������� ����� �������� ��������� �� �������
            while (true)
            {
                try
                {
                    message = await Reader.ReadLineAsync();
                    if (message == null) continue;
                    message = $"{userName}: {message}";
                    Console.WriteLine(message);
                    await server.BroadcastMessageAsync(message, Id);
                }
                catch
                {
                    message = $"{userName} ������� ���";
                    Console.WriteLine(message);
                    await server.BroadcastMessageAsync(message, Id);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            // � ������ ������ �� ����� ��������� �������
            server.RemoveConnection(Id);
        }
    }
    // �������� �����������
    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        client.Close();
    }
}