// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer;
using System.Net.Http;

internal class Server
{
    public static void ThreadFunction(object? obj)
    {
        var sock = (Socket)obj!;
        var bytesReceived = new byte[100000000];
        var data = "";
        var bytesRead = sock.Receive(bytesReceived, bytesReceived.Length, 0);
        data += Encoding.ASCII.GetString(bytesReceived, 0, bytesRead);
        
        Console.WriteLine(data);

        var lines = data.Split();

        var req = new HttpRequest();
        var res = new HttpResponse(sock);
        var uploadServlet = new FileUploadServlet();

        if (lines[0].StartsWith("GET"))
        {
            req.method = HttpRequest.Method.GET;
            req.url = "/";
            uploadServlet.DoGet(req, res);
        }
        else if (lines[0].StartsWith("POST"))
        {
            req.method = HttpRequest.Method.POST;
            req.url = "/";
            // Parse Multipart Form Data
            var parser = new MultipartParser(data, req);
            parser.ParseMultipartData();

            uploadServlet.DoPost(req, res);
        }

        sock.Close();
    }

    public static void Main(string[] args)
    {
        try
        {
            const int port = 8000;
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var ipEndpoint = new IPEndPoint(ipAddress, port);

            var socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndpoint);
            socket.Listen(10);
            
            ThreadPool.SetMaxThreads(Environment.ProcessorCount, 0);

            while (true)
            {
                var connectionSock = socket.Accept();
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadFunction), connectionSock);
                // var thread = new Thread(ThreadFunction);
                // thread.Start(connectionSock);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("Socket exception: {0}", e.Message);
        }
    }
}